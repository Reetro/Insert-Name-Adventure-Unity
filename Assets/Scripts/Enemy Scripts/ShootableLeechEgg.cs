using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableLeechEgg : MonoBehaviour
{
    [SerializeField] private GameObject leechToSpawn = null;

    public void SpawnLeech()
    {
        Instantiate(leechToSpawn, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
