using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    public GameObject letterObject;
    private char letter;
    private SimpleHelvetica letterScript;

    public void AssignLetter(char c)
    {
        letter = c;
        letterScript = letterObject.GetComponent<SimpleHelvetica>();
        letterScript.Text = c.ToString();
        letterScript.GenerateText();
    }

    public char GetLetter()
    {
        return letter;
    }
}
