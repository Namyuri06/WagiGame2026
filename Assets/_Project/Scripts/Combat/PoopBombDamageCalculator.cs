
using UnityEngine;

public class PoopBombDamageCalculator : MonoBehaviour
{
    [SerializeField] private float baseDamage = 15f;
    [SerializeField] private float damagePerMinion = 3f;
    [SerializeField] private float maxDamage = 75f;

    public float CalculateDamage(int minionCount)
    {
        int count = Mathf.Max(0, minionCount);

        float poopBombDamage = baseDamage + count * damagePerMinion;

        return Mathf.Clamp(poopBombDamage, 0f, maxDamage);
    }

}
