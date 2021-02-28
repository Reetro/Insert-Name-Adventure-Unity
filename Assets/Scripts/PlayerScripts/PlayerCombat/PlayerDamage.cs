using System.Collections.Generic;
using EnemyScripts.LeechScripts;
using GeneralScripts;
using UnityEngine;

namespace PlayerScripts.PlayerCombat
{
    public sealed class PlayerDamage : MonoBehaviour
    {
        private float damage = 1;
        private bool appliedDamage = false;

        private readonly List<Collider2D> colliders = new List<Collider2D>();
        public List<Collider2D> GetColliders() { return colliders; }

        public void ConstructBox(float newDamage, float despawnTime)
        {
            GameObject o;
            (o = gameObject).SetActive(true);

            damage = newDamage;

            appliedDamage = false;

            Destroy(o, despawnTime);
        }

        public void ConstructBox(float newDamage)
        {
            gameObject.SetActive(true);

            this.damage = newDamage;

            appliedDamage = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!colliders.Contains(collision)) { colliders.Add(collision); }

            if (!appliedDamage)
            {
                DamageAllObjects();
            }
        }

        private void DamageAllObjects()
        {
            appliedDamage = true;

            foreach (var t in colliders)
            {
                if (t == null) continue;
                if (t.gameObject.CompareTag("Player")) continue;
                var leechEggRipe = t.gameObject.GetComponent<LeechEggRipe>();
                var leechEggCold = t.gameObject.GetComponent<LeechEggCold>();

                if (leechEggRipe)
                {
                    leechEggRipe.SpawnLeech();
                }
                else if (leechEggCold)
                {
                    leechEggCold.SpawnLeech();
                }
                else if (!leechEggRipe && !leechEggCold)
                {
                    GeneralFunctions.ApplyDamageToTarget(t.gameObject, damage, true, gameObject);
                }
            }

            colliders.Clear();
        }
    }
}