using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AuraSystem;
using AuraSystem.Effects;

namespace EnemyCharacter.AI
{
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
        [Tooltip("The damage cooldown applied to each segment when segment damages the player")]
        [SerializeField] private float damageCooldown = 1f;
        [Tooltip("What layers should collision test with")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        [Space]

        [Header("Rotation Settings")]
        [Tooltip("How fast the worm can rotate a segment")]
        [SerializeField] private float rotationSpeed = 1f;
        [Tooltip("The worm ground offset")]
        [SerializeField] private float rotationOffset = 0.4f;
        [Tooltip("The amount time the worm segment waits before returning back to it's starting rotation")]
        [SerializeField] private float returnHomeDelay = 4f;
        [Tooltip("The amount time the worm segment waits before rotating to the player")]
        [SerializeField] private float rotationDelay = 2f;
        [Tooltip("The target angle the worm will rotate to when player is on the right of the worm")]
        [SerializeField] private float targetRightAngle = -90f;
        [Tooltip("The target angle the worm will rotate to when player is on the left of the worm")]
        [SerializeField] private float targetLeftAngle = 90f;
        [Tooltip("The opacity worm sprites will have when collision is disabled")]
        [Range(0, 1)]
        [SerializeField] private float spriteOpacity = 0.5f;

        [Header("Squish Settings")]
        [Tooltip("The debuff to apply to the player")]
        [SerializeField] private ScriptableDebuff debuffToApply = null;
        [Tooltip("The new scale to apply to the player")]
        [SerializeField] private Vector3 SquishScale = new Vector3(1f, 0.5f, 0);

        [Space]

        [Header("Debug Settings")]
        [Tooltip("Whether or not to print debug messages")]
        [SerializeField] private bool drawDebug = false;

        #region Local Varaibles
        private List<WormSegment> childSegments = new List<WormSegment>();
        private float spriteHeight = 0;
        private WormSegment wormSegmentToRotate = null;
        private bool pushingSegment = false;
        private bool segmentRotating = false;
        private Quaternion targetRotation;
        private bool hookedToGround = false;
        private Quaternion homeRotation;
        private Vector3 homeLocation;
        private float defaultHomeDelay = 0f;
        private float defaultRotationDelay = 0f;
        private bool returningHome = false;
        private bool unHookedFromGround = false;
        private GameObject playerObject = null;
        private float currentAngle = 0f;
        private bool appliedDebuff = false;
        #endregion

        #region Setup Functions
        /// <summary>
        /// Get a random rotation to move towards then setup segment damage
        /// </summary>
        private void Start()
        {
            childSegments = GetComponentsInChildren<WormSegment>().ToList();

            playerObject = GeneralFunctions.GetPlayerGameObject();

            foreach (WormSegment wormSegment in childSegments)
            {
                SetupWormSegment(wormSegment);
            }

            wormSegmentToRotate = GetSegmentToRotate();

            if (drawDebug)
            {
                if (wormSegmentToRotate)
                {
                    print("Rotating segment: " + wormSegmentToRotate.name);
                }
            }

            currentAngle = GeneralFunctions.GetObjectEulerAngle(gameObject);

            StartCoroutine(PushSegmentsUp());

            SetupRotation();
        }
        /// <summary>
        /// Setup all default worm segments
        /// </summary>
        /// <param name="wormSegment"></param>
        private void SetupWormSegment(WormSegment wormSegment)
        {
            if (wormSegment)
            {
                wormSegment.DamageToApply = damage;
                wormSegment.MyHealthComponent.SetHealth(segmentHealth);
                wormSegment.DrawDebug = drawDebug;
                wormSegment.WhatIsGround = whatIsGround;
                wormSegment.IsRotating = segmentRotating;
                wormSegment.SquishScale = SquishScale;
                wormSegment.SquishTime = debuffToApply.GetTotalTime();
                wormSegment.CanDamage = true;
                wormSegment.IDObject.ConstructID();
                wormSegment.PlayerObject = playerObject;

                spriteHeight = wormSegment.MyBoxCollider2D.bounds.size.y;
                wormSegment.SpriteWidth = wormSegment.MyBoxCollider2D.bounds.size.x;

                wormSegment.SegmentDeath.AddListener(OnSegmentDeath);
                wormSegment.OnSquishedPlayer.AddListener(OnSquishedPlayer);
                wormSegment.DamagedPlayer.AddListener(OnDamagedPlayer);
                wormSegment.OnUnSquishedPlayer.AddListener(OnUnSquishedPlayer);
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
            else
            {
                Debug.LogError("Failed to setup worm segments");
            }
        }
        /// <summary>
        /// Determines the 1st rotation direction and sets all default rotation values
        /// </summary>
        private void SetupRotation()
        {
            defaultHomeDelay = returnHomeDelay;

            defaultRotationDelay = rotationDelay;

            playerObject = GeneralFunctions.GetPlayerGameObject();

            if (wormSegmentToRotate)
            {
                GetNextRotation();
            }
        }
        /// <summary>
        /// Determine the next rotation the current segment will travel in
        /// </summary>
        private void GetNextRotation()
        {
            homeRotation = wormSegmentToRotate.transform.rotation;

            homeLocation = wormSegmentToRotate.transform.position;

            if (currentAngle >= 180)
            {
                if (GeneralFunctions.IsObjectLeftOrRight(transform, playerObject.transform))
                {
                    targetRotation = Quaternion.AngleAxis(targetRightAngle, wormSegmentToRotate.transform.forward) * wormSegmentToRotate.transform.rotation;
                }
                else
                {
                    targetRotation = Quaternion.AngleAxis(targetLeftAngle, wormSegmentToRotate.transform.forward) * wormSegmentToRotate.transform.rotation;
                }
            }
            else
            {
                if (GeneralFunctions.IsObjectLeftOrRight(transform, playerObject.transform))
                {
                    targetRotation = Quaternion.AngleAxis(targetLeftAngle, wormSegmentToRotate.transform.forward) * wormSegmentToRotate.transform.rotation;

                    foreach (WormSegment wormSegment in childSegments)
                    {
                        if (wormSegment)
                        {
                            wormSegment.IsPlayerLeft = true;
                        }
                    }
                }
                else
                {
                    targetRotation = Quaternion.AngleAxis(targetRightAngle, wormSegmentToRotate.transform.forward) * wormSegmentToRotate.transform.rotation;

                    foreach (WormSegment wormSegment in childSegments)
                    {
                        if (wormSegment)
                        {
                            wormSegment.IsPlayerLeft = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Movement Functions
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

                    foreach (WormSegment wormSegment in childSegments)
                    {
                        wormSegment.CheckCollision();
                    }

                    wormSegmentToRotate = GetSegmentToRotate();

                    if (drawDebug)
                    {
                        print(wormSegmentToRotate);
                    }

                    GetNextRotation();

                    pushingSegment = false;
                }
            }
        }
        /// <summary>
        /// Rotate the current worm segment towards the player
        /// </summary>
        private void Update()
        {
            if (wormSegmentToRotate)
            {
                if (!pushingSegment)
                {
                    HookToGround();

                    if (!returningHome)
                    {
                        wormSegmentToRotate.transform.rotation = Quaternion.Slerp(wormSegmentToRotate.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                        segmentRotating = true;

                        foreach (WormSegment wormSegment in childSegments)
                        {
                            wormSegment.IsRotating = segmentRotating;
                        }

                        if (wormSegmentToRotate.transform.rotation == targetRotation)
                        {
                            returningHome = true;
                        }
                    }
                    else
                    {
                        returnHomeDelay -= Time.deltaTime;

                        if (returnHomeDelay <= 0)
                        {
                            UnHookFromGround();

                            wormSegmentToRotate.transform.rotation = Quaternion.Slerp(wormSegmentToRotate.transform.rotation, homeRotation, rotationSpeed * Time.deltaTime);

                            if (wormSegmentToRotate.transform.rotation == homeRotation)
                            {
                                segmentRotating = false;

                                foreach (WormSegment wormSegment in childSegments)
                                {
                                    wormSegment.IsRotating = segmentRotating;
                                }

                                ReturnHome();

                                foreach (WormSegment wormSegment in childSegments)
                                {
                                    if (!wormSegment.MyHealthComponent.IsCurrentlyDead)
                                    {
                                        wormSegment.EnableCollision();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Restarts rotation timer 
        /// </summary>
        private void ReturnHome()
        {
            rotationDelay -= Time.deltaTime;

            if (rotationDelay <= 0)
            {
                GetNextRotation();

                returnHomeDelay = defaultHomeDelay;
                rotationDelay = defaultRotationDelay;

                hookedToGround = false;
                unHookedFromGround = false;
                returningHome = false;
            }
        }
        /// <summary>
        /// Push the worm segment on to the ground
        /// </summary>
        private void HookToGround()
        {
            if (!hookedToGround)
            {
                if (currentAngle >= 180)
                {
                    wormSegmentToRotate.transform.position = new Vector3(wormSegmentToRotate.transform.position.x, wormSegmentToRotate.transform.position.y + rotationOffset);

                    hookedToGround = true;
                }
                else
                {
                    wormSegmentToRotate.transform.position = new Vector3(wormSegmentToRotate.transform.position.x, wormSegmentToRotate.transform.position.y - rotationOffset);

                    hookedToGround = true;
                }
            }
        }
        /// <summary>
        /// Called when the player Get Squished by a segment
        /// </summary>
        private void OnSquishedPlayer()
        {
            if (!appliedDebuff)
            {
                GeneralFunctions.ApplyDebuffToTarget(playerObject, debuffToApply, true);

                appliedDebuff = true;
            }

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.HasPlayerBeenSquished = true;

                    wormSegment.SetOpacity(spriteOpacity);
                }
            }
        }
        /// <summary>
        /// Called when the player is Unsquished
        /// </summary>
        private void OnUnSquishedPlayer()
        {
            appliedDebuff = false;
        }
        /// <summary>
        /// Reset worm segment position back to starting position
        /// </summary>
        private void UnHookFromGround()
        {
            if (!unHookedFromGround)
            {
                wormSegmentToRotate.transform.position = homeLocation;

                unHookedFromGround = true;
            }

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.HasPlayerBeenSquished = false;
                }
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

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    if (wormSegment.Equals(topSegment))
                    {
                        wormSegment.IsTopSegment = true;
                    }
                    else
                    {
                        wormSegment.IsTopSegment = false;
                    }
                }
            }

            var traceEnd = GeneralFunctions.GetFaceingDirectionY(topSegment.gameObject) * spriteHeight;

            if (drawDebug)
            {
                Debug.DrawRay(topSegment.transform.position, traceEnd * spriteHeight, Color.red);

                Debug.Log("Translate amount: " + traceEnd);
            }

            return traceEnd;
        }
        /// <summary>
        /// Called when a worm segments damages the player will place all segments on cooldown
        /// </summary>
        private void OnDamagedPlayer()
        {
            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.CanDamage = false;
                }
            }

            StartCoroutine(DamageCooldown());
        }
        /// <summary>
        /// The actual cooldown timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageCooldown()
        {
            yield return new WaitForSeconds(damageCooldown);

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    wormSegment.CanDamage = true;
                }
            }
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

            var segemntsDeathState = GetAllSegmentsDeathState();

            if (GeneralFunctions.IsBoolArrayTrue(segemntsDeathState.ToArray()))
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// Gets all segments health component is dead Variables
        /// </summary>
        /// <returns>A bool array</returns>
        public List<bool> GetAllSegmentsDeathState()
        {
            List<bool> isDeadList = new List<bool>();

            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    var health = GeneralFunctions.GetGameObjectHealthComponent(wormSegment.gameObject);

                    if (health)
                    {
                        isDeadList.Add(health.IsCurrentlyDead);
                    }
                }
            }

            return isDeadList;
        }
        /// <summary>
        /// Gets all segments above the given segment
        /// </summary>
        /// <param name="wormSegment"></param>
        private List<WormSegment> GetSegmentsAboveKilledSegment(WormSegment wormSegment)
        {
            List<WormSegment> localSegments = new List<WormSegment>();
            int index = childSegments.IndexOf(wormSegment);

            for (int currentIndex = index; currentIndex < childSegments.Count; currentIndex++)
            {
                if (childSegments[currentIndex])
                {
                    localSegments.Add(childSegments[currentIndex]);
                }
            }

            return localSegments;
        }
        #endregion
    }
}