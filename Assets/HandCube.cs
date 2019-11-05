using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCube : MonoBehaviour
{
    private Transform spawnPoint;
    private char letter;
    private ThrowingHandsController controller;

    private void Start()
    {
        spawnPoint = this.transform;
    }

    public void AsssignLetter(char c, Material m, ThrowingHandsController con)
    {
        letter = c;
        GetComponent<Renderer>().material = m;
        controller = con;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ring")
        {
            Debug.Log("Hit ring trigger collider!");
            RingController ring = other.gameObject.GetComponent<RingController>();
            if (ring != null)
            {
                controller.SpawnCube(ring.GetLetter() == letter);

            }
        }
    }
}
