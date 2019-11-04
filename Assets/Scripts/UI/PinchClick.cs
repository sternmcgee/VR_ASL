using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Interface;
using HI5;

public class PinchClick : MonoBehaviour
{
    private Hi5_Interface_Hand leftHand;
    private Hi5_Interface_Hand rightHand;

    // Start is called before the first frame update
    void Start()
    {
        if (leftHand == null)
            Debug.Log("Warning: Left hand missing reference.");
        if (rightHand == null)
            Debug.Log("Warning: Right hand missing reference.");
    }

    // Update is called once per frame
    void Update()
    {

    }
}