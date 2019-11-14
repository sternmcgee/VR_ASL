using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonController : MonoBehaviour
{
    LetterController letterDisplay;
    public char[] lessonPlan;
    private int lessonIndex;
    private char[] quizPlan;
    private int quizIndex;
    private IEnumerator hint;

    enum LessonState
    {
        Lesson,
        Practice,
        Quiz
    }
    LessonState state;


    private IEnumerator Hint(char letter)
    {
        yield return new WaitForSeconds(5);
        letterDisplay.Display(letter);
    }


    void Start()
    {
        letterDisplay = this.GetComponent<LetterController>();
        letterDisplay.Display(lessonPlan[0]);
        state = LessonState.Lesson;
        lessonIndex = 0;
        GenerateQuiz();
    }


    private void Update()
    {

        switch(state)
        {
            case LessonState.Lesson:
                if(Input.GetKeyDown(lessonPlan[lessonIndex].ToString().ToLower()))
                {
                    state = LessonState.Practice;
                    letterDisplay.TextOnly(lessonPlan[lessonIndex]);
                    hint = Hint(lessonPlan[lessonIndex]);
                    StartCoroutine(hint);
                }
                break;
            case LessonState.Practice:
                if(Input.GetKeyDown(lessonPlan[lessonIndex].ToString().ToLower()))
                {
                    StopCoroutine(hint);
                    lessonIndex++;
                    Debug.Log("Lesson index " + lessonIndex);
                    if(lessonIndex < lessonPlan.Length)
                    {
                        state = LessonState.Lesson;
                        letterDisplay.Display(lessonPlan[lessonIndex]);
                    } else
                    {
                        state = LessonState.Quiz;
                        letterDisplay.TextOnly(quizPlan[quizIndex]);
                    }
                }
                break;
            case LessonState.Quiz:
                if(Input.GetKeyDown(quizPlan[quizIndex].ToString().ToLower()))
                {
                    quizIndex++;
                    Debug.Log("Quiz index " + quizIndex);
                    if(quizIndex < quizPlan.Length)
                    {
                        letterDisplay.TextOnly(quizPlan[quizIndex]);
                    } else
                    {
                        letterDisplay.Blank();
                    }
                }
                break;
        }
    }


    private void GenerateQuiz()
    {
        quizIndex = 0;

        //create array with two copies of each letter
        quizPlan = new char[lessonPlan.Length * 2];
        for(int i = 0; i < lessonPlan.Length; i++)
        {
            quizPlan[2 * i] = lessonPlan[i];
            quizPlan[2 * i + 1] = lessonPlan[i];
        }

        //shuffle array
        for(int i = 0; i < quizPlan.Length; i++)
        {
            int r = Random.Range(0, quizPlan.Length);
            char temp = quizPlan[i];
            quizPlan[i] = quizPlan[r];
            quizPlan[r] = temp;
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
        state = LessonState.Lesson;
        GenerateQuiz();
    }
}
