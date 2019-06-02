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
    public GameObject BoilingPanel;
    public GameObject ChoppingPanel;

    public UnityEvent OnCookingPhaseEnd = new UnityEvent();
    public Recipe BaseRecipe { get; set; }

    private StageManager stageManager;
    private int currentProcedure;
    private int currentScore;

    private void OnEnable()
    {
        stageManager = SingletonManager.GetInstance<StageManager>();
        BaseRecipe = stageManager.CurrentStage.Recipe;
        Debug.Log("Current recipe: " + BaseRecipe.DisplayName);

        foreach (var procedure in BaseRecipe.Procedures)
            procedure.OnProcedureDone.AddListener(MoveToNextProcedure);
    }

    private void OnDisable()
    {
        foreach (var procedure in BaseRecipe.Procedures)
            procedure.OnProcedureDone.RemoveListener(MoveToNextProcedure);
    }

    private void Start()
    {
        ResetUI();

        // Test running a procedure
        if (BaseRecipe.Procedures.Length > 0)
        {
            BaseRecipe.Procedures[currentProcedure].Apply(this);
        }
    }

    private void Update()
    {
        // Test; force end procedure
        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(BaseRecipe.Procedures[currentProcedure].End());
    }

    public void MoveToNextProcedure()
    {
        currentProcedure++;

        if (currentProcedure >= BaseRecipe.Procedures.Length)
        {
            Debug.Log("Cooking Phase - End");

            // TODO: Calculate grade and save grade and score to StageManager
            // Test setting a result
            StageResult result = new StageResult(Grade.C, 50);
            stageManager.StageProgress[stageManager.CurrentStage] = result;

            OnCookingPhaseEnd.Invoke();
        }
        else
        {
            ResetUI();
            BaseRecipe.Procedures[currentProcedure].Apply(this);
        }
    }

    public void UpdateScoreUI(int additionalScore)
    {
        currentScore += additionalScore;
        ScoreText.text = string.Format("Score: {0}", currentScore.ToString());
    }

    public void ResetUI()
    {
        ProcessesPanel.gameObject.SetActive(false);
        BoilingPanel.gameObject.SetActive(false);
        ChoppingPanel.gameObject.SetActive(false);
    }
}
