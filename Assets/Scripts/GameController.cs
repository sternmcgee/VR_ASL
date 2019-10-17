using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject letterPrefab;
    public GameObject[] spawnPoints;    //list of spawn points
    public string[] allLetters;     //list of letters to spawn
    public int numSpawns;

    private GameObject[] spawnedObjects;
    private string[] spawnedLetters;

    public GameObject scoreObject;
    private SimpleHelvetica scoreDisplay;
    private int score;
    private string scoreString = "Score: \n";

    
    void Start()
    {
        spawnedObjects = new GameObject[numSpawns];
        spawnedLetters = new string[numSpawns];
        scoreDisplay = scoreObject.GetComponent<SimpleHelvetica>();
        score = 0;

        StartCoroutine(RunGame());
    }


    private IEnumerator RunGame()
    {
        Debug.Log("Started");
        for(int i = 0; i<numSpawns; i++)
        {
            //Debug.Log("Spawning letter");
            //create letter
            GameObject newLetter = Object.Instantiate(letterPrefab);
            spawnedObjects[i] = newLetter;
            SimpleHelvetica newScript = newLetter.GetComponent<SimpleHelvetica>();
            string letter = allLetters[Random.Range(0, allLetters.Length)];
            newScript.Text = letter;
            spawnedLetters[i] = letter;
            newScript.GenerateText();

            //add random drag and place on a spawn point
            newScript.Drag = Random.Range(0.1f, 1.0f);
            newScript.SetRigidbodyVariables();
            newLetter.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            newLetter.SetActive(true);

            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));
        }

        yield return null;
    }

    
    void Update()
    {
        scoreDisplay.Text = scoreString + score + '/' + numSpawns;
        scoreDisplay.GenerateText();
        
        for(int i = 0; i < spawnedLetters.Length; i++)
        {
            string str = spawnedLetters[i];
            if(str != null)
            {
                if(Input.GetKeyDown(str.ToLower()))
                {
                    Destroy(spawnedObjects[i]);
                    spawnedLetters[i] = null;
                    score += 1;
                    return;
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < spawnedObjects.Length; i++)
        {
            if(other.transform.root.gameObject == spawnedObjects[i])
            {
                Destroy(spawnedObjects[i]);
                spawnedLetters[i] = null;
            }
        }
    }
}
