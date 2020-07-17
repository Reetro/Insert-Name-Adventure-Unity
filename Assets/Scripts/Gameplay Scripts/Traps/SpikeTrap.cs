using UnityEngine;

namespace LevelObjects.Trap
{
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField] private float damageToPlayer = 3f;
        [SerializeField] private float knockbackForceX = 1000f;
        [SerializeField] private float knockbackForceY = 400f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                GeneralFunctions.DamageTarget(collision.gameObject, damageToPlayer, true);

                GeneralFunctions.ApplyKnockback(collision.gameObject, collision.transform.up * knockbackForceY);

                var knockbackX = GeneralFunctions.GetDirectionVectroFrom2Vectors(collision.gameObject.transform.position, transform.position).x;

                GeneralFunctions.ApplyKnockback(collision.gameObject, new Vector2(knockbackX * knockbackForceX, 0));
            }
        }
    }
}