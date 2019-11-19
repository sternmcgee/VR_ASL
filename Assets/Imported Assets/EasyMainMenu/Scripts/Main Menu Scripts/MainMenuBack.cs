using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainMenuTransition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openMainMenu()
    {
        //enable respective panel
        MainMenuTransition.SetActive(true);

    }
}
