using System.Collections;
using UnityEngine;
using StatusEffects;
using ComponentLibrary;

namespace EnemyCharacter.AI
{
    public class TikiHead : EnemyBase
    {
        [Header("Launch Settings")]

        [Tooltip("How long the Tiki Head waits before launching into the air")]
        [SerializeField] private float launchDelay = 1f;

        [Tooltip("How fast the Tiki Head launches up")]
        [SerializeField] private float launchSpeed = 2f;

        [Tooltip("Multiplier Used To calculate Launch Distance")]
        [SerializeField] private float launchDistanceMultiplier = 2f;

        [Header("Move To Ground Settings")]

        [Tooltip("How long the Tiki Head stays in the air for")]
        [SerializeField] private float moveToGroundDelay = 2f;

        [Tooltip("How close the Tiki Heads Y cord can be to the players")]
        [SerializeField] private float yDistanceTolerance = 1f;

        [Header("Follow Settings")]

        [Tooltip("How long the Tiki Head waits before following the player")]
        [SerializeField] private float followDelay = 0.8f;

        [Tooltip("How fast the Tiki Head follows the player")]
        [SerializeField] private float followSpeed = 3f;

        [Header("Squish Settings")]

        [Tooltip("Scale to set the player to when squished")]
        [SerializeField] private Vector3 playerSquishScale = new Vector3(1f, 1f, 1f);

        [Tooltip("Damage to apply to the player when squished")]
        [SerializeField] private float damageToApply = 2f;

        [Tooltip("The Status Effect to apply when the player squished")]
        [SerializeField] private ScriptableStatusEffect squishEffect = null;

        [Range(0, 1)]
        [Tooltip("The opacity to change the sprite to when player is squished")]
        [SerializeField] private float spriteOpacity = 0.5f;

        [Header("Knockback Settings")]

        [Tooltip("Multiplier for player Knockback")]
        [SerializeField] private float knockBackMultiplier = 200f;

        [Header("Collision Settings")]

        [Tooltip("Set's which layers the Tiki Head considers ground")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        [Tooltip("How far to the grounded raycast")]
        [SerializeField] private float traceDistance = 1f;

        [Header("Sight Settings")]

        [Tooltip("What layers this object can see")]
        public LayerMask sightLayers;
        
        [SerializeField] private float sightRange = 10f;

        [Header("Debug Settings")]

        [Tooltip("Whether or not to draw debug info")]
        [SerializeField] private bool drawDebug = false;

        #region Local Variables
        private bool launchTimerRunning = false;
        private bool moveToGroundTimerRunning = false;
        private bool followTimerRunning = false;
        private Vector2 launchTarget = Vector2.zero;
        private SpriteRenderer spriteRenderer = null;
        private Vector3 defaultPlayerScale = Vector2.zero;
        private float defaultOpacity = 0f;
        private bool isPlayerSquished = false;
        private bool isFalling = false;
        private bool canMove = false;
        private bool isLaunching = false;
        private BoxCollider2D colliderBox2D = null;
        private bool isTouchingGround = false;
        private bool wasAbove = false;
        private bool skipVisCheck = false;
        private TileDestroyer[] tileDestroyers;
        private Boundaries boundaries = null;
        #endregion

        /// <summary>
        /// All Tiki Head movement states
        /// </summary>
        public enum TikiHeadMovementState
        {
            /// <summary>
            /// Will Launch the Tiki Head up Into the Air
            /// </summary>
            LaunchTikiHead,
            /// <summary>
            /// Will Move the Tiki Head from the air on to the ground
            /// </summary>
            MoveToGround,
            /// <summary>
            /// Will match the player X coordinate
            /// </summary>
            FollowPlayer,
            /// <summary>
            /// Tiki Head will not move
            /// </summary>
            Idle
        }
        /// <summary>
        /// Freeze Tiki Head Movement
        /// </summary>
        public override void OnSceneCreated()
        {
            base.OnSceneCreated();

            CurrentMovementState = TikiHeadMovementState.Idle;
        }
        /// <summary>
        /// Set Default Variables and start move timer
        /// </summary>
        protected override void OnSceneLoadingDone()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (!spriteRenderer)
            {
                Debug.LogError("Tiki Head " + name + " failed to get spriteRenderer");
            }

            boundaries = GetComponent<Boundaries>();

            if (!boundaries)
            {
                Debug.LogError("Tiki Head " + name + " failed to get boundaries");
            }

            defaultPlayerScale = PlayerTransform.localScale;
            defaultOpacity = spriteRenderer.color.a;

            colliderBox2D = GetComponent<BoxCollider2D>();

            squishEffect.OnEffectEnd.AddListener(OnSquishEnd);

            tileDestroyers = GetComponentsInChildren<TileDestroyer>();

            if (IsAbovePlayer())
            {
                skipVisCheck = true;

                boundaries.DoCheck = false;

                CurrentMovementState = TikiHeadMovementState.FollowPlayer;
            }
            else
            {
                skipVisCheck = false;

                SetLaunchPoint();

                boundaries.DoCheck = true;

                CurrentMovementState = TikiHeadMovementState.LaunchTikiHead;
            }
        }
        /// <summary>
        /// Destroy Gameobject on death
        /// </summary>
        protected override void OnDeath()
        {
            Destroy(gameObject);
        }

        #region Collision Functions
        /// <summary>
        /// When Drill collides with player apply squish effect and damage player
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isFalling)
            {
                if (collision.gameObject)
                {
                    // Squish Player if Tiki Head is moving down
                    if (GeneralFunctions.IsObjectOnLayer("Player", collision.gameObject) && !isPlayerSquished)
                    {
                        if (!GeneralFunctions.IsPlayerDead())
                        {
                            GeneralFunctions.ApplyStatusEffectToTarget(collision.gameObject, squishEffect);

                            GeneralFunctions.DamageTarget(collision.gameObject, damageToApply, true, gameObject);

                            PlayerTransform.localScale = playerSquishScale;

                            colliderBox2D.enabled = false;

                            GeneralFunctions.GetPlayerSpear().DisableSpear();

                            SetSpriteOpacity(spriteOpacity);

                            isPlayerSquished = true;
                        }
                    }
                }
            }
            else
            {
                if (collision.gameObject)
                {
                    // Knockback player if Tiki Head is grounded
                    if (GeneralFunctions.IsObjectOnLayer("Player", collision.gameObject))
                    {
                        if (!isLaunching && !GeneralFunctions.IsObjectAbove(PlayerTransform.position, transform.position))
                        {
                            if (GeneralFunctions.IsObjectLeftOrRight(transform, PlayerTransform))
                            {
                                GeneralFunctions.ApplyKnockback(collision.gameObject, -transform.right * knockBackMultiplier, ForceMode2D.Impulse);

                                GeneralFunctions.DamageTarget(collision.gameObject, damageToApply, true, gameObject);
                            }
                            else
                            {
                                GeneralFunctions.ApplyKnockback(collision.gameObject, transform.right * knockBackMultiplier, ForceMode2D.Impulse);

                                GeneralFunctions.DamageTarget(collision.gameObject, damageToApply, true, gameObject);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Cast a raycast down towards the ground to see if Tiki Head is grounded
        /// </summary>
        private bool TouchingGround()
        {
            Vector2 traceStart = transform.position;
            Vector2 traceEnd = -GeneralFunctions.GetFaceingDirectionY(gameObject);

            RaycastHit2D raycastHits2D = Physics2D.Raycast(traceStart, traceEnd, traceDistance, whatIsGround);

            if (raycastHits2D)
            {
                if (drawDebug)
                {
                    Debug.DrawRay(traceStart, traceEnd, Color.green);
                }

                return true;
            }
            else
            {
                if (drawDebug)
                {
                    Debug.DrawRay(traceStart, traceEnd, Color.red);
                }

                return false;
            }
        }
        /// <summary>
        /// Called when player squish effect has ended
        /// </summary>
        /// <param name="gameObject"></param>
        private void OnSquishEnd()
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                PlayerTransform.localScale = defaultPlayerScale;

                GeneralFunctions.GetPlayerSpear().EnableSpear();

                isPlayerSquished = false;
            }
        }
        /// <summary>
        /// Set the segment opacity the the given value
        /// </summary>
        /// <param name="newOpacity"></param>
        public void SetSpriteOpacity(float newOpacity)
        {
            Color tmp = spriteRenderer.color;

            tmp.a = newOpacity;
            tmp.r = 1f;
            tmp.g = 1f;
            tmp.b = 1f;

            spriteRenderer.color = tmp;
        }
        /// <summary>
        /// Set the segment opacity back to the default value
        /// </summary>
        /// <param name="newOpacity"></param>
        public void ResetSpriteOpacity()
        {
            Color tmp = spriteRenderer.color;

            tmp.a = defaultOpacity;

            spriteRenderer.color = tmp;
        }
        #endregion

        #region Movement Functions
        /// <summary>
        /// Decide What Movement Type To Use
        /// </summary>
        private void Update()
        {
            isTouchingGround = TouchingGround();

            switch (CurrentMovementState)
            {
                case TikiHeadMovementState.LaunchTikiHead:
                    LaunchTikiHeadIntoAir();
                    break;
                case TikiHeadMovementState.FollowPlayer:
                    FollowPlayerX();
                    break;
                case TikiHeadMovementState.MoveToGround:
                    MoveTikiHeadToGround();
                    break;
            }
        }
        /// <summary>
        /// Move towards Player X coordinate
        /// </summary>
        private void FollowPlayerX()
        {
            bool startedMoveToGround = false;

            boundaries.DoCheck = false;

            if (wasAbove)
            {
                var visible = MyMovementComp.IsTransformVisible(sightLayers, transform, PlayerTransform, "Player", sightRange, drawDebug);

                if (visible)
                {
                    startedMoveToGround = SetupMoveToGround(startedMoveToGround);
                }
            }
            else if (skipVisCheck)
            {
                startedMoveToGround = SetupMoveToGround(startedMoveToGround);
            }
            else if (!launchTimerRunning && !startedMoveToGround)
            {
                StartCoroutine(LaunchTimer());
            }

            MyMovementComp.MoveAITowards(new Vector2(PlayerTransform.position.x, transform.position.y), followSpeed);
        }
        /// <summary>
        /// Setup Movement state Move To Ground
        /// </summary>
        /// <param name="startedMoveToGround"></param>
        private bool SetupMoveToGround(bool startedMoveToGround)
        {
            skipVisCheck = false;

            boundaries.DoCheck = false;

            wasAbove = false;

            startedMoveToGround = true;

            CurrentMovementState = TikiHeadMovementState.MoveToGround;

            if (!moveToGroundTimerRunning)
            {
                StartCoroutine(MoveToGroundDelay());
            }

            return startedMoveToGround;
        }
        /// <summary>
        /// Launch Tiki Head Into the Air
        /// </summary>
        private void LaunchTikiHeadIntoAir()
        {
            if (MyMovementComp.MoveAIToPoint(launchTarget, launchSpeed, 0.01f, out isLaunching) || IsAtMaxHeight())
            {
                ResetSpriteOpacity();

                colliderBox2D.enabled = true;

                spriteRenderer.color = Color.white;

                if (!moveToGroundTimerRunning)
                {
                    isTouchingGround = false;

                    CurrentMovementState = TikiHeadMovementState.MoveToGround;

                    if (!moveToGroundTimerRunning)
                    {
                        StartCoroutine(MoveToGroundDelay());
                    }
                }
            }

            if (drawDebug)
            {
                print("Launching to point: " + launchTarget);
            }
        }
        /// <summary>
        /// Will Launch The Tiki Head Back down the Ground
        /// </summary>
        private void MoveTikiHeadToGround()
        {
            if (moveToGroundTimerRunning)
            {
                boundaries.DoCheck = false;

                if (!followTimerRunning)
                {
                    MyMovementComp.MoveAITowards(new Vector2(PlayerTransform.position.x, transform.position.y), followSpeed);
                }
                else
                {
                    StartCoroutine(FollowTimer());
                }
            }
            else
            {
                if (drawDebug)
                {
                    print("Tiki Touching Ground: " + isTouchingGround);
                }

                if (!isTouchingGround && canMove)
                {
                    EnableTileDestroyers();

                    isFalling = true;

                    transform.Translate(Vector2.down * launchSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    DisableTileDestroyers();

                    isFalling = false;

                    if (!launchTimerRunning)
                    {
                        StartCoroutine(LaunchTimer());
                    }
                }
            }
        }
        /// <summary>
        /// Set CanDestroyTile in all tile destroyers to true
        /// </summary>
        private void EnableTileDestroyers()
        {
            foreach (TileDestroyer tileDestroyer in tileDestroyers)
            {
                if (tileDestroyer)
                {
                    tileDestroyer.CanDestroyTile = true;
                }
            }
        }
        /// <summary>
        /// Set CanDestroyTile in all tile destroyers to false
        /// </summary>
        private void DisableTileDestroyers()
        {
            foreach (TileDestroyer tileDestroyer in tileDestroyers)
            {
                if (tileDestroyer)
                {
                    tileDestroyer.CanDestroyTile = false;
                }
            }
        }
        /// <summary>
        /// Calculates the next launch point
        /// </summary>
        private void SetLaunchPoint()
        {
            launchTarget = GeneralFunctions.GetPoint(GeneralFunctions.GetFaceingDirectionY(gameObject), transform.position, launchDistanceMultiplier);
        }
        /// <summary>
        /// Subtracts the Tiki Head y cord from the player's y cord to determine if it's above the player
        /// </summary>
        private bool IsAbovePlayer()
        {
            return transform.position.y - PlayerTransform.position.y > yDistanceTolerance;
        }
        /// <summary>
        /// Checks to see if the Tiki Head is at max height
        /// </summary>
        private bool IsAtMaxHeight()
        {
            return boundaries.MaxCords.y - transform.position.y <= 0.01;
        }

        #region Movement Timers
        /// <summary>
        /// Delay before the Tiki Head will launch up into the air
        /// </summary>
        private IEnumerator LaunchTimer()
        {
            launchTimerRunning = true;

            yield return new WaitForSeconds(launchDelay);

            boundaries.DoCheck = true;

            SetLaunchPoint();

            if (drawDebug)
            {
                Debug.DrawRay(transform.position, GeneralFunctions.GetFaceingDirectionY(gameObject) * launchDistanceMultiplier, Color.red);
            }

            CurrentMovementState = TikiHeadMovementState.LaunchTikiHead;

            launchTimerRunning = false;
        }
        /// <summary>
        /// Delay before the Tiki Head will back the ground
        /// </summary>
        private IEnumerator MoveToGroundDelay()
        {
            moveToGroundTimerRunning = true;

            followTimerRunning = true;

            yield return new WaitForSeconds(moveToGroundDelay);

            spriteRenderer.color = Color.green;

            canMove = true;

            moveToGroundTimerRunning = false;
        }
        /// <summary>
        /// Delay before the Tiki Head follows the player
        /// </summary>
        private IEnumerator FollowTimer()
        {
            followTimerRunning = true;

            yield return new WaitForSeconds(followDelay);

            followTimerRunning = false;
        }
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the drills current movement state
        /// </summary>
        public TikiHeadMovementState CurrentMovementState { get; private set; }
        #endregion
    }
}