
using System;
using UnityEngine;

public class RecruitmentManager : MonoBehaviour
{
    public int HumanCount { get; private set; }
    public int PigeonCount { get; private set; }

    public event Action<int> OnHumanCountChanged;
    public event Action<int> OnPigeonCountChanged;

    public void RecruitHuman()
    {
        HumanCount++;
        OnHumanCountChanged?.Invoke(HumanCount);

        Debug.Log("Recruited Humans: " + HumanCount);
    }

    public void RecruitPigeon()
    {
        PigeonCount++;
        OnPigeonCountChanged?.Invoke(PigeonCount);

        Debug.Log("Recruited Pigeons: " + PigeonCount);
    }
}
