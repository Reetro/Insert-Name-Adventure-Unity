using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private float segmentHealth = 6f;
        [SerializeField] private float MaxSpeedMagnitude = 30;

        private WormSegment[] childSegments;
        private int segmentCount = 0;
        private bool flipFlop = false;

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
                    wormSegment.GetComponent<Rigidbody2D>().AddForce(wormSegment.transform.position * 2, ForceMode2D.Impulse);

                    wormSegment.OnSegmentDeath.AddListener(OnSegmentDeath);
                }
            }

            segmentCount = childSegments.Length;
        }
        /// <summary>
        /// Add a constant force to each worm segment and clamp it's velocity
        /// </summary>
        private void Update()
        {
            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    flipFlop = !flipFlop;

                    var wormBody = wormSegment.GetComponent<Rigidbody2D>();

                    if (flipFlop)
                    {
                        wormSegment.GetComponent<Rigidbody2D>().AddForce(wormSegment.transform.position * 0.0001f, ForceMode2D.Impulse);
                    }
                    else
                    {
                        wormSegment.GetComponent<Rigidbody2D>().AddForce(wormSegment.transform.position * -0.0001f, ForceMode2D.Impulse);
                    }

                    if (wormBody.velocity.magnitude >= MaxSpeedMagnitude || wormBody.velocity.magnitude <= MaxSpeedMagnitude)
                    {
                        wormBody.velocity = wormBody.velocity.normalized * MaxSpeedMagnitude;
                    }
                }
            }
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