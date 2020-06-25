using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float damage = 1;
    private float despawnTime = 1;
    private bool hitPlayer = false;
    private LineRenderer lineRenderer = null;
    private bool touchedGround = false;
  
    private List<Collider2D> colliders = new List<Collider2D>();
    public List<Collider2D> GetColliders() { return colliders; }

    public virtual void ConstructBox(float damage, float despawnTime, bool hitPlayer, bool shouldDespawn)
    {
        this.damage = damage;
        this.despawnTime = despawnTime;
        this.hitPlayer = hitPlayer;

        lineRenderer = GetComponent<LineRenderer>();

        if (shouldDespawn)
        {
            StartCoroutine(DestroyBox());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(lineRenderer);

        if (collision.gameObject.CompareTag("Ground"))
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, transform.forward * 5);

            if (raycastHit2D)
            {
                touchedGround = true;

                Destroy(gameObject);
            }
        }

        if (!touchedGround)
        {
            if (!colliders.Contains(collision)) { colliders.Add(collision); }

            DamageAllObjects();
        }
    }

    private void DamageAllObjects()
    {
        var destroy = false;

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

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyBox()
    {
        yield return new WaitForSecondsRealtime(despawnTime);

        Destroy(gameObject);
    }
}
