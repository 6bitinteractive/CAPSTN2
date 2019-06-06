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
    public ResultsPanel ResultsPanel;

    [Header("Boiling Panel")]
    public GameObject BoilingPanel;
    public StoveController StoveController;

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

        // Set the recipe
        BaseRecipe = stageManager.CurrentStage.Recipe;
        Debug.Log("Current recipe: " + BaseRecipe.DisplayName);

        // TODO: clean these up
        foreach (var procedure in BaseRecipe.Procedures)
        {
            procedure.OnProcedureSuccess.AddListener(() =>
            {
                // When player finishes a procedure, set process box as successful
                ProcessesPanel.ProcessBoxes[currentProcedure].SetSuccess(true);

                // Show the results panel
                ResultsPanel.ShowPanel(ProcessesPanel.ProcessBoxes[currentProcedure].Success);
            });
        }

        foreach (var process in ProcessesPanel.ProcessBoxes)
        {
            process.OnProcessDone.AddListener(() =>
            {
                // Update the score
                UpdateScoreUI(process.Success ? process.ScoreAddition : process.ScoreDeduction);

                // Show the results panel
                ResultsPanel.ShowPanel(ProcessesPanel.ProcessBoxes[currentProcedure].Success);
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
        // Reset currentProcedure's ScriptableObject before moving to the next
        BaseRecipe.Procedures[currentProcedure].Reset();

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
            stageManager.MoveToNextStage();

            OnCookingPhaseEnd.Invoke();
        }
        else
        {
            Debug.Log("Moving on to next procedure...");
            ResetUI();
            ProcessesPanel.MoveToNextProcess();
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
        ResultsPanel.gameObject.SetActive(false);
    }

    private void RunProcedure(Procedure procedure)
    {
        StopAllCoroutines();
        StartCoroutine(procedure.Apply(this));
    }
}
