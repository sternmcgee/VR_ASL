using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject LetterPrefab;
    public GameObject[] SpawnPoints;
    public string[] AllLetters;
    public int NumSpawns;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunGame());
    }

    private IEnumerator RunGame()
    {
        Debug.Log("Started");
        for(int i = 0; i<NumSpawns; i++)
        {
            Debug.Log("Spawning letter");
            GameObject newLetter = Object.Instantiate(LetterPrefab);
            newLetter.GetComponent<SimpleHelvetica>().Text = AllLetters[Random.Range(0, AllLetters.Length)];
            newLetter.transform.position = SpawnPoints[0].transform.position;
            newLetter.SetActive(true);

            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));
        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
