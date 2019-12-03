using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameController : MonoBehaviour
{
    public GestureRecognizer recognizer;
    public Hand rHand = null;
    public Hi5_Interaction_Core.Hi5_Hand_Visible_Hand visibleHand = null;
    private char returnedGesture = '0';
    public GameObject popfx;
    public GameObject letterPrefab;
    public GameObject[] spawnPoints;    //list of spawn points
    public char[] allLetters;     //list of letters to spawn
    public int numSpawns;

    private GameObject[] spawnedObjects;
    private char[] spawnedLetters;

    public GameObject scoreObject;
    private SimpleHelvetica scoreDisplay;
    private int score;
    private string scoreString = "Score: \n";

    
    void Start()
    {
        spawnedObjects = new GameObject[numSpawns];
        spawnedLetters = new char[numSpawns];
        scoreDisplay = scoreObject.GetComponent<SimpleHelvetica>();
        score = 0;

        StartCoroutine(RunGame());
        StartCoroutine(GestureLoop());
    }


    private IEnumerator GestureLoop()
    {
        float waitTime = 5f / 1000f;

        while(true)
        {

            Debug.Log("Start Loop");
            IDictionary<char, int> readings = new Dictionary<char, int>();
            for(int i = 0; i < allLetters.Length; i++)
            {
                if(spawnedLetters[i] != '0')
                readings[spawnedLetters[i]] = 0;
            }

            for(int i = 0; i < 10; i++)
            {
                foreach(char letter in allLetters)
                {
                    if(recognizer.svmIsGesture(HI5.Hand.RIGHT, letter) )
                    {
                        readings[letter]++;
                    }
                }
                Debug.Log(readings);
                yield return new WaitForSeconds(waitTime);
            }
            
            foreach(char letter in readings.Keys)
            {
                if(readings[letter] > 7)
                {
                    returnedGesture = letter;
                    rHand.TriggerHapticPulse(5000);
                    visibleHand.ChangeColor(Color.green);
                    Debug.Log("Returned letter " + letter);

                    for(int i = 0; i < spawnedLetters.Length; i++)
                    {
                        if(spawnedLetters[i] == letter)
                        {
                            spawnedLetters[i] = '0';
                            StartCoroutine(Pop(spawnedObjects[i]));
                            spawnedObjects[i] = null;
                            score += 1;
                        }
                    }
                } else
                {
                    returnedGesture = '0';
                    visibleHand.ChangeColor(visibleHand.orgColor);
                }
            }

            //yield return null;
            //yield return new WaitForSeconds(waitTime);
        }
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
            char letter = allLetters[Random.Range(0, allLetters.Length)];
            newScript.Text = letter.ToString();
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
        
        //for(int i = 0; i < spawnedLetters.Length; i++)
        //{
        //    string str = spawnedLetters[i];
        //    if(str != null)
        //    {
        //        if(Input.GetKeyDown(str.ToLower()))
        //        {
        //            //Destroy(spawnedObjects[i]);
        //            StartCoroutine(Pop(spawnedObjects[i]));
        //            spawnedObjects[i] = null;
        //            spawnedLetters[i] = null;
        //            score += 1;
        //            return;
        //        }
        //    }
        //}
    }



    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < spawnedObjects.Length; i++)
        {
            if(other.transform.root.gameObject == spawnedObjects[i])
            {
                //Destroy(spawnedObjects[i]);
                StartCoroutine(Pop(spawnedObjects[i]));
                spawnedObjects[i] = null;
                spawnedLetters[i] = '0';
            }
        }
    }


    private IEnumerator Pop(GameObject gameObject)
    {
        Transform letterTransform = gameObject.transform.GetChild(1);
        Instantiate(popfx, letterTransform.position, letterTransform.rotation);

        for(int i=2; i<6; i++)
        {
            letterTransform.localScale /= i;
            //gameObject.transform.Translate(new Vector3(-1 / i, -1 / i, -1 / i));
            yield return null;
        }

        Destroy(gameObject);
    }
}
