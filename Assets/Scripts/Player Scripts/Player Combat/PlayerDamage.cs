using System.Collections.Generic;
using UnityEngine;
using EnemyCharacter.SceneObject;

public class PlayerDamage : MonoBehaviour
{
    private float damage = 1;
    private bool appliedDamage = false;

    private List<Collider2D> colliders = new List<Collider2D>();
    public List<Collider2D> GetColliders() { return colliders; }

    public virtual void ConstructBox(float damage, float despawnTime)
    {
        gameObject.SetActive(true);

        this.damage = damage;

        appliedDamage = false;

        Destroy(gameObject, despawnTime);
    }

    public virtual void ConstructBox(float damage)
    {
        gameObject.SetActive(true);

        this.damage = damage;

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

        for (int index = 0; index < colliders.Count; index++)
        {
            if (!colliders[index].gameObject.CompareTag("Player"))
            {
                var leechEggRipe = colliders[index].gameObject.GetComponent<LeechEggRipe>();
                var leechEggCold = colliders[index].gameObject.GetComponent<LeechEggCold>();

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
                    GeneralFunctions.DamageTarget(colliders[index].gameObject, damage, true, gameObject);
                }
            }
        }

        colliders.Clear();
    }
}