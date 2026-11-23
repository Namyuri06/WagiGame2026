
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyRecruitmentConnector : MonoBehaviour
{
    public enum RecruitType
    {
        Human,
        Pigeon
    }

    [Header("Recruitment Type")]
    [SerializeField] private RecruitType recruitType = RecruitType.Human;
    public RecruitType Type => recruitType;

    private EnemyHealth enemyHealth;
    private RecruitmentManager recruitmentManager;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        recruitmentManager =
            FindFirstObjectByType<RecruitmentManager>();

        if (recruitmentManager == null)
        {
            Debug.LogWarning("RecruitmentManager not found!");
            return;
        }

        enemyHealth.OnRecruited += HandleRecruitment;
    }

    private void HandleRecruitment(int score)
    {
        if (recruitType == RecruitType.Human)
        {
            recruitmentManager.RecruitHuman();
        }
        else
        {
            recruitmentManager.RecruitPigeon();
        }
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnRecruited -= HandleRecruitment;
        }
    }
}
