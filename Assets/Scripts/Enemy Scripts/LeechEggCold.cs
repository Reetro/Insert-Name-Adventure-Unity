using UnityEngine;

public class LeechEggCold : MonoBehaviour
{
    [Tooltip("The actual leech prefab to spawn")]
    [SerializeField] private GameObject leechToSpawn = null;
    [Tooltip("Determines if the leech spawns when it hits the ground")]
    [SerializeField] private bool spawnOnGroundHit = false;

    private bool isOnPlatform = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (spawnOnGroundHit && !isOnPlatform)
        {
            SpawnLeech();
        }
    }

    public void OnPlatformEnter()
    {
        isOnPlatform = true;
    }

    public void OnPlatformExit()
    {
        isOnPlatform = false;
    }

    public void SpawnLeech()
    {
        Instantiate(leechToSpawn, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
