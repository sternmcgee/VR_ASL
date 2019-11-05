using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Core;

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
        score = 0;

        if(allLetters.Length > 4)
        {
            Debug.Log("Cannot use more than 4 letters; array will be truncated");
        }

        for(int i = 0; i < 4 && i < allLetters.Length; i++)
        {
            rings[i].GetComponent<RingController>().AssignLetter(allLetters[i]);
        }

        SpawnCube(false);
    }
    

    public void SpawnCube(bool increaseScore)
    {
        if(increaseScore)
        {
            score += 1;
            Debug.Log("Score: " + score);
        }

        char nextLetter = allLetters[Random.Range(0,4)];
        GameObject newCube = Instantiate(cube);
        newcube.transform.position = spawnPoint.transform.position;

        newCube.GetComponent<HandCube>().AsssignLetter(nextLetter, images[nextLetter-'A'], this);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            other.transform.parent.position = spawnPoint.transform.position;
            other.transform.parent.rotation = spawnPoint.transform.rotation;
        }
    }
}
