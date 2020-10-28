namespace StatusEffects.Effects
{
    public class GeneralDamage_SE : StatusEffect
    {
        protected override void ApplyStatusEffect()
        {
            if (IsCurrentlyActive)
            {
                GeneralFunctions.DamageTarget(Target, EffectValue * StackCount, true, gameObject);
            }
        }
    }
}