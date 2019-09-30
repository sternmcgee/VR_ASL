using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CaptureGesture : MonoBehaviour
{
    public float timeDelay = 3f;

    private SteamVR_Behaviour_Skeleton skeleton = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (skeleton == null)
            skeleton = GetComponent<SteamVR_Behaviour_Skeleton>();
        if (skeleton == null)
            Debug.LogError("No behaviour skeleton attached to this gameobject.");

        //capture gesture after a time delay
        Invoke("Capture", timeDelay);
    }

    private void Capture()
    {
        //get current skeleton data
        Transform[] bones = skeleton.bones;
        float[] fingerCurls = skeleton.fingerCurls;

        //use ScriptableObject.CreateInstance to store data as a new gesture
        Gesture newGesture = ScriptableObject.CreateInstance<Gesture>();
        newGesture.name = "Put name here";
        newGesture.symbol = "Put symbol here";
        newGesture.bones = bones;
        newGesture.fingerCurls = fingerCurls;

        //disable the gameobject
        this.enabled = false;
    }
}
