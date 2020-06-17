using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!colliders.Contains(other)) { colliders.Add(other); }

        DamageAllActors();
    }

    private void DamageAllActors()
    {
        foreach (Collider2D collider2D in colliders)
        {
            if (hitPlayer)
            {
                var healthComp = collider2D.gameObject.GetComponent<HealthComponent>();

                if (healthComp)
                {
                    healthComp.ProccessDamage(damage);
                }
            }
            else
            {
                if (!collider2D.gameObject.CompareTag("Player"))
                {
                    var healthComp = collider2D.gameObject.GetComponent<HealthComponent>();

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
