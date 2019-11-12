﻿/* GestureRecognizer.cs
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
    private enum Gesture { None, A, B, C, D, E, F, G, H, I, J, K, L,
                            M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z }

    private HI5_Glove_TransformData_Interface handInterface;

    // Methods for recognition
    private KNearestNeighbors knn;
    private MultilabelSupportVectorMachine<Gaussian> svm;


    private string dataPath = "Assets/Scripts/Data/";

    private static double[][] train_inputs;
    private static int[] train_outputs;

    private static int knn_gesture;
    private static int svm_gesture;

    //for access outside this class
    public int KNNgesture
    {
        get { return knn_gesture; }
    }
    public int SVMgesture
    {
        get { return svm_gesture; }
    }


    //Read training data into train_input and train_output
    private static void readData(string path)
    {
        List<List<double>> inputs = new List<List<double>>();
        List<Gesture> outputs = new List<Gesture>();

        var files = Directory.EnumerateFiles(path, "*.csv");
        foreach(string file in files)
        {
            using (var reader = new StreamReader(file))
            {
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

                    //parse gesture and add to list
                    Gesture gesture;
                    Enum.TryParse(values[values.Length - 1], out gesture);
                    outputs.Add(gesture);
                }
            }
        }

        train_inputs = inputs.Select(x => x.ToArray()).ToArray();
        train_outputs = outputs.Select(x => (int)x).ToArray();      //tranform gestures into integers representations
    }

    private double[] getHandData()
    {
        List<double> data = new List<double>();

        for (int i = 0; i < (int)Bones.NumOfHI5Bones - 1; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                data.Add(handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localPosition[j]);
            }
            for (int j = 0; j < 4; ++j)
            {
                data.Add(handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localRotation[j]);
            }
        }

        return data.ToArray();
    }

    // Use this for initialization
    void Start()
    {
        handInterface = HI5_Glove_TransformData_Interface.Instance;

        readData(dataPath);

        //set up and train recognizer(s)
        knn = new KNearestNeighbors(k: 9);      //TEST FOR K
        knn.Learn(train_inputs, train_outputs);

        var teacher = new MultilabelSupportVectorLearning<Gaussian>()
        {
            Learner = (p) => new SequentialMinimalOptimization<Gaussian>()
            {
                UseKernelEstimation = true
            }
        };

        svm = teacher.Learn(train_inputs, train_outputs);
    }

    // Update is called once per frame
    void Update()
    {
        // Get data from gloves
        double[] data = getHandData();

        /* Make sure we have collected data before we run this!!! */

        // Recognize gesture for knn
        knn_gesture = knn.Decide(data);
        Debug.Log("kNN gesture: " + knn_gesture);

        // Recognize gesture for svm
        foreach( Gesture gesture in Enum.GetValues(typeof(Gesture)) )
        {
            if(svm.Decide(data, (int)gesture))
            {
                svm_gesture = (int)gesture;
                break;
            }
        }
        Debug.Log("SVM gesture: " + svm_gesture);

    }
}
