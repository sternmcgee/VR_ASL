using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    public GameObject letterObject;
    private char letter;
    private SimpleHelvetica letterScript;
    public ThrowingHandsController controller;


    public void AssignLetter(char c)
    {
        letter = c;
        letterScript = letterObject.GetComponent<SimpleHelvetica>();
        letterScript.Text = c.ToString();
        letterScript.GenerateText();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cube")
        {
            Debug.Log("Cube hit ring " + letter);
            controller.ResetCube(ringLetter: letter);
        }
    }
}
