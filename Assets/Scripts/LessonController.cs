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

    enum LessonState
    {
        Lesson,
        Practice,
        Quiz
    }
    LessonState state;

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
        if(Input.GetKeyDown(lessonPlan[lessonIndex].ToString().ToLower()))
        {

            switch(state)
            {
                case LessonState.Lesson:
                    state = LessonState.Practice;
                    letterDisplay.TextOnly(lessonPlan[lessonIndex]);
                    break;
                case LessonState.Practice:
                    lessonIndex++;
                    if(lessonIndex < lessonPlan.Length)
                    {
                        state = LessonState.Lesson;
                        letterDisplay.Display(lessonPlan[lessonIndex]);
                    } else
                    {
                        state = LessonState.Quiz;
                        letterDisplay.TextOnly(quizPlan[quizIndex]);
                    }
                    break;
                case LessonState.Quiz:
                    quizIndex++;
                    if(quizIndex < quizPlan.Length)
                    {
                        letterDisplay.TextOnly(quizPlan[quizIndex]);
                    } else
                    {
                        letterDisplay.Blank();
                    }
                    break;
            }
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
