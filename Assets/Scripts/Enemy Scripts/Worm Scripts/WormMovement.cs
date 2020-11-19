﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : MonoBehaviour
    {
        [Tooltip("How much health each segment has")]
        [SerializeField] private float segmentHealth = 6;

        [Tooltip("How long the Worm waits before launching into the air")]
        [SerializeField] private float launchDelay = 1f;
        
        [Tooltip("How fast the Worm launches up")]
        [SerializeField] private float launchSpeed = 2f;
        
        [Tooltip("How long the Worm's launches into the air for")]
        [SerializeField] private float launchTimer = 1f;

        [Tooltip("How long the Worm stays in the air for")]
        [SerializeField] private float inAirTimer = 2f;
        
        [Tooltip("How long before the Worm's sinks into the ground")]
        [SerializeField] private float sinkDelay = 2f;
        
        [Tooltip("Distance of the Worm's ground trace")]
        [SerializeField] private float moveToGroundDistance = 50f;
        
        [Tooltip("Amount to subtract from the Worm's Y cord when sinking into the ground")]
        [SerializeField] private float sinkSubtractAmount = 5.5f;
        
        [Tooltip("The Worm's target rotation the air")]
        [SerializeField] private float wormTargetRotation = -90;
        
        [Tooltip("Set's which layers the worm considers ground")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        #region Local Variables
        private Vector2 playerPostion = new Vector2();
        private float deafaultInAirTimer = 0f;
        private float deafaultSinkDelay = 0f;
        private float defaultLaunchTimer = 0f;
        private bool runLaunchTimer = false;
        private bool runMoveTimer = false;
        private bool runSinkTimer = false;
        private bool gotSinkTarget = false;
        private Vector2 sinkTarget = Vector2.zero;
        #endregion

        /// <summary>
        /// All worm movement states
        /// </summary>
        public enum WormMovementState
        {
            /// <summary>
            /// Will match the player X coordinate
            /// </summary>
            FollowPlayer,
            /// <summary>
            /// Will Launch the Worm up Into the Air
            /// </summary>
            IsLaunching,
            /// <summary>
            /// Will Rotate the Worm to -90 degrees
            /// </summary>
            RotateInAir,
            /// <summary>
            /// Will Move the Worm from the air on to the ground
            /// </summary>
            MoveToGround,
            /// <summary>
            /// Will sink the Worm back down into the ground and reset rotation
            /// </summary>
            SinkToGround
        }

        /// <summary>
        /// Called right after the SceneCreator has setup the Player Gameobject
        /// </summary>
        public void OnSceneCreated()
        {
            deafaultInAirTimer = inAirTimer;
            deafaultSinkDelay = sinkDelay;
            defaultLaunchTimer = launchTimer;

            AllChildSegments = GetComponentsInChildren<WormSegment>();

            for (int index = 0; index < AllChildSegments.Length; index++)
            {
                var segment = AllChildSegments[index];

                if (segment)
                {
                    segment.SetupWormSegment(index, segmentHealth, whatIsGround);

                    segment.SegmentDeath.AddListener(OnSegmentDeath);
                }
            }

            TopMostWormSegment = AllChildSegments[AllChildSegments.Length - 1];

            SegmentToRotate = AllChildSegments[0];

            if (!AreAllWormSegmentsUp())
            {
                CurrentMovementState = WormMovementState.FollowPlayer;
            }

            StartCoroutine(StartLaunch());
        }

        #region Movement Code
        /// <summary>
        /// Check to see if worm segments are above ground if so enable collision         
        /// </summary>
        private void Update()
        {
            foreach (WormSegment wormSegment in AllChildSegments)
            {
                if (wormSegment)
                {
                    if (!wormSegment.MyHealthComponent.IsCurrentlyDead)
                    {
                        if (wormSegment.IsOverlappingGround())
                        {
                            wormSegment.DisableCollision();
                        }
                        else
                        {
                            wormSegment.EnableCollision();
                        }
                    }
                }
            }

            switch (CurrentMovementState)
            {
                case WormMovementState.FollowPlayer:
                    FollowPlayerX();
                    break;
                case WormMovementState.IsLaunching:
                    LaunchWorm();
                    break;
                case WormMovementState.RotateInAir:
                    RotateWorm();
                    break;
                case WormMovementState.MoveToGround:
                    MoveWormToGround();
                    break;
                case WormMovementState.SinkToGround:
                    SinkWormToGround();
                    break;
            }
        }
        /// <summary>
        /// Follow player along the X axis
        /// </summary>
        private void FollowPlayerX()
        {
            playerPostion = GeneralFunctions.GetPlayerGameObject().transform.position;

            transform.position = new Vector3(playerPostion.x, transform.position.y, transform.position.z);
        }
        /// <summary>
        /// Launch the worm up towards the launch end point
        /// </summary>
        private void LaunchWorm()
        {
            if (runLaunchTimer)
            {
                launchTimer -= Time.deltaTime;

                transform.Translate(Vector2.up * launchSpeed * Time.deltaTime, Space.World);
            }

            if (launchTimer <= 0)
            {
                launchTimer = defaultLaunchTimer;

                runLaunchTimer = false;

                CurrentMovementState = WormMovementState.RotateInAir;
            }
        }
        /// <summary>
        /// Rotate Worm down to -90 degrees
        /// </summary>
        private void RotateWorm()
        {
            var targetRotation = new Vector3(0, 0, wormTargetRotation);

            SegmentToRotate.transform.Rotate(targetRotation);

            CurrentMovementState = WormMovementState.MoveToGround;
            runMoveTimer = true;
        }
        /// <summary>
        /// Move the worm back to the ground
        /// </summary>
        private void MoveWormToGround()
        {
            if (runMoveTimer)
            {
                inAirTimer -= Time.deltaTime;
            }

            if (inAirTimer <= 0)
            {
                runMoveTimer = false;

                var targetLocation = GetPoint(-GeneralFunctions.GetFaceingDirectionY(transform.gameObject), moveToGroundDistance, transform.position);

                float step = launchSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetLocation, step);

                if (Vector3.Distance(transform.position, targetLocation) < 0.001f)
                {
                    CurrentMovementState = WormMovementState.SinkToGround;

                    inAirTimer = deafaultInAirTimer;

                    runSinkTimer = true;
                }
            }
        }
        /// <summary>
        /// Sink the worm back into the ground and reset rotation back to 0
        /// </summary>
        private void SinkWormToGround()
        {
            if (runSinkTimer)
            {
                sinkDelay -= Time.deltaTime;
            }
            
            if (sinkDelay <= 0)
            {
                if (!gotSinkTarget)
                {
                    gotSinkTarget = true;

                    sinkTarget = new Vector2(transform.position.x, transform.position.y - sinkSubtractAmount);
                }

                float step = launchSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, sinkTarget, step);

                if (Vector3.Distance(transform.position, sinkTarget) < 0.001f)
                {
                    SegmentToRotate.transform.rotation = new Quaternion(0, 0, 0, 0);

                    CurrentMovementState = WormMovementState.FollowPlayer;

                    sinkDelay = deafaultSinkDelay;

                    StartCoroutine(StartLaunch());
                }
            }
        }
        /// <summary>
        /// Gets a point X units away from the facing directions
        /// </summary>
        private Vector2 GetPoint(Vector2 facingDirection, float distance, Vector2 startPostion)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(startPostion, facingDirection, distance, whatIsGround);

            if (raycastHit2D)
            {
                return raycastHit2D.point;
            }
            else
            {
                return facingDirection * distance;
            }
        }
        /// <summary>
        /// Get the worm launch end point
        /// </summary>
        private IEnumerator StartLaunch()
        {
            yield return new WaitForSeconds(launchDelay);

            runLaunchTimer = true;

            CurrentMovementState = WormMovementState.IsLaunching;
        }
        /// <summary>
        /// Checks to see if all worm segments are above ground
        /// </summary>
        public bool AreAllWormSegmentsUp()
        {
            foreach (WormSegment wormSegment in AllChildSegments)
            {
                if (wormSegment)
                {
                    if (!wormSegment.AboveGround)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region Health Functions
        /// <summary>
        /// Called whenever a worm segment dies
        /// </summary>
        private void OnSegmentDeath(WormSegment wormSegment)
        {
            var segmentsToKill = GetSegmentsAboveIndex(wormSegment.Index);

            foreach (WormSegment segment in segmentsToKill)
            {
                if (segment)
                {
                    GeneralFunctions.KillTarget(segment.gameObject);
                }
            }

            if (AreAllSegmentsDead())
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// Gets all worm segments above the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>An array of worm segments</returns>
        public WormSegment[] GetSegmentsAboveIndex(int index)
        {
            List<WormSegment> foundSegments = new List<WormSegment>();

            for (int currentIndex = index; currentIndex < AllChildSegments.Length; currentIndex++)
            {
                var segment = AllChildSegments[currentIndex];

                if (segment)
                {
                    if (!segment.MyHealthComponent.IsCurrentlyDead)
                    {
                        foundSegments.Add(segment);
                    }
                }
            }

            return foundSegments.ToArray();
        }
        /// <summary>
        /// Checks to see if all worm segments are dead
        /// </summary>
        private bool AreAllSegmentsDead()
        {
            foreach (WormSegment wormSegment in AllChildSegments)
            {
                if (wormSegment)
                {
                    if (!wormSegment.MyHealthComponent.IsCurrentlyDead)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// An array of all child segments
        /// </summary>
        public WormSegment[] AllChildSegments { get; private set; }
        /// <summary>
        /// Gets the current movement state of the worm
        /// </summary>
        public WormMovementState CurrentMovementState { get; private set; }
        /// <summary>
        /// Gets the segment the worm will rotate always the bottom most worm segment
        /// </summary>
        public WormSegment SegmentToRotate { get; private set; }
        /// <summary>
        /// Gets the worms top most segment
        /// </summary>
        public WormSegment TopMostWormSegment { get; private set; }
        #endregion
    }
}
