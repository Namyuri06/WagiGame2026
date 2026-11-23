using System;
using UnityEngine;

/// <summary>
/// 인간 잡몹의 현재 AI 상태입니다.
/// </summary>
public enum HumanEnemyState
{
    Patrol,
    Chase,
    Windup,
    Recover,
    Disabled
}

/// <summary>
/// 일반 인간 잡몹의 이동 및 공격 AI입니다.
///
/// 공격 적중 시 AttackHit 이벤트를 발생시킵니다.
/// 실제 플레이어 피해 처리는 파트 3의 전투 시스템에서 담당합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public sealed class HumanEnemyAI : MonoBehaviour
{
    [Header("AI Profile")]
    [SerializeField] private HumanEnemyProfile profile;

    [Header("Target")]
    [SerializeField] private Transform playerTarget;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // 스프라이트 원본이 오른쪽을 바라보는지 설정합니다.
    [SerializeField] private bool spriteFacesRight = true;

    private Rigidbody2D body;

    private HumanEnemyState state = HumanEnemyState.Patrol;

    private float horizontalInput;
    private float patrolDirection = 1f;

    private float patrolTimer;
    private float actionTimer;
    private float attackCooldownTimer;

    /// <summary>
    /// 공격이 실제로 적중했을 때 발생합니다.
    ///
    /// 매개변수:
    /// attacker: 공격한 인간 잡몹
    /// target: 공격 대상
    /// damage: 프로필에 설정된 공격 피해량
    /// </summary>
    public event Action<HumanEnemyAI, Transform, float> AttackHit;

    public HumanEnemyState State => state;
    public Transform PlayerTarget => playerTarget;
    public HumanEnemyProfile Profile => profile;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }

        patrolTimer = profile != null
            ? profile.PatrolTurnInterval
            : 2f;
    }

    private void OnEnable()
    {
        state = HumanEnemyState.Patrol;
        horizontalInput = 0f;
        actionTimer = 0f;
    }

    private void Update()
    {
        if (profile == null || body == null)
        {
            horizontalInput = 0f;
            return;
        }

        attackCooldownTimer = Mathf.Max(
            0f,
            attackCooldownTimer - Time.deltaTime);

        horizontalInput = 0f;

        // 공격 준비 중에는 이동하지 않습니다.
        if (state == HumanEnemyState.Windup)
        {
            UpdateWindup();
            return;
        }

        // 공격 직후에는 회복 상태를 유지합니다.
        if (state == HumanEnemyState.Recover)
        {
            UpdateRecovery();
            return;
        }

        // 대상이 없거나 비활성화된 경우 순찰 상태로 돌아갑니다.
        if (!IsTargetValid())
        {
            playerTarget = null;
            UpdatePatrol();
            return;
        }

        Vector2 difference =
            playerTarget.position - transform.position;

        float horizontalDistance = Mathf.Abs(difference.x);
        float verticalDistance = Mathf.Abs(difference.y);

        // 다른 높이에 있는 플레이어는 탐지하지 않습니다.
        if (verticalDistance > profile.VerticalTolerance)
        {
            UpdatePatrol();
            return;
        }

        float effectiveDetectionRange =
            profile.DetectionRange * GetDetectionMultiplier();

        // 탐지 범위 밖에서는 순찰합니다.
        if (horizontalDistance > effectiveDetectionRange)
        {
            UpdatePatrol();
            return;
        }

        // 공격 범위 안에 들어오면 공격을 준비합니다.
        if (horizontalDistance <= profile.AttackRange)
        {
            if (attackCooldownTimer <= 0f)
            {
                BeginWindup();
            }
            else
            {
                state = HumanEnemyState.Chase;
                horizontalInput = 0f;
            }

            return;
        }

        // 탐지 범위 안이지만 공격 범위 밖이라면 추적합니다.
        UpdateChase(difference.x);
    }

    private void FixedUpdate()
    {
        if (body == null)
        {
            return;
        }

        // 수직 속도는 유지하고 수평 속도만 제어합니다.
        // Rigidbody2D의 중력 및 점프 동작을 방해하지 않습니다.
        Vector2 velocity = body.linearVelocity;

        velocity.x = profile != null
            ? horizontalInput
            : 0f;

        body.linearVelocity = velocity;
    }

    /// <summary>
    /// 외부 스포너 또는 게임 매니저가 플레이어를 지정합니다.
    /// null을 전달하면 현재 추적 대상을 해제합니다.
    /// </summary>
    public bool SetTarget(Transform target)
    {
        if (target == null)
        {
            playerTarget = null;
            state = HumanEnemyState.Patrol;
            horizontalInput = 0f;
            return true;
        }

        // 자기 자신이나 자신의 자식 오브젝트를 대상으로 삼지 않습니다.
        if (target == transform || target.IsChildOf(transform))
        {
            return false;
        }

        playerTarget = target;

        if (state == HumanEnemyState.Windup ||
            state == HumanEnemyState.Recover)
        {
            actionTimer = 0f;
        }

        state = HumanEnemyState.Chase;

        return true;
    }

    private bool IsTargetValid()
    {
        return playerTarget != null &&
               playerTarget.gameObject.activeInHierarchy;
    }

    private void UpdatePatrol()
    {
        state = HumanEnemyState.Patrol;

        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0f)
        {
            patrolDirection *= -1f;
            patrolTimer = profile.PatrolTurnInterval;
        }

        horizontalInput =
            patrolDirection * profile.PatrolSpeed;

        UpdateFacing(horizontalInput);
    }

    private void UpdateChase(float targetDifferenceX)
    {
        state = HumanEnemyState.Chase;

        if (Mathf.Abs(targetDifferenceX) < 0.01f)
        {
            horizontalInput = 0f;
            return;
        }

        float direction = Mathf.Sign(targetDifferenceX);

        float speedMultiplier = GetChaseSpeedMultiplier();

        horizontalInput =
            direction * profile.ChaseSpeed * speedMultiplier;

        UpdateFacing(horizontalInput);
    }

    private void BeginWindup()
    {
        state = HumanEnemyState.Windup;

        actionTimer = profile.WindupDuration;
        horizontalInput = 0f;
    }

    private void UpdateWindup()
    {
        horizontalInput = 0f;
        actionTimer -= Time.deltaTime;

        if (actionTimer <= 0f)
        {
            CompleteAttack();
        }
    }

    private void CompleteAttack()
    {
        // 공격 준비 도중 대상이 사라지거나 범위를 벗어났다면
        // 공격을 적중시키지 않습니다.
        if (!IsTargetWithinAttackRange())
        {
            state = IsTargetValid()
                ? HumanEnemyState.Chase
                : HumanEnemyState.Patrol;

            return;
        }

        AttackHit?.Invoke(
            this,
            playerTarget,
            profile.AttackDamage
        );

        attackCooldownTimer = profile.AttackCooldown;

        state = HumanEnemyState.Recover;
        actionTimer = profile.RecoveryDuration;
    }

    private bool IsTargetWithinAttackRange()
    {
        if (!IsTargetValid())
        {
            return false;
        }

        Vector2 difference =
            playerTarget.position - transform.position;

        return Mathf.Abs(difference.x) <= profile.AttackRange &&
               Mathf.Abs(difference.y) <= profile.VerticalTolerance;
    }

    private void UpdateRecovery()
    {
        horizontalInput = 0f;
        actionTimer -= Time.deltaTime;

        if (actionTimer <= 0f)
        {
            state = HumanEnemyState.Chase;
        }
    }

    private float GetDetectionMultiplier()
    {
        switch (profile.Pattern)
        {
            case HumanEnemyPattern.ChasingChild:
                return 1.4f;

            case HumanEnemyPattern.ThreateningPasserby:
                return 0.85f;

            case HumanEnemyPattern.BroomMerchant:
            default:
                return 1f;
        }
    }

    private float GetChaseSpeedMultiplier()
    {
        switch (profile.Pattern)
        {
            case HumanEnemyPattern.ChasingChild:
                return 1.25f;

            case HumanEnemyPattern.ThreateningPasserby:
                return 0.8f;

            case HumanEnemyPattern.BroomMerchant:
            default:
                return 0.65f;
        }
    }

    private void UpdateFacing(float direction)
    {
        if (spriteRenderer == null || Mathf.Abs(direction) < 0.01f)
        {
            return;
        }

        bool facingRight = direction > 0f;

        spriteRenderer.flipX = spriteFacesRight
            ? !facingRight
            : facingRight;
    }

    private void OnDisable()
    {
        horizontalInput = 0f;
        state = HumanEnemyState.Disabled;

        if (body != null)
        {
            Vector2 velocity = body.linearVelocity;
            velocity.x = 0f;
            body.linearVelocity = velocity;
        }
    }

    private void OnValidate()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }
    }
}
