using UnityEngine;

public class RazorBladeMovement : ProjectileMovement
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GeneralFunctions.DamageTarget(collision.gameObject, damage, true);

        OnImpact.Invoke();
    }
}
