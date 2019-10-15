using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject LetterPrefab;
    public GameObject[] SpawnPoints;    //list of spawn points
    public string[] AllLetters;     //list of letters to spawn
    public int NumSpawns;

    private GameObject[] spawnedObjects;
    private string[] spawnedLetters;

    
    void Start()
    {
        spawnedObjects = new GameObject[NumSpawns];
        spawnedLetters = new string[NumSpawns];

        Debug.Log(spawnedLetters[0]);

        StartCoroutine(RunGame());
    }


    private IEnumerator RunGame()
    {
        Debug.Log("Started");
        for(int i = 0; i<NumSpawns; i++)
        {
            //Debug.Log("Spawning letter");
            //create letter
            GameObject newLetter = Object.Instantiate(LetterPrefab);
            spawnedObjects[i] = newLetter;
            SimpleHelvetica newScript = newLetter.GetComponent<SimpleHelvetica>();
            string letter = AllLetters[Random.Range(0, AllLetters.Length)];
            newScript.Text = letter;
            spawnedLetters[i] = letter;
            newScript.GenerateText();

            //add random drag and place on a spawn point
            newScript.Drag = Random.Range(0.1f, 1.0f);
            newScript.SetRigidbodyVariables();
            newLetter.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
            newLetter.SetActive(true);

            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));
        }

        yield return null;
    }

    
    void Update()
    {
        for(int i = 0; i < spawnedLetters.Length; i++)
        {
            string str = spawnedLetters[i];
            if(str != null)
            {
                if(Input.GetKeyDown(str.ToLower()))
                {
                    Destroy(spawnedObjects[i]);
                    spawnedLetters[i] = null;
                    return;
                }
            }
        }
    }
}
