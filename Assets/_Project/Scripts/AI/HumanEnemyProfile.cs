using UnityEngine;

/// <summary>
/// 일반 인간 잡몹의 행동 유형입니다.
/// </summary>
public enum HumanEnemyPattern
{
    BroomMerchant,
    ChasingChild,
    ThreateningPasserby
}

/// <summary>
/// 인간 잡몹의 공통 능력치와 AI 패턴을 관리합니다.
/// 실제 적 오브젝트마다 이 에셋을 참조합니다.
/// </summary>
[CreateAssetMenu(
    fileName = "HumanEnemyProfile",
    menuName = "Game/AI/Human Enemy Profile")]
public sealed class HumanEnemyProfile : ScriptableObject
{
    [Header("Enemy Pattern")]
    [SerializeField]
    private HumanEnemyPattern pattern =
        HumanEnemyPattern.BroomMerchant;

    [Header("Detection")]
    [SerializeField, Min(0.1f)] private float detectionRange = 5f;
    [SerializeField, Min(0.1f)] private float attackRange = 1.3f;
    [SerializeField, Min(0.1f)] private float verticalTolerance = 1.5f;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float patrolSpeed = 1f;
    [SerializeField, Min(0f)] private float chaseSpeed = 2f;
    [SerializeField, Min(0.1f)] private float patrolTurnInterval = 2f;

    [Header("Attack")]
    [SerializeField, Min(0f)] private float windupDuration = 0.4f;
    [SerializeField, Min(0f)] private float recoveryDuration = 0.7f;
    [SerializeField, Min(0f)] private float attackCooldown = 1.5f;
    [SerializeField, Min(0f)] private float attackDamage = 1f;

    public HumanEnemyPattern Pattern => pattern;

    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public float VerticalTolerance => verticalTolerance;

    public float PatrolSpeed => patrolSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float PatrolTurnInterval => patrolTurnInterval;

    public float WindupDuration => windupDuration;
    public float RecoveryDuration => recoveryDuration;
    public float AttackCooldown => attackCooldown;
    public float AttackDamage => attackDamage;

    private void OnValidate()
    {
        detectionRange = Mathf.Max(0.1f, detectionRange);
        attackRange = Mathf.Max(0.1f, attackRange);
        verticalTolerance = Mathf.Max(0.1f, verticalTolerance);

        patrolSpeed = Mathf.Max(0f, patrolSpeed);
        chaseSpeed = Mathf.Max(0f, chaseSpeed);
        patrolTurnInterval = Mathf.Max(0.1f, patrolTurnInterval);

        windupDuration = Mathf.Max(0f, windupDuration);
        recoveryDuration = Mathf.Max(0f, recoveryDuration);
        attackCooldown = Mathf.Max(0f, attackCooldown);
        attackDamage = Mathf.Max(0f, attackDamage);
    }
}
