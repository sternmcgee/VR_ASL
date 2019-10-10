using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonController : MonoBehaviour
{
    LetterController LetterDisplay;
    public char[] LessonPlan;
    private int LessonIndex = 0;


    void Start()
    {
        LetterDisplay = this.GetComponent<LetterController>();
        LetterDisplay.Display(LessonPlan[0]);
    }


    private void Update()
    {
        if(Input.GetKeyDown(LessonPlan[LessonIndex].ToString().ToLower()))
        {
            LessonIndex++;
            if(LessonIndex < LessonPlan.Length)
            {
                LetterDisplay.Display(LessonPlan[LessonIndex]);
            } else
            {
                
            }
        }
    }
}
