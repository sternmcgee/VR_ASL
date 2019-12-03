/* GestureRecognizer.cs
 *  VR_ASL
 *
 * This script recognizes the gesture make by the player in-game in real-time.
 * Uses Accord.Net machine learning framework to decide on gesture.
 *
 * AUTHORS: Jack Belcher, (put your name here if you edited it :) )
 *
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HI5;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;


public class GestureRecognizer : MonoBehaviour
{
    private enum Gesture
    {
        None, A, B, C, D, E, F, G, H, I, J, K, L,
        M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }

    private HI5_Glove_TransformData_Interface handInterface;

    // Methods for recognition
    //private KNearestNeighbors knn = null;
    //private MultilabelSupportVectorMachine<Linear> svm = null;
    private MultilabelSupportVectorMachine<Linear> svmLeft = null;
    private MultilabelSupportVectorMachine<Linear> svmRight = null;

    //private string dataPath = "Assets/Scripts/Data/";
    private string leftDataPath = "Assets/Scripts/Data/Left/";
    private string rightDataPath = "Assets/Scripts/Data/Right/";

    //private static double[][] train_inputs;
    //private static int[] train_outputs;
    private static double[][] leftTrainInputs;
    private static int[] leftTrainOutputs;
    private static double[][] rightTrainInputs;
    private static int[] rightTrainOutputs;


    // Read training data into train_input and train_output
    /*private static void readData(string path)
    {
        List<List<double>> inputs = new List<List<double>>();
        List<Gesture> outputs = new List<Gesture>();

        var files = Directory.EnumerateFiles(path, "*.csv");
        foreach(string file in files)
        {
            using (var reader = new StreamReader(file))
            {
                var header = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    List<double> entry = new List<double>();
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    for (int i = 0; i < values.Length - 1; ++i)      //omit the label value
                    {
                        entry.Add(Convert.ToDouble(values[i]));
                    }

                    inputs.Add(entry);

                    // parse gesture and add to list
                    Gesture gesture;
                    Enum.TryParse(values[values.Length - 1], out gesture);
                    outputs.Add(gesture);
                }
            }
        }

        train_inputs = inputs.Select(x => x.ToArray()).ToArray();
        train_outputs = outputs.Select(x => (int)x).ToArray();      //tranform gestures into integers representations
    }*/

    private static void readData(string path, Hand hand)
    {
        List<List<double>> inputs = new List<List<double>>();
        List<Gesture> outputs = new List<Gesture>();

        var files = Directory.EnumerateFiles(path, "*.csv");
        foreach (string file in files)
        {
            using (var reader = new StreamReader(file))
            {
                var header = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    List<double> entry = new List<double>();
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    for (int i = 0; i < values.Length - 1; ++i)      //omit the label value
                    {
                        entry.Add(Convert.ToDouble(values[i]));
                    }

                    inputs.Add(entry);

                    // parse gesture and add to list
                    Gesture gesture;
                    Enum.TryParse(values[values.Length - 1], out gesture);
                    outputs.Add(gesture);
                }
            }
        }

        if(hand == Hand.LEFT)
        {
            leftTrainInputs = inputs.Select(x => x.ToArray()).ToArray();
            leftTrainOutputs = outputs.Select(x => (int)x).ToArray();
        }
        else if(hand == Hand.RIGHT)
        {
            rightTrainInputs = inputs.Select(x => x.ToArray()).ToArray();
            rightTrainOutputs = outputs.Select(x => (int)x).ToArray();
        }
    }

    // Get current hand sensor readings
    private double[] getHandData(Hand hand)
    {    
        List<double> data = new List<double>();

        //return if hand not assigned in start() yet
        if (handInterface == null)
            return data.ToArray();

        for (int i = 0; i < (int)Bones.NumOfHI5Bones - 1; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (hand == Hand.RIGHT)
                {
                    data.Add(handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localPosition[j]);
                }
                else
                {
                    data.Add(handInterface.GetLeftHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localPosition[j]);
                }
            }
            for (int j = 0; j < 4; ++j)
            {
                if (hand == Hand.RIGHT)
                {
                    data.Add(handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localRotation[j]);
                }
                else
                {
                    data.Add(handInterface.GetLeftHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localRotation[j]);
                }
            }
        }

        return data.ToArray();
    }

    // Get current gesture (via kNN approach)
    /*public int knnGetGesture(Hand hand)
    {
        double[] data = getHandData(hand);
        return knn.Decide(data);
    }*/

    // Get current gesture (via SVM approach; lame)
    public int svmGetGesture(Hand hand)
    {
        double[] data = getHandData(hand);
        foreach(Gesture gesture in Enum.GetValues(typeof(Gesture)))
        {
            if (hand == Hand.LEFT)
            {
                if (svmLeft.Decide(data, (int)gesture))
                {
                    return (int)gesture;
                }
            }
            else if (hand == Hand.RIGHT)
            {
                if (svmRight.Decide(data, (int)gesture))
                {
                    return (int)gesture;
                }
            }
        }
        return 0;       // return 'None' if nothing works
    }

    // Get kNN decision response; return true if gesture matches
    /*public bool knnIsGesutre(Hand hand, char gesture)
    {
        double[] data = getHandData(hand);
        Gesture g;
        Enum.TryParse(gesture.ToString(), out g);

        return g == (Gesture)(knn.Decide(data));
    }*/

    // Get SVM  decision response; return true if gesture matches
    public bool svmIsGesture(Hand hand, char gesture)
    {
        double[] data = getHandData(hand);
        Gesture g;
        Enum.TryParse(gesture.ToString(), out g);

        if(hand == Hand.LEFT)
        {
            return svmLeft.Decide(data, (int)g);
        }
        if(hand == Hand.RIGHT)
        {
            return svmRight.Decide(data, (int)g);
        }
        return false;       // default return false
    }


    // Use this for initialization
    void Start()
    {
        handInterface = HI5_Glove_TransformData_Interface.Instance;

        readData(leftDataPath, Hand.LEFT);
        readData(rightDataPath, Hand.RIGHT);

        // Set up and train recognizer(s)
        /*Debug.Log("Knn learning started!");
        knn = new KNearestNeighbors(k: 9);      // TEST FOR K!!!
        knn.Learn(train_inputs, train_outputs);
        Debug.Log("Knn learning function finished!");*/

        var leftTeacher = new MultilabelSupportVectorLearning<Linear>()
        {
            Learner = (p) => new LinearDualCoordinateDescent()
            {
                Loss = Loss.L2
            }
        };

        var rightTeacher = new MultilabelSupportVectorLearning<Linear>()
        {
            Learner = (p) => new LinearDualCoordinateDescent()
            {
                Loss = Loss.L2
            }
        };

        svmLeft = leftTeacher.Learn(leftTrainInputs, leftTrainOutputs);
        svmRight = rightTeacher.Learn(rightTrainInputs, rightTrainOutputs);
    }

    // Update is called once per frame
    void Update()
    {
        // comment this out when not needed
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Debug.Log("kNN right gesture: " + (Gesture)knnGetGesture(Hand.RIGHT));
            Debug.Log("SVM right gesture: " + (Gesture)svmGetGesture(Hand.RIGHT));
        }        
    }
}
