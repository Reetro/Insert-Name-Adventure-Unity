using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float damage = 1;
    private float despawnTime = 1;
    private bool hitPlayer = false;
  
    private List<Collider2D> colliders = new List<Collider2D>();
    public List<Collider2D> GetColliders() { return colliders; }

    public virtual void ConstructBox(float damage, float despawnTime, bool hitPlayer, bool shouldDespawn)
    {
        this.damage = damage;
        this.despawnTime = despawnTime;
        this.hitPlayer = hitPlayer;

        if (shouldDespawn)
        {
            StartCoroutine(DestroyBox());
        }
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
            if (hitPlayer)
            {
                var healthComp = colliders[index].gameObject.GetComponent<HealthComponent>();

                if (healthComp)
                {
                    healthComp.ProccessDamage(damage);
                }
            }
            else
            {
                if (!colliders[index].gameObject.CompareTag("Player"))
                {
                    var healthComp = colliders[index].gameObject.GetComponent<HealthComponent>();

                    if (healthComp)
                    {
                        healthComp.ProccessDamage(damage);
                    }
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
