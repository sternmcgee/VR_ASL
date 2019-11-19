using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu2 : MonoBehaviour
{
    public string newGameSceneName;
    public int quickSaveSlotID;

    [Header("Options Panel")]
    public GameObject MainScreenPanel;
    public GameObject StartGameOptionsPanel;
    public GameObject ChoosingGamePanel;
    public GameObject LectureTransition;
    public GameObject FallingLetters;
    public GameObject ThrowingHands;
    public GameObject Calibration;
    
    //public GameObject BackToMainMenu;

    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();
        MainScreenPanel.SetActive(true);

        //new key
        PlayerPrefs.SetInt("quickSaveSlot", quickSaveSlotID);
    }

    #region Open Different panels

    public void openStartGameOptions()
    {
        //enable respective panel
        StartGameOptionsPanel.SetActive(true);
        MainScreenPanel.SetActive(false);

        //play anim for opening main options panel
        //anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        Debug.Log("Start Game Go");

        //enable BLUR
        //Camera.main.GetComponent<Animator>().Play("BlurOn");

    }

    public void openStartGameOptions_Lecture()
    {
        //enable respective panel
        //StartGameOptionsPanel.SetActive(true);
        LectureTransition.SetActive(true);

        //play anim for opening main options panel
        //anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        //enable BLUR
        //Camera.main.GetComponent<Animator>().Play("BlurOn");
    }

    public void openFallingLetters()
    {
        //enable respective panel
        StartGameOptionsPanel.SetActive(false);
        FallingLetters.SetActive(true);

        //play anim for opening main options panel
        //anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        //enable BLUR
        //Camera.main.GetComponent<Animator>().Play("BlurOn");
    }

    public void openThrowingHands()
    {
        //enable respective panel
        StartGameOptionsPanel.SetActive(false);
        ThrowingHands.SetActive(true);

        //play anim for opening main options panel
        //anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        //enable BLUR
        //Camera.main.GetComponent<Animator>().Play("BlurOn");
    }

    public void openLecture()
    {
        //enable respective panel
        StartGameOptionsPanel.SetActive(true);
        LectureTransition.SetActive(true);

        //play anim for opening main options panel
        //anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        //enable BLUR
        //Camera.main.GetComponent<Animator>().Play("BlurOn");
    }

    public void openCalibration()
    {
        //MainScreenPanel.SetActive(false);
        Calibration.SetActive(true);
    }

    public void openStartGameOptions_Games()
    {
        //enable respective panel
        ChoosingGamePanel.SetActive(true);
        StartGameOptionsPanel.SetActive(false);
        MainScreenPanel.SetActive(false);

        //play click sfx
        playClickSound();

    }

    public void newGame()
    {
        
    }
    #endregion

    #region Back Buttons

    public void openBackToMainMenu()
    {
        //enable respective panel
        //StartGameOptionsPanel.SetActive(true);
        MainScreenPanel.SetActive(true);
        StartGameOptionsPanel.SetActive(false);

        //play anim for opening main options panel
        //anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        //enable BLUR
        //Camera.main.GetComponent<Animator>().Play("BlurOn");
    }

    public void back_options()
    {

        //disable BLUR
        // Camera.main.GetComponent<Animator>().Play("BlurOff");

        //play click sfx
        playClickSound();
    }

    public void back_options_panels()
    {

        //play click sfx
        playClickSound();

    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Sounds
    public void playHoverClip()
    {

    }

    void playClickSound()
    {

    }


    #endregion

}
