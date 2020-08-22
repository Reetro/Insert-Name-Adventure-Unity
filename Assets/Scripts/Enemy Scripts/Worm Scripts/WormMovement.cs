using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private float segmentHealth = 6f;

        private WormSegment[] childSegments;
        private int segmentCount = 0;

        /// <summary>
        /// Get a random rotation to move towards then setup segment damage
        /// </summary>
        private void Start()
        {
            childSegments = GetComponentsInChildren<WormSegment>();

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.DamageToApply = damage;
                    wormSegment.MyHealthComponent.SetHealth(segmentHealth);

                    wormSegment.OnSegmentDeath.AddListener(OnSegmentDeath);
                }
            }

            segmentCount = childSegments.Length;
        }

        private void Update()
        {
            
        }
        /// <summary>
        /// Called whenever a segment is killed if all segments are destroyed then this object is destroyed
        /// </summary>
        private void OnSegmentDeath()
        {
            segmentCount--;

            if (segmentCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}