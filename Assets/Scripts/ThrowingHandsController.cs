using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingHandsController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] rings;
    public char[] allLetters;
    public int numSpawns;
    private int score;

    public GameObject popfx;
    public Material[] images;
    public GameObject cube;
    

    void Start()
    {
        if(allLetters.Length > 4)
        {
            Debug.Log("Cannot use more than 4 letters; array will be truncated");
        }

        for(int i = 0; i < 4 && i < allLetters.Length; i++)
        {
            rings[i].GetComponent<RingController>().AssignLetter(allLetters[i]);
        }
    }
    
    void Update()
    {
        
    }
}
