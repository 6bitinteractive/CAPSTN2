using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doneness : MonoBehaviour
{
    public DonenessCondition Condition;

    public enum DonenessCondition
    {
        Uncooked,
        Undercooked,
        Cooked,
        Overcooked
    }
}
