using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LessonController : MonoBehaviour
{
    public GestureRecognizer recognizer;
    public Hand rHand = null;
    public Hi5_Interaction_Core.Hi5_Hand_Visible_Hand visibleHand = null;
    LetterController letterDisplay;
    //time in ms between gesture read calls
    public float timeInterval = 5f;
    public char[] lessonPlan;
    private int lessonIndex;
    private char[] quizPlan;
    private int quizIndex;
    private IEnumerator hint;
    private IEnumerator gestureCouroutine;
    private bool correctGesture = false;

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

    private IEnumerator WaitThenRun()
    {
        yield return  new WaitForSeconds(1);

        StartCoroutine(ReadGesture());
    }

    private IEnumerator ChangeHandColor()
    {
        visibleHand.ChangeColor(Color.green);
        yield return new WaitForSeconds(0.5f);
        visibleHand.ChangeColor(visibleHand.orgColor);
    }

    private IEnumerator ReadGesture()
    {
        float waitTime = timeInterval / 1000f;
        int counter = 0;
        int gestureCount = 0;

        while (true)
        {
            char letter = (state == LessonState.Quiz) ? quizPlan[quizIndex] : lessonPlan[lessonIndex];
            if(recognizer.svmIsGesture(HI5.Hand.RIGHT, letter) || recognizer.svmIsGesture(HI5.Hand.LEFT, letter))
            {
                gestureCount++;
            }
            counter++;

            if (counter == 10 )
            {
                if (gestureCount > 7)
                {
                    correctGesture = true;
                    rHand.TriggerHapticPulse(5000);
                    StartCoroutine(ChangeHandColor());
                    Debug.Log("Correct Gesture");
                }
                else
                    correctGesture = false;

                gestureCount = 0;
                counter = 0;
                //Debug.Log("Gesture read loop ended!");
            }                
            
            yield return new WaitForSeconds(waitTime);
        }
       
    }


    void Start()
    {
        if (rHand == null)
            Debug.LogError("Hand component not assigned to this script!");

        letterDisplay = this.GetComponent<LetterController>();
        letterDisplay.Display(lessonPlan[0]);
        state = LessonState.Lesson;
        lessonIndex = 0;
        GenerateQuiz();
        StartCoroutine(WaitThenRun());
    }


    private void Update()
    {

        switch(state)
        {
            case LessonState.Lesson:
                //if(Input.GetKeyDown(lessonPlan[lessonIndex].ToString().ToLower()))
                if (correctGesture)
                {
                    state = LessonState.Practice;
                    letterDisplay.TextOnly(lessonPlan[lessonIndex]);
                    hint = Hint(lessonPlan[lessonIndex]);
                    StartCoroutine(hint);
                    correctGesture = false;
                }
                break;
            case LessonState.Practice:
                if (correctGesture)
                //if(Input.GetKeyDown(lessonPlan[lessonIndex].ToString().ToLower()))
                {
                    correctGesture = false;
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
                //if (Input.GetKeyDown(quizPlan[quizIndex].ToString().ToLower()))
                    if (correctGesture)
                    {
                        correctGesture = false;
                        quizIndex++;
                        Debug.Log("Quiz index " + quizIndex);
                        if (quizIndex < quizPlan.Length)
                        {
                            letterDisplay.TextOnly(quizPlan[quizIndex]);
                        }
                        else
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
