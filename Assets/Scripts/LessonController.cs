using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonController : MonoBehaviour
{
    LetterController LetterDisplay;
    public char[] lessonPlan;
    

    void Start()
    {
        LetterDisplay = this.GetComponent<LetterController>();

        StartCoroutine(Lesson());
    }


    private IEnumerator Lesson()
    {
        foreach (char c in lessonPlan)
        {
            yield return StartCoroutine(DisplayAndWait(c));
        }
    }


    private IEnumerator DisplayAndWait(char c)
    {
        LetterDisplay.Display(c);
        string cstr = c.ToString().ToLower();
        while(!Input.GetKeyDown(cstr))
        {
            yield return null;
        }

        Debug.Log("Pressed " + c);
    }
}
