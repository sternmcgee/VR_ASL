using UnityEngine;
using System;
using System.Collections;
using System.IO;
using HI5;


public class GloveRecorder : MonoBehaviour
{
    private enum Gesture { None, A, B, C, D, E, F, G, H, I, J, K, L,
                            M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z }

    private HI5_Glove_TransformData_Interface handInterface;

    private StreamWriter writer;

    private bool recording = false;     // we may or may not need this, let's see how it reacts in Update...

    private string dataPath = "Assets/Scripts/Data/";


    // Set up file for recording
    private void initializeWriter(Hand hand, Gesture gesture)
    {
        recording = true;

        writer = new StreamWriter(dataPath + hand.ToString() + "_" + gesture.ToString() +
                                    "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".csv");
        string header = "";

        for (int i = 0; i < (int)Bones.NumOfHI5Bones - 1; ++i)
        {
            string[] cord = { "x", "y", "z", "w" };

            for (int j = 0; j < 3; ++j)
            {
                if (i == 0 && j == 0)
                {
                    header += (HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i + "_pos" + cord[j];
                }
                else
                {
                    header += "," + (HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i + "_pos" + cord[j];
                }
            }
            foreach (string s in cord)
            {
                header += "," + "_quad" + (HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i + s;
            }
            header += ",gesture";
        }
        writer.WriteLine(header);
        writeData(hand, gesture);
    }


    // Write hand data to csv file
    private void writeData(Hand hand, Gesture gesture)
    {
        // record gesture 100 times
        for( int x = 0; x < 100; ++x )
        {
            string data = "";
            for (int i = 0; i < (int)Bones.NumOfHI5Bones - 1; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i == 0 && j == 0)
                    {
                        data += handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localPosition[j];
                    }
                    else
                    {
                        data += "," + handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localPosition[j];
                    }
                }
                for (int j = 0; j < 4; ++j)
                {
                    data += "," + handInterface.GetRightHandTransform()[(HI5_Glove_TransformData_Interface.EHi5_Glove_TransformData_Bones)i].localRotation[j];
                }
            }

            data += "," + gesture.ToString();
            writer.WriteLine(data);
        }

        writer.Close();
        recording = false;
    }


    // Use this for initialization
    void Start()
    {
        handInterface = HI5_Glove_TransformData_Interface.Instance;
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.R))        //R for record
        {
            if (!recording)
            {
                // JUST FOR TESTING; we'll add adjustments for specifiations
                Hand hand = Hand.RIGHT;
                Gesture gesture = Gesture.None;

                initializeWriter(hand, gesture);
            }
        }
    }

}
