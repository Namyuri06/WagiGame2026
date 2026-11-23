
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyScoreConnector : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private StageManager stageManager;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        stageManager = FindFirstObjectByType<StageManager>();

        if (stageManager == null)
        {
            Debug.LogWarning("StageManager not found!");
            return;
        }

        enemyHealth.OnDied += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(int score)
    {
        stageManager.AddScore(score);
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDied -= HandleEnemyDeath;
        }
    }
}
