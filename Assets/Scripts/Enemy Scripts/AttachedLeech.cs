using UnityEngine;

public class AttachedLeech : MonoBehaviour
{
    [SerializeField] ScriptableDebuff leechingDebuff = null;
    [SerializeField] HealthComponent leechHealthComp = null;

    private AuraManager auraManager = null;
    
    public void OnLeechSpawn(float health, AuraManager auraManager, GameObject player)
    {
        leechHealthComp.SetHealth(health);

        auraManager.ApplyDebuff(player, leechingDebuff, true);

        this.auraManager = auraManager;
    }

    public void OnDeath()
    {
        var debuff = auraManager.FindDebuffOtype(leechingDebuff);

        debuff.RemoveFromStack(true, auraManager, leechingDebuff);

        Destroy(gameObject);
    }
}
