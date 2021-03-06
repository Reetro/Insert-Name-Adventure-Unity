﻿using GeneralScripts;
using GeneralScripts.GeneralComponents;
using UnityEngine;

namespace EnemyScripts.LeechScripts
{
    [RequireComponent(typeof(RigidbodyManager))]
    public class LeechEggCold : MonoBehaviour
    {
        [Tooltip("The actual leech prefab to spawn")]
        [SerializeField] private GameObject leechToSpawn = null;
        [Tooltip("Determines if the leech spawns when it hits the ground")]
        [SerializeField] private bool spawnOnGroundHit = false;

        private bool isOnPlatform = false;
        private bool hasPlatformEntered = false;
        private RigidbodyManager rigidbodyManager = null;

        private void Start()
        {
            rigidbodyManager = GetComponent<RigidbodyManager>();

            rigidbodyManager.onPlatformEnter.AddListener(OnPlatformEnter);
            rigidbodyManager.onPlatformExit.AddListener(OnPlatformExit);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!GeneralFunctions.IsObjectPlayer(collision.gameObject) && !GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                if (hasPlatformEntered)
                {
                    if (spawnOnGroundHit && !isOnPlatform)
                    {
                        SpawnLeech();
                    }
                }
            }
        }

        private void OnPlatformEnter()
        {
            hasPlatformEntered = true;
            isOnPlatform = true;
        }

        private void OnPlatformExit()
        {
            isOnPlatform = false;
        }

        public void SpawnLeech()
        {
            Instantiate(leechToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}