using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingsDataResetter : MonoBehaviour
{
    public void ResetGameProgress()
    {
        SingletonManager.GetInstance<RatingsManager>().ResetProgress();
    }
}
