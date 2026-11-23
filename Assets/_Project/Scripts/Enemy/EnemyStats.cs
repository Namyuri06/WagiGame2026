
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public enum EnemyType
    {
        Kid,
        BreadMan,
        Cleaner
    }

    [Header("Enemy Type")]
    [SerializeField] private EnemyType enemyType;

    public int AttackDamage { get; private set; }

    private void Awake()
    {
        switch (enemyType)
        {
            case EnemyType.Kid:
                AttackDamage = 5;
                break;

            case EnemyType.BreadMan:
                AttackDamage = 10;
                break;

            case EnemyType.Cleaner:
                AttackDamage = 15;
                break;
        }
    }
}
