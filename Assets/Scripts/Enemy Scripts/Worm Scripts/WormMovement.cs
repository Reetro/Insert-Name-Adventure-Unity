using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : MonoBehaviour
    {
        [Header("Worm Settings")]
        [Tooltip("How much damage each segments deals")]
        [SerializeField] private float damage = 1f;
        [Tooltip("How much health each segment has")]
        [SerializeField] private float segmentHealth = 6f;
        [Tooltip("The maximum speed the segments are allowed to go")]
        [SerializeField] private float MaxSpeed = 5;

        [Space]

        [Header("Debug Settings")]
        [Tooltip("Whether or not to print debug messages")]
        [SerializeField] private bool printDebug = false;

        private List<WormSegment> childSegments = new List<WormSegment>();
        private int segmentCount = 0;
        private int segementsAboveGroundCount = 0;
        private bool flipFlop = false;

        /// <summary>
        /// Get a random rotation to move towards then setup segment damage
        /// </summary>
        private void Start()
        {
            childSegments = GetComponentsInChildren<WormSegment>().ToList();

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.DamageToApply = damage;
                    wormSegment.MyHealthComponent.SetHealth(segmentHealth);
                    wormSegment.GetComponent<Rigidbody2D>().AddForce(wormSegment.transform.position * 2, ForceMode2D.Impulse);

                    wormSegment.SegmentDeath.AddListener(OnSegmentDeath);
                }
            }

            segmentCount = childSegments.Count;

            //StartCoroutine(PushWormSegmentUp());
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

                    if (wormBody.velocity.magnitude >= MaxSpeed || wormBody.velocity.magnitude <= MaxSpeed)
                    {
                        wormBody.velocity = wormBody.velocity.normalized * MaxSpeed;
                    }
                }
            }
        }
        /// <summary>
        /// Called whenever a segment is killed if all segments are destroyed then this object is destroyed
        /// </summary>
        private void OnSegmentDeath(WormSegment wormSegment)
        {
            var segementsToKill = GetSegmentsAboveKilledSegment(wormSegment);

            foreach (WormSegment wormSeg in segementsToKill)
            {
                if (wormSeg)
                {
                    var health = wormSeg.GetComponent<HealthComponent>();

                    if (health)
                    {
                        segmentCount = Mathf.Clamp(segmentCount - 1, 0, childSegments.Count - 1);

                        if (!health.IsCurrentlyDead)
                        {
                            GeneralFunctions.KillTarget(wormSeg.gameObject);

                            if (printDebug)
                            {
                                print("Hit: " + wormSegment.name + " Killed: " + wormSeg.name);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning(wormSeg.name + " does not have a health component");
                    }
                }
            }

            if (printDebug)
            {
                print("Current Segment Count: " + segmentCount.ToString());
            }

            if (segmentCount <= 0)
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// Gets all segments above the given segment
        /// </summary>
        /// <param name="wormSegment"></param>
        private List<WormSegment> GetSegmentsAboveKilledSegment(WormSegment wormSegment)
        {
            List<WormSegment> localSegments = new List<WormSegment>();
            int index = childSegments.IndexOf(wormSegment);
            int totalCount = childSegments.Count - 1;
            bool isEqual = index == totalCount;

            while (!isEqual)
            {
                isEqual = index == totalCount;

                localSegments.Add(childSegments[index]);

                index = Mathf.Clamp(index + 1, 0, totalCount);
            }

            return localSegments;
        }
        /// <summary>
        /// Every X seconds push another worm segment up
        /// </summary>
        private IEnumerator PushWormSegmentUp()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                transform.position = transform.position = new Vector3(0, transform.position.y + 1, 0);

                segementsAboveGroundCount++;

                if (AreAllSegmentsUp())
                {
                    yield break;
                }
            }
        }
        /// <summary>
        /// Checks to see if all worm segments are above ground
        /// </summary>
        /// <returns></returns>
        private bool AreAllSegmentsUp()
        {
            return segementsAboveGroundCount >= segmentCount ? true : false;
        }
    }
}