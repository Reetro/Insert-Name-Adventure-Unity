namespace AuraSystem.Effects
{
    public class GeneralHeal_BE : BuffEffect
    {
        public override void ApplyBuffEffect(float buffAmount)
        {
            if (IsCurrentlyActive)
            {
                GeneralFunctions.HealTarget(Target, buffAmount);
            }
        }
    }
}