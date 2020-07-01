using UnityEngine;

public class Leech_DE : DebuffEffect
{
    public override void ApplyDebuffEffect()
    {
        var targetHealth = GetTargetHealthComponent();

        if (targetHealth)
        {
            targetHealth.ProccessDamage(GetDamage(), true);
        }
        else
        {
            Debug.LogError(gameObject.name + "debuff has no target health component");
        }
    }
}
