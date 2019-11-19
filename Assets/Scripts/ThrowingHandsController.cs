using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Core;

public class ThrowingHandsController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject scoreObject;
    private SimpleHelvetica scoreDisplay;
    public GameObject[] rings;
    public char[] allLetters;

    public int numSpawns;
    private int spawns = 0;
    private int score;
    private int cubeLetterIndex;

    public GameObject popfx;
    public Material[] images;
    public GameObject cube;


    void Start()
    {
        score = 0;
        scoreDisplay = scoreObject.GetComponent<SimpleHelvetica>();

        if(allLetters.Length > 4)
        {
            Debug.Log("Cannot use more than 4 letters; array will be truncated");
        }

        for(int i = 0; i < 4 && i < allLetters.Length; i++)
        {
            rings[i].GetComponent<RingController>().AssignLetter(allLetters[i]);
        }

        cubeLetterIndex = Random.Range(0, 4);
        cube.GetComponent<HandCube>().AssignLetter(allLetters[cubeLetterIndex], images[allLetters[cubeLetterIndex] - 'A'], this);
    }


    public void Update()
    {
        scoreDisplay.Text = "SCORE:\n" + score + "/" + numSpawns;
        scoreDisplay.GenerateText();
    }


    public void ResetCube(char ringLetter)
    {
        //move cube
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        cube.transform.position = spawnPoint.transform.position;
        cube.transform.rotation = spawnPoint.transform.rotation;
        rb.Sleep();


        //spawn new cube
        //GameObject oldCube = cube;
        //cube = Instantiate(oldCube, spawnPoint.transform.position, spawnPoint.transform.rotation);
        //Destroy(oldCube);

        if(ringLetter != '-')
        {
            if(ringLetter == allLetters[cubeLetterIndex])
            {
                score += 1;
            }
            //Debug.Log("Score: " + score);
            spawns++;
            if(spawns >= numSpawns)
            {
                cube.SetActive(false);
            }

            int nextIndex = Random.Range(0, 3);
            if(nextIndex == cubeLetterIndex) { nextIndex = 3; }
            cubeLetterIndex = nextIndex;
            Debug.Log("Changing to letter " + allLetters[cubeLetterIndex]);
            Debug.Log("Total spawns: " + spawns);
            cube.GetComponent<HandCube>().AssignLetter(allLetters[cubeLetterIndex], images[allLetters[cubeLetterIndex] - 'A'], this);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cube")
        {
            Debug.Log("Cube hit the controller");
            ResetCube(ringLetter: '-');
        }
    }
}
