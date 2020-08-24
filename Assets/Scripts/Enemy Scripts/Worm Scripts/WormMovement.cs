using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnemyCharacter.AI
{
    [RequireComponent(typeof(EnemyMovement))]
    public class WormMovement : MonoBehaviour
    {
        [Header("Worm Settings")]
        [Tooltip("How much damage each segments deals")]
        [SerializeField] private float damage = 1f;
        [Tooltip("How much health each segment has")]
        [SerializeField] private float segmentHealth = 6f;
        [Tooltip("The maximum speed the segments are allowed to go")]
        [SerializeField] private float MaxSpeed = 5;
        [Tooltip("The delay between each worm push up action")]
        [SerializeField] private float pushDelay = 1f;
        [Tooltip("How fast Worm Segments are pushed up to the surface")]
        [SerializeField] private float pushUpSpeed = 0.1f;
        [Tooltip("What layers should collision checks collide with")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        [Space]

        [Header("Debug Settings")]
        [Tooltip("Whether or not to print debug messages")]
        [SerializeField] private bool drawDebug = false;

        private List<WormSegment> childSegments = new List<WormSegment>();
        private int segmentCount = 0;
        private bool flipFlop = false;
        private float defaultDelay = 0;
        private float defaultPushUpTime = 0;
        private float spriteHeight = 0;
        private float pushUpTime = 0;

        /// <summary>
        /// Get this objects enemy movement component
        /// </summary>
        public EnemyMovement MyMovementComp { get; private set; }

        /// <summary>
        /// Get a random rotation to move towards then setup segment damage
        /// </summary>
        private void Start()
        {
            defaultDelay = pushDelay;

            MyMovementComp = GetComponent<EnemyMovement>();

            childSegments = GetComponentsInChildren<WormSegment>().ToList();

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.DamageToApply = damage;
                    wormSegment.MyHealthComponent.SetHealth(segmentHealth);
                    wormSegment.DrawDebug = drawDebug;
                    wormSegment.WhatIsGround = whatIsGround;

                    spriteHeight = GeneralFunctions.GetSpriteHeight(wormSegment.GetComponent<SpriteRenderer>());

                    wormSegment.SegmentDeath.AddListener(OnSegmentDeath);
                    wormSegment.CheckCollision();
                }
            }

            pushUpTime = CaluclatePushTime();
     
            segmentCount = childSegments.Count;
            defaultPushUpTime = pushUpTime;
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
        /// Every X seconds push worm segment up
        /// </summary>
        private void Update()
        {
            if (!AreAllSegmentsUp())
            {
                pushDelay -= Time.deltaTime;

                if (pushDelay <= 0)
                {
                    pushUpTime -= Time.deltaTime;

                    if (pushUpTime <= 0)
                    {
                        foreach (WormSegment wormSegment in childSegments)
                        {
                            wormSegment.CheckCollision();
                        }

                        pushUpTime = defaultPushUpTime;
                        pushDelay = defaultDelay;
                    }
                    else
                    {
                        MyMovementComp.MoveAIForward(Vector2.up, pushUpSpeed);
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

                            if (drawDebug)
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

            if (drawDebug)
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
        /// <summary>
        /// Calculate the amount of time needed to push a worm segment up
        /// </summary>
        private float CaluclatePushTime()
        {
            return spriteHeight * 2 / pushUpSpeed;
        }
    }
}