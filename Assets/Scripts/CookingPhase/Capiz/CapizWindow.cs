using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapizWindow : MonoBehaviour
{
    [SerializeField] private Animator leftWindow;
    [SerializeField] private Animator rightWindow;
    [SerializeField] private Animator anahaw;

    public void Display(bool display = true)
    {
        leftWindow.SetTrigger(display ? "SlideIn" : "SlideOut");
        rightWindow.SetTrigger(display ? "SlideIn" : "SlideOut");
        anahaw.SetTrigger(display ? "Enter" : "Exit");
    }
}
