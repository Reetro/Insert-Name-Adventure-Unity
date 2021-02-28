using GeneralScripts;
using UnityEngine;

namespace StatusEffects.Effects
{
    public class GeneralHealSe : StatusEffect
    {
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void ApplyStatusEffect()
        {
            if (!IsCurrentlyActive) return;
            
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