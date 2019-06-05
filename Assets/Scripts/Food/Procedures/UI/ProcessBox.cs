using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class ProcessBox : MonoBehaviour
{
    [Header("Setup")]
    public int ScoreAddition = 10;
    public int ScoreDeduction = -3;
    public bool Success { get; private set; }

    [SerializeField] private float timerDuration = 30f;

    [Header("UI")]
    [SerializeField] private float inset = 50f;
    [SerializeField] private Color success = new Color(0.2815607f, 1f, 0, 0.7490196f);
    [SerializeField] private Color fail = new Color(1f, 0, 0, 0.7490196f);
    [SerializeField] private Color active = new Color(1f, 0.7664064f, 0f, 0.7490196f);
    [SerializeField] private Color inactive = new Color(0.6509434f, 0.6509434f, 0.6509434f, 0.7490196f);

    [HideInInspector] public UnityEvent OnProcessDone = new UnityEvent();

    private Image image;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private ProcessBox.Timer timer;
    private UnityAction setSuccess;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        timer = new Timer(image, timerDuration);
        SetActive(false);
    }

    private void OnEnable()
    {
        setSuccess = new UnityAction(() => SetSuccess(false));
        timer.OnTimerEnd.AddListener(setSuccess);
    }

    private void OnDisable()
    {
        timer.OnTimerEnd.RemoveListener(setSuccess);
    }

    public void SetActive(bool active = true)
    {
        if (active)
        {
            //Debug.Log("Process Active: " + gameObject.name);

            image.color = this.active;
            canvasGroup.alpha = 1f;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + inset, rectTransform.anchoredPosition.y);

            // Start the timer
            StartCoroutine(timer.Start());
        }
        else
        {
            //Debug.Log("Process Inactive: " + gameObject.name);

            image.color = inactive;
            canvasGroup.alpha = 0.5f;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - inset, rectTransform.anchoredPosition.y);
        }

        // Fill the bar again
        image.fillAmount = 1f;
        // NOTE: Failed process boxes don't refill the bar. In any case, this is just a placeholder.
        // Might be a bug in Unity? The UI/Image doesn't update when fillAmount has been set to 0; amount is correct in console though.
        // Seems like an old bug that should've been fixed:
        // https://issuetracker.unity3d.com/issues/ui-image-fill-amount-breaks-when-value-is-set-to-0-through-script
    }

    public void SetSuccess(bool successful)
    {
        SetActive(false);

        if (successful)
        {
            Debug.Log("Process succeeded.");
            Debug.Log("Succeeded: " + gameObject.name);
            Success = true;
            timer.End = true;
            image.color = success;
        }
        else
        {
            Debug.Log("Process failed. Timer ran out.");
            Success = false;
            image.color = fail;
        }

        OnProcessDone.Invoke();
    }

    public class Timer
    {
        public Image Image;
        public float MaxDuration;
        public float CurrentTime;
        public bool Countdown;
        public bool End;
        public UnityEvent OnTimerEnd;

        public Timer(Image image, float maxDuration, float currentTime = 0f, bool countdown = true, bool end = false)
        {
            Image = image;
            MaxDuration = maxDuration;
            CurrentTime = currentTime;
            Countdown = countdown;
            End = end;
            OnTimerEnd = new UnityEvent();

            if (Countdown)
                CurrentTime = MaxDuration;
        }

        public IEnumerator Start()
        {
            Debug.Log("Process timer started.");

            while (!End)
            {
                if (Countdown)
                {
                    CurrentTime -= Time.deltaTime;

                    if (CurrentTime <= 0f)
                    {
                        End = true;
                        OnTimerEnd.Invoke();
                    }
                }
                else
                {
                    CurrentTime += Time.deltaTime;

                    if (CurrentTime >= MaxDuration)
                    {
                        End = true;
                        OnTimerEnd.Invoke();
                    }
                }

                // Image fill
                if (MaxDuration > 0)
                    Image.fillAmount = Mathf.Clamp01(CurrentTime / MaxDuration);

                //Debug.Log("Process current time: " + CurrentTime);
                yield return null;
            }
        }
    }
}
