using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PrepStation : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI ScoreText;
    public Processes ProcessesPanel;

    [Header("Boiling Panel")]
    public GameObject BoilingPanel;

    [Header("Chopping Panel")]
    public GameObject ChoppingPanel;
    public GameObject ChoppingKnife;
    public GameObject ChoppingIngredient;

    public UnityEvent OnCookingPhaseEnd = new UnityEvent();
    public Recipe BaseRecipe { get; set; }
    public InputHandler InputHandler { get; private set; }

    private StageManager stageManager;
    private int currentProcedure;
    private int currentScore;

    private void OnEnable()
    {
        InputHandler = SingletonManager.GetInstance<InputHandler>();
        stageManager = SingletonManager.GetInstance<StageManager>();
        BaseRecipe = stageManager.CurrentStage.Recipe;
        Debug.Log("Current recipe: " + BaseRecipe.DisplayName);

        foreach (var procedure in BaseRecipe.Procedures)
            procedure.OnProcedureDone.AddListener(() => procedure.Reset()); // Reset ScriptableObject; hack-ish

        // TODO: clean these up
        foreach (var process in ProcessesPanel.ProcessBoxes)
        {
            process.OnProcessDone.AddListener(() =>
            {
                UpdateScoreUI(process.Success ? process.ScoreAddition : process.ScoreDeduction);
                // TODO: Show results (success or fail) before switching
                MoveToNextProcedure();
            });
        }

        ProcessesPanel.OnAllProcessesDone.AddListener(() => Debug.Log("All Processes done.. End."));
    }

    private void OnDisable()
    {
        // TODO: Implement RemoveListeners
    }

    private void Start()
    {
        ResetUI();

        // Show relevant UI
        ProcessesPanel.gameObject.SetActive(true);

        // Test running a procedure
        if (BaseRecipe.Procedures.Length > 0)
        {
            RunProcedure(BaseRecipe.Procedures[currentProcedure]);
        }
    }

    public void MoveToNextProcedure()
    {
        // Broadcast that the procedure has been done
        BaseRecipe.Procedures[currentProcedure].OnProcedureDone.Invoke();

        // Move to the next
        currentProcedure++;

        // Check if we've reached the end of the procedure
        if (currentProcedure >= BaseRecipe.Procedures.Length)
        {
            Debug.Log("Cooking Phase - End");

            // TODO: Calculate grade and save grade and score to StageManager
            // Test setting a result
            StageResult result = new StageResult(Grade.C, currentScore);
            stageManager.StageProgress[stageManager.CurrentStage] = result;

            OnCookingPhaseEnd.Invoke();
        }
        else
        {
            Debug.Log("Moving on to next procedure...");
            ResetUI();
            RunProcedure(BaseRecipe.Procedures[currentProcedure]);
        }
    }

    public void UpdateScoreUI(int additionalScore)
    {
        currentScore += additionalScore;
        currentScore = Mathf.Clamp(currentScore, 0, int.MaxValue);
        ScoreText.text = string.Format("Score: {0}", currentScore.ToString());
    }

    public void ResetUI()
    {
        BoilingPanel.gameObject.SetActive(false);
        ChoppingPanel.gameObject.SetActive(false);
    }

    private void RunProcedure(Procedure procedure)
    {
        StopAllCoroutines();
        StartCoroutine(procedure.Apply(this));
    }
}
