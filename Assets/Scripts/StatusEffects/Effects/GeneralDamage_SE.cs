using GeneralScripts;

namespace StatusEffects.Effects
{
    public class GeneralDamageSe : StatusEffect
    {
        protected override void ApplyStatusEffect()
        {
            if (IsCurrentlyActive)
            {
                GeneralFunctions.ApplyDamageToTarget(Target, Value1 * StackCount, true, gameObject);
            }
        }
    }
}