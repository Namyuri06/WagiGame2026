
using UnityEngine;

public class EyeAttackHandler : MonoBehaviour
{
    [Header("Damage Calculator")]
    [SerializeField] private EyeAttackDamageCalculator calculator;

    [Header("Recruitment Manager")]
    [SerializeField] private RecruitmentManager recruitmentManager;

    public void Attack(EnemyHealth target)
    {
        if (target == null)
        {
            Debug.LogWarning("Eye Attack: Target is missing!");
            return;
        }

        if (calculator == null || recruitmentManager == null)
        {
            Debug.LogWarning("Eye Attack: Missing reference!");
            return;
        }

        int humanCount = recruitmentManager.HumanCount;

        float damage = calculator.CalculateDamage(humanCount);

        target.TakeDamage(damage);

        Debug.Log(
            "Eye Attack Damage: " + damage +
            " / Recruited Humans: " + humanCount
        );
    }

    // 기존 테스트 및 다른 파트와의 호환성 유지
    public void Attack(EnemyHealth target, int recruitedHumanCount)
    {
        if (target == null || calculator == null)
            return;

        float damage = calculator.CalculateDamage(recruitedHumanCount);
        target.TakeDamage(damage);
    }
}
