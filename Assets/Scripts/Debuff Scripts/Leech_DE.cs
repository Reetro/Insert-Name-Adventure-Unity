using UnityEngine;

public class Leech_DE : DebuffEffect
{
    public override void ApplyDebuffEffect()
    {
        Debug.Log(GetTicks());
    }
}
