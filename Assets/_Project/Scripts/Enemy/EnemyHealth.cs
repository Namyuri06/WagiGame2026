
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

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnDied;

    private bool isDead = false;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        OnDied?.Invoke(scoreValue);
        Destroy(gameObject);
    }
}

