using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonController : MonoBehaviour
{
    LetterController letterDisplay;
    public char[] lessonPlan;
    private int lessonIndex = 0;


    void Start()
    {
        letterDisplay = this.GetComponent<LetterController>();
        letterDisplay.Display(lessonPlan[0]);
    }


    private void Update()
    {
        if(Input.GetKeyDown(lessonPlan[lessonIndex].ToString().ToLower()))
        {
            lessonIndex++;
            if(lessonIndex < lessonPlan.Length)
            {
                letterDisplay.Display(lessonPlan[lessonIndex]);
            } else
            {
                
            }
        }
    }


    public void Reset(char[] newPlan = null)
    {
        if(newPlan != null)
        {
            lessonPlan = newPlan;
        }

        lessonIndex = 0;
        letterDisplay.Display(lessonPlan[0]);
    }
}
