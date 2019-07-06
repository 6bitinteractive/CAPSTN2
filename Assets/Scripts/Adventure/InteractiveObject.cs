using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] UnityEvent OnInteract = new UnityEvent();
    [SerializeField] UnityEvent OnNPCCollision = new UnityEvent();
    [SerializeField] UnityEvent OnNPCExit = new UnityEvent();
    [SerializeField] Image InteractSymbol;
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        canvas.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // If player
        if (collision.gameObject.layer == 8)
        {
            canvas.enabled = true;
            OnNPCCollision.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // If player
        if (collision.gameObject.layer == 8)
        {
            canvas.enabled = false;
        }
    }

    public void Talk()
    {
        OnInteract.Invoke();
    }
}
