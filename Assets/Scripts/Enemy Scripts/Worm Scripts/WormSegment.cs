using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        public float DamageToApply { get; set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GeneralFunctions.DamageTarget(collision.gameObject, DamageToApply, true, gameObject);
            }
        }
    }
}