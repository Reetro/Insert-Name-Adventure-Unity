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
        private float defaultDelay = 0;
        private float defaultPushUpTime = 0;
        private float spriteHeight = 0;
        private float pushUpTime = 0;
        private WormSegment wormSegmentToRotate = null;
        private bool pushingSegment = false;
        private bool segmentRotating = false;

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

            if (drawDebug)
            {
                print("Sprite Height: " + spriteHeight + " Push Up Time: " + pushUpTime);
            }

            wormSegmentToRotate = GetSegmentToRotate();

            segmentCount = childSegments.Count;
            defaultPushUpTime = pushUpTime;
        }
        /// <summary>
        /// Every X seconds push worm segment up and rotate worm downward to player
        /// </summary>
        private void Update()
        {
            PushSegmentsUp();

            PushToPlayer();
        }
        /// <summary>
        /// Will push the next grounded segment up into the world
        /// </summary>
        private void PushSegmentsUp()
        {
            if (!AreAllSegmentsUp() && !segmentRotating)
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

                        wormSegmentToRotate = GetSegmentToRotate();

                        pushingSegment = false;
                    }
                    else
                    {
                        pushingSegment = true;

                        MyMovementComp.MoveAIForward(Vector2.up, pushUpSpeed);
                    }
                }
            }
        }
        /// <summary>
        /// Push the current worm segment towards the player
        /// </summary>
        private void PushToPlayer()
        {
            if (!pushingSegment)
            {

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
        /// Find the bottom most segment
        /// </summary>
        private WormSegment GetSegmentToRotate()
        {
            var lastIndex = childSegments.Count - 1;
            WormSegment wormSegment = null;

            for (int index = 0; index < childSegments.Count; index++)
            {
                if (childSegments[index].AboveGround && index <= lastIndex)
                {
                    wormSegment = childSegments[index];
                    break;
                }
            }
            return wormSegment;
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
            return spriteHeight * 2 / pushUpSpeed * 0.3f;
        }
    }
}