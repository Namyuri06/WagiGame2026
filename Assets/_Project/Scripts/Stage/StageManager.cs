

using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum EndingType
    {
        Bad,
        Normal,
        Good
    }

    [Header("Stage Settings")]
    [SerializeField] private float timeLimit = 180f;

    [Header("Ending Score")]
    [SerializeField] private int normalScore = 50;
    [SerializeField] private int goodScore = 100;

    public float RemainingTime { get; private set; }
    public int CurrentScore { get; private set; }
    public bool IsStageOver { get; private set; }

    public event Action<float> OnTimeChanged;
    public event Action<int> OnScoreChanged;
    public event Action<EndingType> OnStageEnded;

    private void Start()
    {
        RemainingTime = Mathf.Max(0f, timeLimit);
        CurrentScore = 0;
        IsStageOver = false;

        OnTimeChanged?.Invoke(RemainingTime);
        OnScoreChanged?.Invoke(CurrentScore);

        if (RemainingTime <= 0f)
            EndStage();
    }

    private void Update()
    {
        if (IsStageOver)
            return;

        RemainingTime = Mathf.Max(
            0f,
            RemainingTime - Time.deltaTime
        );

        OnTimeChanged?.Invoke(RemainingTime);

        if (RemainingTime <= 0f)
            EndStage();
    }

    public void AddScore(int points)
    {
        if (IsStageOver || points <= 0)
            return;

        CurrentScore += points;
        OnScoreChanged?.Invoke(CurrentScore);

        Debug.Log("Current Score: " + CurrentScore);
    }

    public void PlayerDied()
    {
        if (IsStageOver)
            return;

        FinishStage(EndingType.Bad);
    }

    private void EndStage()
    {
        EndingType ending;

        if (CurrentScore >= goodScore)
            ending = EndingType.Good;
        else if (CurrentScore >= normalScore)
            ending = EndingType.Normal;
        else
            ending = EndingType.Bad;

        FinishStage(ending);
    }

    private void FinishStage(EndingType ending)
    {
        if (IsStageOver)
            return;

        IsStageOver = true;

        Debug.Log("Stage Ended: " + ending);
        OnStageEnded?.Invoke(ending);
    }
}
