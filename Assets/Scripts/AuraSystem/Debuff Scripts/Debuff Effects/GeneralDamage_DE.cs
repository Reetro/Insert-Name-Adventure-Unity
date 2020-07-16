using AuraSystem;

public class GeneralDamage_DE : DebuffEffect
{
    public override void ApplyDebuffEffect()
    {
        GeneralFunctions.DamageTarget(GetTarget(), GetDamage(), true);
    }
}
