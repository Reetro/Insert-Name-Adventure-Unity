using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float damage = 1;
    private float despawnTime = 1;
  
    private List<Collider2D> colliders = new List<Collider2D>();
    public List<Collider2D> GetColliders() { return colliders; }

    public virtual void ConstructBox(float damage, float despawnTime)
    {
        this.damage = damage;
        this.despawnTime = despawnTime;

        StartCoroutine(DestroyBox());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!colliders.Contains(collision)) { colliders.Add(collision); }

        DamageAllObjects();
    }

    private void DamageAllObjects()
    {
        for (int index = 0; index < colliders.Count; index++)
        {
            if (!colliders[index].gameObject.CompareTag("Player"))
            {
                var healthComp = colliders[index].gameObject.GetComponent<HealthComponent>();

                if (healthComp)
                {
                    healthComp.ProccessDamage(damage, true);
                }

                var leechEggRipe = colliders[index].gameObject.GetComponent<LeechEggRipe>();

                if (leechEggRipe)
                {
                    leechEggRipe.SpawnLeech();
                }

                var leechEggCold = colliders[index].gameObject.GetComponent<LeechEggCold>();

                if (leechEggCold)
                {
                    leechEggCold.SpawnLeech();
                }
            }
        }

        colliders.Clear();
    }

    private IEnumerator DestroyBox()
    {
        yield return new WaitForSecondsRealtime(despawnTime);

        Destroy(gameObject);
    }
}
