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
}
