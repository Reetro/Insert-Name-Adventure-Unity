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
        [Tooltip("What layers should collision checks collide with")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();
        [Tooltip("The delay between each worm push up action")]
        [SerializeField] private float pushDelay = 1f;

        [Space]

        [Header("Debug Settings")]
        [Tooltip("Whether or not to print debug messages")]
        [SerializeField] private bool printDebug = false;

        private List<WormSegment> childSegments = new List<WormSegment>();
        private int segmentCount = 0;
        private bool flipFlop = false;
        private float pushAmount = 0f;

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
                    wormSegment.DrawDebug = printDebug;
                    wormSegment.WhatIsGround = whatIsGround;

                    wormSegment.SegmentDeath.AddListener(OnSegmentDeath);
                    wormSegment.CheckCollision();

                    pushAmount = GeneralFunctions.GetSpriteHeight(wormSegment.GetComponent<SpriteRenderer>()) * 2;
                }
            }

            segmentCount = childSegments.Count;

            StartCoroutine(PushWormSegmentUp());
        }
        /// <summary>
        /// Add a constant force to each worm segment every frame and clamp it's velocity
        /// </summary>
        private void FixedUpdate()
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
                yield return new WaitForSeconds(pushDelay);

                transform.Translate(0, pushAmount, 0);

                foreach (WormSegment wormSegment in childSegments)
                {
                    wormSegment.CheckCollision();
                }

                print("Test");

                if (AreAllSegmentsUp())
                {
                    yield break;
                }
            }
        }
        /// <summary>
        /// Checks to see if all worm segments are above ground
        /// </summary>
        private bool AreAllSegmentsUp()
        {
            foreach (WormSegment wormSegment in childSegments)
            {
                if (!wormSegment.AboveGround)
                {
                    return false;
                }
            }
            return true;
        }
    }
}