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
        /// Called right after the SceneCreator has setup the Player Gameobject
        /// </summary>
        public void OnSceneCreated()
        {
            AllChildSegments = GetComponentsInChildren<WormSegment>();

            for (int index = 0; index < AllChildSegments.Length; index++)
            {
                var segment = AllChildSegments[index];

                if (segment)
                {
                    segment.SetupWormSegment(index, segmentHealth);

                    segment.SegmentDeath.AddListener(OnSegmentDeath);
                }
            }
        }

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
        #endregion
    }
}
