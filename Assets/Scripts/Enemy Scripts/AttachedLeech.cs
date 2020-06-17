using UnityEngine;

public class AttachedLeech : MonoBehaviour
{
    [SerializeField] ScriptableDebuff leechingDebuff = null;
    [SerializeField] HealthComponent leechHealthComp = null;

    public void OnLeechSpawn(float health, AuraManager auraManager, GameObject player)
    {
        leechHealthComp.SetHealth(health);

        auraManager.ApplyDebuff(player, leechingDebuff, true);
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
