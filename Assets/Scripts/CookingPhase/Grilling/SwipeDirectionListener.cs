using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kind of a hack for PanAnimationController; same as InputListener script =_=;
public class SwipeDirectionListener : MonoBehaviour
{
    public SwipeEvent OnCorrectSwipe = new SwipeEvent();

    private void Awake()
    {
        SingletonManager.Register<SwipeDirectionListener>(this);
    }

    private void OnDisable()
    {
        SingletonManager.UnRegister<SwipeDirectionListener>();
    }

    public void InvokeOnCorrectSwipe(SwipeData swipeData)
    {
        OnCorrectSwipe.Invoke(swipeData);
    }
}
