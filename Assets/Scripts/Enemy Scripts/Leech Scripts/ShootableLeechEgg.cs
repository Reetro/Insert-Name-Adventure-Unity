﻿using UnityEngine;

namespace EnemyCharacter.SceneObject
{
    public class ShootableLeechEgg : MonoBehaviour
    {
        [SerializeField] private GameObject leechToSpawn = null;

        public void SpawnLeech()
        {
            Instantiate(leechToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}