using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Progress))]

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    private Progress progress;

    private void Awake()
    {
        progress = GetComponent<Progress>();
    }

    private void OnEnable()
    {
        progress.OnProgressUpdate.AddListener(UpdateProgressImage);
    }

    private void OnDisable()
    {
        progress.OnProgressUpdate.RemoveListener(UpdateProgressImage);
    }

    private void UpdateProgressImage()
    {
        progressImage.fillAmount = progress.NormalizedValue;
    }
}
