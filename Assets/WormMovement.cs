﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyCharacter.AI
{
    public class WormMovement : MonoBehaviour
    {
        [Tooltip("The stiffness of each worm segment")]
        [SerializeField] private float wormStiffness = 5f;
        [Tooltip("The health of each worm segment")]
        [SerializeField] private float segmentHealth = 6f;
        [Tooltip("Layers the worm will check as ground")]
        [SerializeField] private LayerMask whatIsGround = new LayerMask();

        #region Setup Functions
        /// <summary>
        /// Get all attached worm segments and update worm stiffness
        /// </summary>
        public void OnSceneCreated()
        {
            AllWormSegments = transform.GetComponentsInChildren<WormSegment>();

            for (int index = 0; index < AllWormSegments.Length; index++)
            {
                var wormSegment = AllWormSegments[index];

                if (wormSegment)
                {
                    wormSegment.MyWormMovement = this;

                    wormSegment.SegmentDeath.AddListener(OnSegmentKilled);

                    wormSegment.SetupSegment(index, segmentHealth, whatIsGround);

                    wormSegment.CheckCollision();
                }
            }

            UpdateWormStiffness();
        }
        /// <summary>
        /// Change the stiffness of each worm segment
        /// </summary>
        public void UpdateWormStiffness()
        {
            for (var index = 0; index < AllWormSegments.Length; index++)
            {
                var wormSegment = AllWormSegments[index];

                if (wormSegment)
                {
                    if (wormSegment.IsAboveGround)
                    {
                        var joint = wormSegment.GetComponent<FixedJoint2D>();

                        if (joint)
                        {
                            joint.frequency = wormStiffness;
                        }
                        else if (index > 0)
                        {
                            Debug.LogError("Failed to get Fixed Joint 2D On " + wormSegment.name);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets the bottom most segment above ground
        /// </summary>
        public WormSegment GetBottomMostSegment()
        {
            WormSegment foundSegment = null;

            foreach (WormSegment wormSegment in AllWormSegments)
            {
                if (wormSegment)
                {
                    if (wormSegment.IsAboveGround)
                    {
                        foundSegment = wormSegment;
                        break;
                    }
                }
            }

            return foundSegment;
        }
        #endregion

        #region bool functions
        /// <summary>
        /// Checks to see if the given index is the top most index in the AllWormSegments array
        /// </summary>
        /// <param name="index"></param>
        public bool IsTopMostSegment(int index)
        {
            return index == AllWormSegments.Length - 1;
        }
        /// <summary>
        /// Checks to see if all worm segments are dead
        /// </summary>
        public bool AllSegmentsAreDead()
        {
            foreach (WormSegment wormSegment in AllWormSegments)
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
        /// <summary>
        /// Gets the worm segment under the given segment
        /// </summary>
        /// <param name="index"></param>
        public WormSegment GetSegmentUnderIndex(int index)
        {
            var targetIndex = Mathf.Clamp(index - 1, 0, AllWormSegments.Length);

            return AllWormSegments[targetIndex];
        }
        #endregion

        #region Gameplay Functions
        /// <summary>
        /// Called whenever a worm segment is killed will kill all segments above the given index if all segments are dead it will destroy the worm object
        /// </summary>
        private void OnSegmentKilled(int index)
        {
            var segmentsToKill = GetSegmentsAboveIndex(index);

            foreach (WormSegment wormSegment in segmentsToKill)
            {
                if (wormSegment)
                {
                    GeneralFunctions.KillTarget(wormSegment.gameObject);
                }
            }

            if (AllSegmentsAreDead())
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// Gets all worm segments above the given index
        /// </summary>
        /// <param name="index"></param>
        public WormSegment[] GetSegmentsAboveIndex(int index)
        {
            List<WormSegment> wormSegments = new List<WormSegment>();

            for (int currentIndex = 0; currentIndex < AllWormSegments.Length; currentIndex++)
            {
                if (currentIndex > index)
                {
                    var wormSegment = AllWormSegments[currentIndex];

                    if (wormSegment)
                    {
                        wormSegments.Add(wormSegment);
                    }
                }
            }

            return wormSegments.ToArray();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets all worm segments attached to the worm object
        /// </summary>
        public WormSegment[] AllWormSegments { get; private set; }
        #endregion
    }
}