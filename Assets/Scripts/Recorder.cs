using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;



namespace HI5
{
    public class Recorder : MonoBehaviour
    {
        public GameObject player;
        private HI5_InertiaInstance instance;
        private HI5_GloveStatus m_Status;
        private HI5_Instance instance1;
        private HI5.HI5_Glove_TransformData_Interface asdf;
        HI5_Source source;




        //private HI5_TransformInstance leftHand;


        void Start()
        {
            m_Status = HI5_Manager.GetGloveStatus();


            Debug.Log("yeet");
            transform.position = player.transform.position - Vector3.forward * 100f;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("/Users/jackshirley/Documents/data/data.csv", true))
            {
                file.WriteLine(transform.position);
            }
            Debug.Log(source.GetReceivedRotation(1, Hand.RIGHT));
            Debug.Log("yeet2");
            //Debug.Log(HI5_Manager.GetGloveStatus());




        }

        void Update()
        {

            Debug.Log(HI5.HI5_Manager.GetGloveStatus().ToString());
        }









    }

    
}