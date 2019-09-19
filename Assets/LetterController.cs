using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterController : MonoBehaviour
{

    public GameObject FloatingLetter;
    public GameObject FloatingImage;
    public SimpleHelvetica LetterScript;
    public Renderer ImageRenderer;
    public Material[] Images;
    // Start is called before the first frame update
    void Start()
    {
        LetterScript = FloatingLetter.GetComponent<SimpleHelvetica>();
        ImageRenderer = FloatingImage.GetComponent<Renderer>();

        StartCoroutine(ChangeTextAFewTimes());
    }

    private IEnumerator ChangeTextAFewTimes()
    {
        yield return new WaitForSeconds(5);
        UpdateText('B');
        UpdateImage('B');
        yield return new WaitForSeconds(5);
        UpdateText('C');
        UpdateImage('C');
        yield return new WaitForSeconds(5);
        UpdateText('D');
        UpdateImage('D');
    }

   public void UpdateText(char c)
    {
        LetterScript.Text = c.ToString();
        LetterScript.GenerateText();
    }

    public void UpdateImage(char c)
    {
        ImageRenderer.material = Images[c - 'A'];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
