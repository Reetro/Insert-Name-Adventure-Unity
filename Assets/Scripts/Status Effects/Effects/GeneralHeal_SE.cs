using UnityEngine;

namespace StatusEffects.Effects
{
    public class GeneralHeal_SE : StatusEffect
    {
        protected override void ApplyStatusEffect()
        {
            if (IsCurrentlyActive)
            {
                if (UseTicks)
                {
                    GeneralFunctions.HealTarget(Target, Value1);
                }
                else
                {
                    GeneralFunctions.HealTarget(Target, Value1 * Time.deltaTime);
                }
            }
        }
    }
}