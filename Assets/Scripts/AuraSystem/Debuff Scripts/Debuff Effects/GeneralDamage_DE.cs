using AuraSystem.Effects;

public class GeneralDamage_DE : DebuffEffect
{
    public override void ApplyDebuffEffect()
    {
        GeneralFunctions.DamageTarget(Target, Damage * StackCount, true);
    }
}
