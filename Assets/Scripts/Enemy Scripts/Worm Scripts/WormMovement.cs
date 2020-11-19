using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : MonoBehaviour
    {
        /// <summary>
        /// How much health each segment has
        /// </summary>
        [SerializeField] private float segmentHealth = 6;
        /// <summary>
        /// Set's which layers the worm considers ground
        /// </summary>
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        [SerializeField] private float launchDelay = 1f;

        [SerializeField] private float launchSpeed = 2f;

        [SerializeField] private float endPointDistance = 10f;

        [SerializeField] private float inAirTimer = 2f;

        [SerializeField] private float sinkDelay = 2f;

        private Vector2 playerPostion = new Vector2();
        private Vector3 launchEndPoint = new Vector3();
        private float deafaultInAirTimer = 0f;
        private float deafaultSinkDelay = 0f;
        private bool runMoveTimer = false;
        private bool runSinkTimer = false;
        private bool gotSinkTarget = false;
        private Vector2 sinkTarget = Vector2.zero;

        /// <summary>
        /// All worm movement types
        /// </summary>
        public enum WormMovementState
        {
            FollowPlayer,
            IsLaunching,
            RotateInAir,
            MoveToGround,
            SinkToGround
        }

        /// <summary>
        /// Called right after the SceneCreator has setup the Player Gameobject
        /// </summary>
        public void OnSceneCreated()
        {
            deafaultInAirTimer = inAirTimer;
            deafaultSinkDelay = sinkDelay;

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
            float step = launchSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, launchEndPoint, step);

            if (Vector3.Distance(transform.position, launchEndPoint) < 0.001f)
            {
                CurrentMovementState = WormMovementState.RotateInAir;
            }
        }
        /// <summary>
        /// Rotate Worm down to -90 degrees
        /// </summary>
        private void RotateWorm()
        {
            var targetRotation = new Vector3(0, 0, -90);

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

                var targetLocation = GetPoint(-GeneralFunctions.GetFaceingDirectionY(transform.gameObject), 50f, transform.position);

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

                    sinkTarget = (Vector2)transform.position + new Vector2(0, -5.5f);
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

            launchEndPoint = GetPoint(GeneralFunctions.GetFaceingDirectionY(TopMostWormSegment.gameObject), endPointDistance, TopMostWormSegment.transform.position);

            print(launchEndPoint);

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
