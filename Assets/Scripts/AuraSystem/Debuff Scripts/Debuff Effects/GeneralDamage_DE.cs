namespace AuraSystem.Effects
{
    public class GeneralDamage_DE : DebuffEffect
    {
        public override void ApplyDebuffEffect()
        {
            if (IsCurrentlyActive)
            {
                GeneralFunctions.DamageTarget(Target, DebuffValue * StackCount, true, gameObject);
            }
        }
    }
}