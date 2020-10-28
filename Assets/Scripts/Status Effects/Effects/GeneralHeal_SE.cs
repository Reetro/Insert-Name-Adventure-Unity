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
                    GeneralFunctions.HealTarget(Target, EffectValue);
                }
                else
                {
                    GeneralFunctions.HealTarget(Target, EffectValue * Time.deltaTime);
                }
            }
        }
    }
}