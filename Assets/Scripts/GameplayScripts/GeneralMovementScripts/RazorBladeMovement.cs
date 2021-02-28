using GeneralScripts;
using UnityEngine;

namespace GameplayScripts.GeneralMovementScripts
{
    public class RazorBladeMovement : ProjectileMovement
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GeneralFunctions.ApplyDamageToTarget(collision.gameObject, damage, true, gameObject);

            Destroy(gameObject);
        }
    }
}