using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct FloatRange
{
    public float Min, Max;
    public float RandomInRange => Random.Range(Min, Max);
}
