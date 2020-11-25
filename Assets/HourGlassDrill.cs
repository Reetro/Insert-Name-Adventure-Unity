using System.Collections;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class HourGlassDrill : EnemyBase
    {
        [Tooltip("How long the Hour Glass waits before launching into the air")]
        [SerializeField] private float launchDelay = 1f;

        [Tooltip("How fast the Hour Glass launches up")]
        [SerializeField] private float launchSpeed = 2f;

        [Tooltip("How long the Hour Glass stays in the air for")]
        [SerializeField] private float moveToGroundDelay = 2f;

        [Tooltip("How long the Hour Glass waits before following the player")]
        [SerializeField] private float followDelay = 0.8f;

        [Tooltip("How fast the Hour Glass follows the player")]
        [SerializeField] private float followSpeed = 3f;

        [Tooltip("Multiplier Used To calculate Launch Distance")]
        [SerializeField] private float launchDistanceMultiplier = 2f;

        [Tooltip("Multiplier Used To calculate Sink Distance")]
        [SerializeField] private float sinkDistanceMultiplier = 2f;

        [Tooltip("How long before the Hour Glass sinks into the ground")]
        [SerializeField] private float sinkDelay = 2f;

        [Tooltip("How fast the Hour Glass sinks back into the ground")]
        [SerializeField] private float sinkSpeed = 2f;

        [Tooltip("Distance of the Hour Glass ground trace")]
        [SerializeField] private float moveToGroundDistance = 50f;

        [Tooltip("Set's which layers the worm considers ground")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        #region Local Variables
        private bool launchTimerRunning = false;
        private bool sinkTimerRunning = false;
        private Vector2 sinkTarget = Vector2.zero;
        private bool moveToGroundTimerRunning = false;
        private float defaultfollowDelay = 0f;
        private bool runFollowDelay = false;
        private Vector2 launchTarget = Vector2.zero;
        #endregion

        /// <summary>
        /// All Hour Glass Drill movement states
        /// </summary>
        public enum HourGlassDrillMovementState
        {
            /// <summary>
            /// Will match the player X coordinate
            /// </summary>
            FollowPlayer,
            /// <summary>
            /// Will Launch the Hour Glass up Into the Air
            /// </summary>
            LaunchHourGlass,
            /// <summary>
            /// Will Move the Hour Glass from the air on to the ground
            /// </summary>
            MoveToGround,
            /// <summary>
            /// Will sink the Hour Glass back down into the ground and reset rotation
            /// </summary>
            SinkToGround
        }
        /// <summary>
        /// Set Default Variables and start move timer
        /// </summary>
        public override void OnSceneCreated()
        {
            base.OnSceneCreated();

            defaultfollowDelay = followDelay;

            CurrentMovementState = HourGlassDrillMovementState.FollowPlayer;
        }

        #region Movement Functions
        /// <summary>
        /// Decide What Movement Type To Use
        /// </summary>
        private void Update()
        {
            switch (CurrentMovementState)
            {
                case HourGlassDrillMovementState.FollowPlayer:
                    FollowPlayerX();
                    break;
                case HourGlassDrillMovementState.LaunchHourGlass:
                    LaunchHourGlassToAir();
                    break;
                case HourGlassDrillMovementState.MoveToGround:
                    MoveHourGlassToGround();
                    break;
                case HourGlassDrillMovementState.SinkToGround:
                    SinkBackToGround();
                    break;
            }
        }
        /// <summary>
        /// Move towards Player X coordinate
        /// </summary>
        private void FollowPlayerX()
        {
            if (!launchTimerRunning)
            {
                StartCoroutine(LaunchTimer());
            }

            MyMovementComp.MoveAITowards(new Vector2(PlayerTransform.position.x, transform.position.y), followSpeed);
        }
        /// <summary>
        /// Launch Hour Glass Into the Air
        /// </summary>
        private void LaunchHourGlassToAir()
        {
            if (MyMovementComp.MoveAIToPoint(launchTarget, launchSpeed, 0.01f))
            {
                if (!moveToGroundTimerRunning)
                {
                    CurrentMovementState = HourGlassDrillMovementState.MoveToGround;

                    StartCoroutine(MoveToGroundDelay());
                }
            }
        }
        /// <summary>
        /// Will Launch The Hour Glass Back down the Ground
        /// </summary>
        private void MoveHourGlassToGround()
        {
            if (moveToGroundTimerRunning)
            {
                if (runFollowDelay)
                {
                    followDelay -= Time.deltaTime;
                }

                if (followDelay <= 0)
                {
                    runFollowDelay = false;

                    MyMovementComp.MoveAITowards(new Vector2(PlayerTransform.position.x, transform.position.y), followSpeed);
                } 
            }
            else
            {
                if (!HitGround())
                {
                    transform.Translate(Vector2.down * launchSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    followDelay = defaultfollowDelay;

                    if (!sinkTimerRunning)
                    {
                        StartCoroutine(SinkToGroundDelay());
                    }
                }
            }
        }
        /// <summary>
        /// Will Sink the Hour Glass back into the ground
        /// </summary>
        private void SinkBackToGround()
        {
            if (MyMovementComp.MoveAIToPoint(sinkTarget, sinkSpeed, 0.01f))
            {
                CurrentMovementState = HourGlassDrillMovementState.FollowPlayer;
            }
        }
        /// <summary>
        /// Checks to see if the Hour Glass Is Hitting the ground
        /// </summary>
        /// <returns></returns>
        private bool HitGround()
        {
            var hit = Physics2D.Raycast(transform.position, -GeneralFunctions.GetFaceingDirectionY(gameObject), moveToGroundDistance, whatIsGround);

            return hit;
        }

        #region Movement Timers
        /// <summary>
        /// Delay before the Hour Glass will launch up into the air
        /// </summary>
        private IEnumerator LaunchTimer()
        {
            launchTimerRunning = true;

            yield return new WaitForSeconds(launchDelay);

            launchTarget = GeneralFunctions.GetPoint(GeneralFunctions.GetFaceingDirectionY(gameObject), transform.position, launchDistanceMultiplier);

            CurrentMovementState = HourGlassDrillMovementState.LaunchHourGlass;

            launchTimerRunning = false;
        }
        /// <summary>
        /// Delay before the Hour Glass will sink back into the ground
        /// </summary>
        private IEnumerator SinkToGroundDelay()
        {
            sinkTimerRunning = true;

            yield return new WaitForSeconds(sinkDelay);

            CurrentMovementState = HourGlassDrillMovementState.SinkToGround;

            sinkTarget = GeneralFunctions.GetPoint(-GeneralFunctions.GetFaceingDirectionY(gameObject), transform.position, sinkDistanceMultiplier);

            sinkTimerRunning = false;
        }
        /// <summary>
        /// Delay before the Hour Glass will back the ground
        /// </summary>
        private IEnumerator MoveToGroundDelay()
        {
            moveToGroundTimerRunning = true;

            runFollowDelay = true;

            yield return new WaitForSeconds(moveToGroundDelay);

            moveToGroundTimerRunning = false;
        }
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the drills current movement state
        /// </summary>
        public HourGlassDrillMovementState CurrentMovementState { get; private set; }
        #endregion
    }
}