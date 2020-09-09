using UnityEngine;

namespace AuraSystem.Effects
{
    public class GeneralHeal_BE : BuffEffect
    {
        public override void ApplyBuffEffect()
        {
            if (IsCurrentlyActive)
            {
                if (UseTicks)
                {
                    GeneralFunctions.HealTarget(Target, BuffValue);
                }
                else
                {
                    GeneralFunctions.HealTarget(Target, BuffValue * Time.deltaTime);
                }
            }
        }
    }
}