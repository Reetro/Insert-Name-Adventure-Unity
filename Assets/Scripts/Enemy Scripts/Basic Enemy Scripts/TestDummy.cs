using UnityEngine;

namespace EnemyCharacter
{
    public class TestDummy : MonoBehaviour
    {
        HealthComponent healthComp = null;
        private void Start()
        {
            healthComp = GetComponent<HealthComponent>();

            GeneralFunctions.ConstructHPComponent(gameObject);
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