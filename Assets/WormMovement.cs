using System.Collections;
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

        /// <summary>
        /// Get all attached worm segments and update worm stiffness
        /// </summary>
        public void OnSceneCreated()
        {
            var segments = transform.GetComponentsInChildren<WormSegment>();

            for (int index = 0; index < segments.Length; index++)
            {
                var wormSegment = segments[index];

                if (wormSegment)
                {
                    AllWormSegments.Add(wormSegment);

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
            for (var index = 0; index < AllWormSegments.Count; index++)
            {
                var wormSegment = AllWormSegments[index];

                if (wormSegment)
                {
                    if (wormSegment.isAboveGround)
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
                    if (wormSegment.isAboveGround)
                    {
                        foundSegment = wormSegment;
                        break;
                    }
                }
            }

            return foundSegment;
        }

        #region Properties
        /// <summary>
        /// Gets all worm segments attached to the worm object
        /// </summary>
        public List<WormSegment> AllWormSegments { get; private set; } = new List<WormSegment>();
        #endregion
    }
}