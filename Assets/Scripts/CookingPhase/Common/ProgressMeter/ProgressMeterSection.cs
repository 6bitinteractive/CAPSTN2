using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class ProgressMeterSection : MonoBehaviour
{
    public ProgressMeterState state;
    public Image meter;
}
