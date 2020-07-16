using AuraSystem.Effects;

public class GeneralHeal_BE : BuffEffect
{
    public override void ApplyBuffEffect(float buffAmount)
    {
        GeneralFunctions.HealTarget(Target, buffAmount);
    }
}
