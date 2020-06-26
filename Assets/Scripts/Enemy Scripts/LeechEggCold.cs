using UnityEngine;

public class LeechEggCold : MonoBehaviour
{
    [SerializeField] private GameObject leechToSpawn = null;

    public void SpawnLeech()
    {
        Instantiate(leechToSpawn, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
