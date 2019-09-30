using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Gesture", order = 1)]
public class Gesture : ScriptableObject
{
    [Tooltip("The symbol that this gesture represents (e.g. 'A')")]
    public string symbol = null;

    public Transform[] bones = null;
    public float[] fingerCurls = new float[5];
}
