
using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private int scoreValue = 10;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public int ScoreValue => scoreValue;
    public bool IsRecruited { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnRecruited;

    // 기존 코드와의 호환성을 위해 유지
    public event Action<int> OnDied;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        CurrentHealth = maxHealth;
        IsRecruited = false;
    }

    public void TakeDamage(float damage)
    {
        if (IsRecruited || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            Recruit();
        }
    }

    private void Recruit()
    {
        if (IsRecruited)
            return;

        IsRecruited = true;

        OnRecruited?.Invoke(scoreValue);

        // 기존 EnemyScoreConnector와의 호환성
        OnDied?.Invoke(scoreValue);

        // 기존 테스트처럼 대상 제거
        // 실제 포섭 연출은 다른 파트에서 연결 예정
        Destroy(gameObject);
    }
}
