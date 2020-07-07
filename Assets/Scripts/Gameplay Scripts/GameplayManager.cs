﻿using UnityEngine;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{
    [Header("Collider settings")]
    public Collider2D[] colliders;
    public float radius = 100f;
    public LayerMask mask;

    [Space]

    [Header("Combat Text Settings")]
    [Range(0.01f, 1f)] public float combatTextSpeed = 0.01f;
    public float combatTextUpTime = 0.5f;
    public float combatRandomVectorMinX = -0.5f;
    public float combatRandomVectorMaxX = 1f;
    public float combatRandomVectorMinY = -0.5f;
    public float combatRandomVectorMaxY = 1f;

    [Header("Damage Settings")]
    public LayerMask whatCanBeDamaged;

    List<int> gameplayObjects = new List<int>();

    public int GenID()
    {
        var newID = Random.Range(1, 1000000);

        for (int index = 0; index < gameplayObjects.Count; index++)
        {
            if (gameplayObjects.Contains(newID))
            {
                newID = Random.Range(1, 1000000);
                break;
            }
        }

        gameplayObjects.Add(newID);
        return newID;
    }

    public List<int> GetAllIDs()
    {
        return gameplayObjects;
    }

    public bool PreventSpawnOverlap(Vector3 spawnPosition)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, radius, mask);

        for (int index = 0; index < colliders.Length; index++)
        {
            Vector3 centerPoint = colliders[index].bounds.center;
            float width = colliders[index].bounds.extents.x;
            float height = colliders[index].bounds.extents.y;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.y - height;
            float upperExtent = centerPoint.y + height;

            if (spawnPosition.x >= leftExtent && spawnPosition.x <= rightExtent)
            {
                if (spawnPosition.y >= lowerExtent && spawnPosition.y <= upperExtent)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
