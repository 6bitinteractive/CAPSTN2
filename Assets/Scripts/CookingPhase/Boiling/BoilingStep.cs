using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoilingStep : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float pressureEventDelay = 5f; // Would random range be better?
    [SerializeField] private int maxPrompts;
    [SerializeField] private GameObject spawnZonesContainer;

    [Header("1 - Adding ingredients")]
    public UnityEvent OnWaterBoiling = new UnityEvent();
    public UnityEvent OnWaterNotBoiling = new UnityEvent();

    [Header("2 - Main loop")]
    public UnityEvent OnPressureStart = new UnityEvent();
    public UnityEvent OnPressureHasMounted = new UnityEvent();
    public UnityEvent OnPressureReleased = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();

    private List<SpawnZone> spawnZones = new List<SpawnZone>();
    private bool boilingWater;
    private bool startPressureAccumulation;
    private float pressureTimer;
    private int promptCount;

    private void Awake()
    {
        spawnZones.AddRange(spawnZonesContainer.GetComponentsInChildren<SpawnZone>());
    }

    private void Update()
    {
        if (MaxPromptReached())
        {
            startPressureAccumulation = false;
            return;
        }

        if (startPressureAccumulation)
        {
            pressureTimer += Time.deltaTime;

            if (PressureHasMounted(pressureTimer, pressureEventDelay))
            {
                OnPressureHasMounted.Invoke();
                SpawnPrompt();
            }
        }
    }

    // First part of the game's session: adding ingredients
    public void BoilWater(HeatSetting heatSetting)
    {
        if (boilingWater)
            return;

        if (heatSetting == HeatSetting.High)
        {
            boilingWater = true;
            OnWaterBoiling.Invoke();
        }
        else
        {
            OnWaterNotBoiling.Invoke();
        }
    }

    public void StartPressure()
    {
        if (MaxPromptReached())
            return;

        startPressureAccumulation = true;
        pressureTimer = 0f;
        OnPressureStart.Invoke();
    }

    public void ReleasePressure()
    {
        if (MaxPromptReached())
            return;

        OnPressureReleased.Invoke();
    }

    public void SpawnPrompt()
    {
        if (MaxPromptReached())
            return;

        spawnZones[Random.Range(0, spawnZones.Count)].Spawn();
        promptCount++;
        startPressureAccumulation = false;
    }

    private bool PressureHasMounted(float timer, float delay)
    {
        return timer >= delay;
    }

    private bool MaxPromptReached()
    {
        return promptCount >= maxPrompts;
    }

    public void EndCheck(bool successful)
    {
        if (MaxPromptReached())
        {
            OnEnd.Invoke();
        }
        else if (!successful)
        {
            SpawnPrompt();
        }
    }
}
