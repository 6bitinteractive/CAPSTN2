using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Procedure : ScriptableObject
{
    public abstract void Apply(Serving serving);
}
