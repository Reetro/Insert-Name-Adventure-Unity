namespace StatusEffects.Effects
{
    public class GeneralDamage_SE : StatusEffect
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