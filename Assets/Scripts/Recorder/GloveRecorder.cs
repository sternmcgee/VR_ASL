using UnityEngine;
using System;
using System.Collections;
using System.IO;
using HI5;

public class GloveRecorder : MonoBehaviour
{
    private enum Gesture { None, A, B, C, D, E, F, G, H, I, J, K, L,
                            M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z }

    private HI5_Source handSource;

    private StreamWriter writer;

    private bool recording = false;     // we may or may not need this, let's see how it reacts in Update()...

    private string dataPath = "../Data/";


    // Set up file for recording
    private void initializeWriter(Hand hand, Gesture gesture)
    {
        writer = new StreamWriter(dataPath + hand + "_" + gesture + "_" + DateTime.Now + ".csv");
        string header = "";

        for(int i = 0; i < Enum.GetNames(typeof(Bones)).Length; ++i)
        {
            string[] cord = {"x", "y", "z", "w"};

            for(int j = 0; i < 3; ++i)
            {
                if(i == 0 && j == 0) { header += (Bones)i + "_pos" + cord[j]; }
                else { header += "," + (Bones)i + "_pos" + cord[j]; }
            }
            foreach(string s in cord)
            {
                header += "," + "_quot" + (Bones)i + s;
            }
        }
        header += ",gesture";
        writer.WriteLine(header);

        writeData(hand, gesture);
    }


    // Write hand data to csv file
    private void writeData(Hand hand, Gesture gesture)
    {
        recording = true;
        string data = "";

        for(int i = 0; i < Enum.GetNames(typeof(Bones)).Length; ++i)
        {
            Vector3 bonePos = handSource.GetReceivedPosition(i, hand);
            Vector3 boneRot = handSource.GetReceivedRotation(i, hand);

            for(int j = 0; j < 3; ++j)
            {
                if (i == 0 && j == 0) { data += bonePos[j].ToString(); }
                else { data += "," + bonePos[j].ToString(); }
            }
            for (int j = 0; j < 4; ++j)
            {
                data += boneRot[j].ToString();
            }
        }

        data += "," + gesture;
        writer.WriteLine(data);
        writer.Close();

        recording = false;
    }


    // Use this for initialization
    void Start()
    {

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
