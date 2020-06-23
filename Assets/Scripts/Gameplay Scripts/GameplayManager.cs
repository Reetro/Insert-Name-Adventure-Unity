using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public Collider2D[] colliders;
    public float radius = 100f;
    public LayerMask mask;

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
