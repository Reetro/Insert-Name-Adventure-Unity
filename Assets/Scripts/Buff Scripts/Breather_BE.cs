using UnityEngine;
public class Breather_BE : BuffEffect
{
    public override void ApplyBuffEffect(float buffAmount)
    {
        //Debug.Log(buffAmount);
    }

    public override void OnBuffEnd()
    {
        Debug.Log("Debuff done");

        GetAuraManager().RemoveBuff(this);
    }
}
