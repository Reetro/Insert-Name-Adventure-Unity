﻿using System.Collections;
using EnemyScripts.BasicEnemyScripts;
using GeneralScripts;
using GeneralScripts.GeneralComponents;
using StatusEffects;
using UnityEngine;

namespace EnemyScripts.TikiHeadScripts
{
    public class TikiHead : AStarEnemy
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

        [Tooltip("How far to the grounded raycast goes on the Y axis")]
        [SerializeField] private float traceYDistance = 1f;
        [Tooltip("How far to the grounded raycast goes on the X axis")]
        [SerializeField] private float traceXDistance = 1f;

        [Header("Camera Shake Settings")]

        [Tooltip("Intensity of the camera shake")]
        [SerializeField] private float cameraShakeIntensity = 1f;

        [Tooltip("How long the camera shake lasts for")]
        [SerializeField] private float cameraShakeTime = 0.5f;

        [Header("Sight Settings")]

        [Tooltip("What layers this object can see")]
        public LayerMask sightLayers;
        
        [SerializeField] private float sightRange = 10f;

        [Header("Follow References")]

        [SerializeField] private GameObject leftTarget = null;

        [SerializeField] private GameObject rightTarget = null;

        [Header("Debug Settings")]

        [Tooltip("Whether or not to draw debug info")]
        [SerializeField] private bool drawDebug = false;

        #region Local Variables
        private bool launchTimerRunning = false;
        private bool moveToGroundTimerRunning = false;
        private bool followTimerRunning = false;
        private Vector2 launchTarget = Vector2.zero;
        private bool hitWall = false;
        private SpriteRenderer spriteRenderer = null;
        private Vector3 defaultPlayerScale = Vector2.zero;
        private float defaultOpacity = 0f;
        private bool isPlayerSquished = false;
        private bool isFalling = false;
        private bool canMove = false;
        private BoxCollider2D colliderBox2D = null;
        private bool isTouchingGround = false;
        private TileDestroyer[] tileDestroyers;
        private bool hitBoundary = false;
        private bool appliedCameraShake = false;
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

            if (!leftTarget)
            {
                Debug.LogError("TikiHead " + name + " failed to get left target object");
            }

            if (!rightTarget)
            {
                Debug.LogError("TikiHead " + name + " failed to get right target object");
            }

            GetCurrentTargetTransform();

            spriteRenderer = GetComponent<SpriteRenderer>();

            CurrentMovementState = TikiHeadMovementState.Idle;
        }
        /// <summary>
        /// Set Default Variables and start move timer
        /// </summary>
        protected override void OnSceneLoadingDone()
        {
            if (!spriteRenderer)
            {
                Debug.LogError("Tiki Head " + name + " failed to get spriteRenderer");
            }

            defaultPlayerScale = PlayerTransform.localScale;
            defaultOpacity = spriteRenderer.color.a;

            colliderBox2D = GetComponent<BoxCollider2D>();

            squishEffect.onEffectEnd.AddListener(OnSquishEnd);

            tileDestroyers = GetComponentsInChildren<TileDestroyer>();

            if (IsAbovePlayer())
            {
                CurrentMovementState = TikiHeadMovementState.FollowPlayer;
            }
            else
            {
                SetLaunchPoint();

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
                if (!collision.gameObject) return;
                // Squish Player if Tiki Head is moving down
                if (!GeneralFunctions.IsObjectOnLayer("Player", collision.gameObject) || isPlayerSquished) return;
                
                if (GeneralFunctions.IsPlayerDead()) return;
                
                GeneralFunctions.ApplyStatusEffectToTarget(collision.gameObject, squishEffect);

                GeneralFunctions.ApplyDamageToTarget(collision.gameObject, damageToApply, true, gameObject);

                PlayerTransform.localScale = playerSquishScale;

                colliderBox2D.enabled = false;

                GeneralFunctions.GetPlayerSpear().DisableSpear();

                SetSpriteOpacity(spriteOpacity);

                isPlayerSquished = true;
            }
            else
            {
                if (!collision.gameObject) return;
                
                // Knockback player if Tiki Head is grounded
                if (!GeneralFunctions.IsObjectOnLayer("Player", collision.gameObject)) return;
                
                MyRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;

                Vector2 direction = transform.position - collision.transform.position;

                direction.Normalize();

                if (!GeneralFunctions.IsObjectAbove(collision.transform.position, transform.position))
                {
                    direction.y = 0;

                    // Invert Knockback Direction
                    direction.x = -direction.x;
                }
                else
                {
                    direction.x = 0;

                    // Invert Knockback Direction
                    direction.y = -direction.y;
                }

                GeneralFunctions.ApplyKnockback(collision.gameObject, direction * knockBackMultiplier, ForceMode2D.Impulse);

                GeneralFunctions.ApplyDamageToTarget(collision.gameObject, damageToApply, true, gameObject);

                MyRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        /// <summary>
        /// Cast a raycast down towards the ground to see if Tiki Head is grounded
        /// </summary>
        private bool TouchingGround()
        {
            Vector2 traceStart = transform.position;
            var traceEndBottom = -GeneralFunctions.GetFacingDirectionY(gameObject) * GeneralFunctions.GetSpriteHeight(spriteRenderer);
            var traceEndTop = GeneralFunctions.GetFacingDirectionY(gameObject) * GeneralFunctions.GetSpriteHeight(spriteRenderer);
            var traceEndRight = GeneralFunctions.GetFacingDirectionX(gameObject) * GeneralFunctions.GetSpriteWidth(spriteRenderer);
            var traceEndLeft = -GeneralFunctions.GetFacingDirectionX(gameObject) * GeneralFunctions.GetSpriteWidth(spriteRenderer);

            var bottomRaycastHits2D = Physics2D.Raycast(traceStart, traceEndBottom, traceYDistance, whatIsGround);
            var topRaycastHits2D = Physics2D.Raycast(traceStart, traceEndTop, traceYDistance, whatIsGround);
            var rightRaycastHits2D = Physics2D.Raycast(traceStart, traceEndRight, traceXDistance, whatIsGround);
            var leftRaycastHits2D = Physics2D.Raycast(traceStart, traceEndLeft, traceXDistance, whatIsGround);

            if (drawDebug)
            {
                Debug.DrawRay(traceStart, traceEndBottom * traceYDistance, bottomRaycastHits2D ? Color.green : Color.red);

                Debug.DrawRay(traceStart, traceEndTop * traceYDistance, topRaycastHits2D ? Color.green : Color.red);

                Debug.DrawRay(traceStart, traceEndRight * traceXDistance, rightRaycastHits2D ? Color.green : Color.red);

                Debug.DrawRay(traceStart, traceEndLeft * traceXDistance, leftRaycastHits2D ? Color.green : Color.red);
            }

            hitWall = rightRaycastHits2D || leftRaycastHits2D;

            return bottomRaycastHits2D || topRaycastHits2D || rightRaycastHits2D || leftRaycastHits2D;
        }
        /// <summary>
        /// Called when player squish effect has ended
        /// </summary>
        /// <param name="gameObject"></param>
        private void OnSquishEnd()
        {
            if (GeneralFunctions.IsPlayerDead()) return;
            
            PlayerTransform.localScale = defaultPlayerScale;

            GeneralFunctions.GetPlayerSpear().EnableSpear();

            isPlayerSquished = false;
        }
        /// <summary>
        /// Set the segment opacity the the given value
        /// </summary>
        /// <param name="newOpacity"></param>
        private void SetSpriteOpacity(float newOpacity)
        {
            var tmp = spriteRenderer.color;

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
        private void ResetSpriteOpacity()
        {
            var tmp = spriteRenderer.color;

            tmp.a = defaultOpacity;

            spriteRenderer.color = tmp;
        }
        #endregion

        #region Movement Functions
        /// <summary>
        /// Checks to see if the player is left or right of the TikiHead then sets the current move target accordingly
        /// </summary>
        private void GetCurrentTargetTransform()
        {
            if (GeneralFunctions.IsObjectLeftOrRight(transform, PlayerTransform))
            {
                Target = leftTarget.transform;
            }
            else
            {
                Target = rightTarget.transform;
            }
        }
        /// <summary>
        /// Decide What Movement Type To Use
        /// </summary>
        private void FixedUpdate()
        {
            GetCurrentTargetTransform();

            isTouchingGround = TouchingGround();

            switch (CurrentMovementState)
            {
                case TikiHeadMovementState.LaunchTikiHead:
                    //LaunchTikiHeadIntoAir();
                    break;
                case TikiHeadMovementState.FollowPlayer:
                    FollowPlayerX();
                    break;
                case TikiHeadMovementState.MoveToGround:
                    //MoveTikiHeadToGround();
                    break;
            }

            if (drawDebug)
            {
                print("Current Movement State is " + CurrentMovementState);
            }
        }
        /// <summary>
        /// Move towards Player X coordinate
        /// </summary>
        private void FollowPlayerX()
        {
            if (CurrentPath != null)
            {
                if (CurrentWaypoint >= CurrentPath.vectorPath.Count)
                {
                    ReachedEndOfPath = true;
                    return;
                }
                else
                {
                    ReachedEndOfPath = false;
                }

                Vector2 direction = ((Vector2)CurrentPath.vectorPath[CurrentWaypoint] - MyRigidBody2D.position).normalized;
                Vector2 force = direction * followSpeed * Time.fixedDeltaTime;

                MyRigidBody2D.AddForce(force);

                float distance = Vector2.Distance(transform.position, CurrentPath.vectorPath[CurrentWaypoint]);

                if (distance < NextWaypointDistance)
                {
                    CurrentWaypoint++;
                }
            }
            else
            {
                Debug.LogError(name + " Is unable to follow player current path is not valid");

                return;
            }
        }
        /// <summary>
        /// Setup Movement state Move To Ground
        /// </summary>
        /// <param name="startedMoveToGround"></param>
        private bool SetupMoveToGround(bool startedMoveToGround)
        {
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
            if (hitBoundary)
            {
                if (!moveToGroundTimerRunning)
                {
                    CurrentMovementState = TikiHeadMovementState.MoveToGround;

                    isTouchingGround = false;

                    /*if (!moveToGroundTimerRunning)
                    {
                        StartCoroutine(MoveToGroundDelay());
                    }*/
                }
            }
            else
            {
                ResetSpriteOpacity();

                colliderBox2D.enabled = true;

                spriteRenderer.color = Color.white;

                transform.Translate(transform.up * launchSpeed * Time.deltaTime);
            }
        }
        /// <summary>
        /// On Camera collision stop moving up
        /// </summary>
        protected override void OnCameraTopCollision()
        {
            hitBoundary = true;
        }
        /// <summary>
        /// Will Launch The Tiki Head Back down the Ground
        /// </summary>
        private void MoveTikiHeadToGround()
        {
            if (moveToGroundTimerRunning)
            {
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

                    print("Tiki Touching Hit Wall: " + hitWall);
                }

                if (!isTouchingGround && canMove || hitWall && canMove)
                {
                    if (hitWall)
                    {
                        hitWall = false;
                    }

                    appliedCameraShake = false;

                    EnableTileDestroyers();

                    isFalling = true;

                    transform.Translate(-transform.up * launchSpeed * Time.deltaTime);
                }
                
                if (isTouchingGround)
                {
                    DisableTileDestroyers();

                    isFalling = false;

                    if (!appliedCameraShake)
                    {
                        appliedCameraShake = true;

                        GeneralFunctions.ShakeCamera(cameraShakeIntensity, cameraShakeTime);
                    }

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
            launchTarget = GeneralFunctions.GetPoint(GeneralFunctions.GetFacingDirectionY(gameObject), transform.position, launchDistanceMultiplier);
        }
        /// <summary>
        /// Subtracts the Tiki Head y cord from the player's y cord to determine if it's above the player
        /// </summary>
        private bool IsAbovePlayer()
        {
            return transform.position.y - PlayerTransform.position.y > yDistanceTolerance;
        }

        #region Movement Timers
        /// <summary>
        /// Delay before the Tiki Head will launch up into the air
        /// </summary>
        private IEnumerator LaunchTimer()
        {
            launchTimerRunning = true;

            yield return new WaitForSeconds(launchDelay);

            hitBoundary = false;

            SetLaunchPoint();

            if (drawDebug)
            {
                Debug.DrawRay(transform.position, GeneralFunctions.GetFacingDirectionY(gameObject) * launchDistanceMultiplier, Color.red);
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