using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterController : MonoBehaviour
{

    public GameObject FloatingLetter;
    public GameObject FloatingImage;
    private SimpleHelvetica LetterScript;
    private Renderer ImageRenderer;
    public Material[] Images;
    // Start is called before the first frame update
    void Start()
    {
        LetterScript = FloatingLetter.GetComponent<SimpleHelvetica>();
        ImageRenderer = FloatingImage.GetComponent<Renderer>();
    }

    
    public void Display(char c)
    {
        LetterScript.Text = c.ToString();
        LetterScript.GenerateText();

        ImageRenderer.enabled = true;
        ImageRenderer.material = Images[c - 'A'];
    }


    public void Blank()
    {
        LetterScript.Text = "";
        LetterScript.GenerateText();

        ImageRenderer.enabled = false;
    }


    public void TextOnly(char c)
    {
        LetterScript.Text = c.ToString();
        LetterScript.GenerateText();

        ImageRenderer.enabled = false;
    }
}
