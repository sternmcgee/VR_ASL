using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterController : MonoBehaviour
{

    public GameObject GameObjec;
    public SimpleHelvetica script;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObjec.GetComponent<SimpleHelvetica>();

        StartCoroutine(ChangeTextAFewTimes());
    }

    private IEnumerator ChangeTextAFewTimes()
    {
        yield return new WaitForSeconds(5);
        UpdateText('B');
        yield return new WaitForSeconds(5);
        UpdateText('C');
        yield return new WaitForSeconds(5);
        UpdateText('D');
    }

   public void UpdateText(char c)
    {
        script.Text = c.ToString();
        script.GenerateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
