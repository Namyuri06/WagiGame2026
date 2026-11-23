
using UnityEngine;

public class PoopBombHandler : MonoBehaviour
{
    [Header("Damage Calculator")]
    [SerializeField] private PoopBombDamageCalculator calculator;

    [Header("Recruitment Manager")]
    [SerializeField] private RecruitmentManager recruitmentManager;

    [Header("Target Camera")]
    [SerializeField] private Camera targetCamera;

    // 실제 포섭한 비둘기 수를 사용
    public void Attack()
    {
        if (recruitmentManager == null)
        {
            Debug.LogWarning("Poop Bomb: RecruitmentManager is missing!");
            return;
        }

        Attack(recruitmentManager.PigeonCount);
    }

    // 기존 테스트 및 다른 파트와의 호환성 유지
    public void Attack(int recruitedPigeonCount)
    {
        if (calculator == null)
        {
            Debug.LogWarning("Poop Bomb: Calculator is missing!");
            return;
        }

        Camera cam = targetCamera != null
            ? targetCamera
            : Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("Poop Bomb: Camera is missing!");
            return;
        }

        float damage =
            calculator.CalculateDamage(recruitedPigeonCount);

        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        int hitCount = 0;

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy == null || enemy.IsRecruited)
                continue;

            EnemyRecruitmentConnector connector =
                enemy.GetComponent<EnemyRecruitmentConnector>();

            // 인간만 공격하고 비둘기는 제외
            if (connector == null ||
                connector.Type != EnemyRecruitmentConnector.RecruitType.Human)
                continue;

            Vector3 screenPosition =
                cam.WorldToViewportPoint(enemy.transform.position);

            bool isVisible =
                screenPosition.z > 0f &&
                screenPosition.x >= 0f &&
                screenPosition.x <= 1f &&
                screenPosition.y >= 0f &&
                screenPosition.y <= 1f;

            if (isVisible)
            {
                enemy.TakeDamage(damage);
                hitCount++;
            }
        }

        Debug.Log(
            "Poop Bomb Damage: " + damage +
            " / Recruited Pigeons: " + recruitedPigeonCount +
            " / Hit Enemies: " + hitCount
        );
    }
}
