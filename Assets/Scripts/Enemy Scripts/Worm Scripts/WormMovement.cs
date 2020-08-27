using System.Collections;
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

        #region Local Varaibles
        private List<WormSegment> childSegments = new List<WormSegment>();
        private int segmentCount = 0;
        private float spriteHeight = 0;
        private WormSegment wormSegmentToRotate = null;
        private bool pushingSegment = false;
        private bool segmentRotating = false;

        /// <summary>
        /// Get this objects enemy movement component
        /// </summary>
        public EnemyMovement MyMovementComp { get; private set; }
        #endregion

        #region Setup Functions
        /// <summary>
        /// Get a random rotation to move towards then setup segment damage
        /// </summary>
        private void Start()
        {
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

                    spriteHeight = wormSegment.MyBoxCollider2D.bounds.size.y;

                    wormSegment.SegmentDeath.AddListener(OnSegmentDeath);
                    wormSegment.CheckCollision();

                    if (drawDebug)
                    {
                        if (wormSegment.AboveGround)
                        {
                            print("Worm Segment: " + wormSegment.name + " is above ground");
                        }
                        else
                        {
                            print("Worm Segment: " + wormSegment.name + " is underground");
                        }
                    }
                }
            }

            segmentCount = childSegments.Count;

            wormSegmentToRotate = GetSegmentToRotate();

            if (drawDebug)
            {
                if (wormSegmentToRotate)
                {
                    print("Rotating segment: " + wormSegmentToRotate.name);
                }
            }

            StartCoroutine(PushSegmentsUp());
        }
        #endregion

        #region Movement Functions
        /// <summary>
        /// Every X seconds push worm segment up and rotate worm downward to player
        /// </summary>
        private void Update()
        {
            PushToPlayer();
        }
        /// <summary>
        /// Will push the next grounded segment up into the world
        /// </summary>
        private IEnumerator PushSegmentsUp()
        {
            while (true)
            {
                yield return new WaitForSeconds(pushDelay);

                if (AreAllSegmentsUp())
                {
                    yield break;
                }

                if (!segmentRotating)
                {
                    var direction = GetNextSegmentLocation();

                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = (Vector2)transform.position + direction;
                    float elapsedTime = 0;

                    while (elapsedTime < pushUpSpeed)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / pushUpSpeed));
                        elapsedTime += Time.deltaTime;
                        pushingSegment = true;
                        yield return null;
                    }

                    pushingSegment = false;

                    foreach (WormSegment wormSegment in childSegments)
                    {
                        wormSegment.CheckCollision();
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
        /// Find the bottom most segment
        /// </summary>
        private WormSegment GetSegmentToRotate()
        {
            WormSegment localWormSegment = null;

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment.AboveGround)
                {
                    localWormSegment = wormSegment;
                    break;
                }
            }

            return localWormSegment;
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
        /// Calculates the amount of distance the worm needs to move
        /// </summary>
        private Vector2 GetNextSegmentLocation()
        {
            var topSegment = GetTopMostSegment();

            var traceEnd = GeneralFunctions.GetFaceingDirectionY(topSegment.gameObject) * spriteHeight;

            if (drawDebug)
            {
                Debug.DrawRay(topSegment.transform.position, traceEnd * spriteHeight, Color.red);

                Debug.Log("Translate amount: " + traceEnd);
            }

            return traceEnd;
        }
        /// <summary>
        /// Gets the top most worm segment
        /// </summary>
        private WormSegment GetTopMostSegment()
        {
            return childSegments[childSegments.Count - 1];
        }
        #endregion

        #region Health Functions
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
        #endregion
    }
}