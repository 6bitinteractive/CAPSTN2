using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;

public class QuickTimeEventAccelerometer : Prompt
{
    [SerializeField] private float timerDuration = 3f;

    private Image timerImage;
    private float currentTime;
    private Vector3 InitialTilt;
    private Vector3 Tilt;
    private bool isFlippingUpwards;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip[] clips;

    protected override void Awake()
    {
        base.Awake();
        currentTime = timerDuration;
        timerImage = GetComponent<Image>();     
    }

    protected void OnEnable()
    {
        ResetTimer();
        SetInitialYTilt();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            PlaySFX(1);
            Debug.Log("Failed");
            OnFailedInput.Invoke();
            Hide();
            return;
        }

        CheckFlip();

        timerImage.fillAmount = Mathf.Clamp01(currentTime / timerDuration);
    }

    private void SuccessfulInput()
    {
        PlaySFX(0);
        Debug.Log("Success");
        OnSuccessfulInput.Invoke();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        currentTime = timerDuration;
        timerImage.fillAmount = 1f;
    }

    public void SetInitialYTilt()
    {
        InitialTilt = Input.acceleration;
        InitialTilt.y -= 0.2f;
        InitialTilt.y = Mathf.Clamp(InitialTilt.y, -1.0f, 0);

        //Checks if its greater than the maximum threshold
        if (InitialTilt.y == -1.0f)
        {
            isFlippingUpwards = false;
            InitialTilt.y += 0.4f;
        }

        else
        {
            isFlippingUpwards = true;
        }
    }

    public void CheckFlip()
    {
        Tilt = Input.acceleration;
        Debug.Log("Initial Tilt: " + InitialTilt);
        Debug.Log("Current Tilt: " + Tilt);

        // Flip upwards
        if (isFlippingUpwards && Tilt.y <= InitialTilt.y)
        {
            Debug.Log("Up");
            SuccessfulInput();
        }

        //Flipdownwards NOTE* This is used to counteract a bug in which if the player's phone's initial Tilt.y is at -1.0f it will automatically pass a SuccessfulInput
        else if(!isFlippingUpwards && Tilt.y >= InitialTilt.y)
        {
            Debug.Log("Down");
            SuccessfulInput();
        }
    }

    public void PlaySFX(int index)
    {
        sfx.clip = clips[index];
        sfx.Play();
    }
}
