using EnemyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : EnemyBase
    {
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float damage = 1f;

        private Quaternion targetRotation;
        private WormSegment[] childSegments;


        private void Start()
        {
            targetRotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));

            childSegments = GetComponentsInChildren<WormSegment>();

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.DamageToApply = damage;
                    print(wormSegment.DamageToApply);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GeneralFunctions.DamageTarget(collision.gameObject, damage, true, gameObject);
            }
        }
    }
}
