﻿using GeneralScripts;
using UnityEngine;
using GeneralScripts.GeneralComponents;

namespace EnemyScripts.BasicEnemyScripts
{
    public class TestDummy : MonoBehaviour
    {
        HealthComponent healthComp = null;
        private void Start()
        {
            healthComp = GetComponent<HealthComponent>();

            GeneralFunctions.ConstructHpComponent(gameObject);
        }

        public void OnTakeDamage(float damage)
        {
            Debug.Log("Current HP: " + healthComp.CurrentHealth.ToString());
            Debug.Log("Damage Taken: " + damage);
        }

        public void OnDeath()
        {
            Debug.Log("Dead");
        }
    }
}