public class Breather_BE : BuffEffect
{
    public override void ApplyBuffEffect(float buffAmount)
    {
        GeneralFunctions.HealTarget(GetTarget(), buffAmount);
    }
}
