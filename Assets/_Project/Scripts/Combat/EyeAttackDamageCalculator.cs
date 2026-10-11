
using UnityEngine;

public class EyeAttackDamageCalculator : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damagePerHuman = 2f;
    [SerializeField] private float maxDamage = 50f;

    public float CalculateDamage(int bribedHumanCount)
    {
        int count = Mathf.Max(0, bribedHumanCount);

        float eyeAttackDamage = baseDamage + count * damagePerHuman;

        return Mathf.Clamp(eyeAttackDamage, 0f, maxDamage);
    }

}
