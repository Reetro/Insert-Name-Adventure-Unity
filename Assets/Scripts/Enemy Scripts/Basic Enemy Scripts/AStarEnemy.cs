using UnityEngine;
using Pathfinding;

namespace EnemyCharacter.AI
{
    [RequireComponent(typeof(Seeker))]
    public class AStarEnemy : EnemyBase
    {
        [Tooltip("Distance to next waypoint on A* grid")]
        [SerializeField] private float nextWaypointDistance = 3f;

        public override void OnSceneCreated()
        {
            base.OnSceneCreated();

            OnAStarEnemySpawn();

            InvokeRepeating("UpdatePath", 0f, 0.5f);
        }
        /// <summary>
        /// Called every 0.5 seconds will update the current AI Path
        /// </summary>
        protected virtual void UpdatePath()
        {
            if (MySeeker.IsDone())
            {
                MySeeker.StartPath(transform.position, Target.position, OnPathComplete);
            }
        }
        /// <summary>
        /// Called After a Scene has been created
        /// </summary>
        protected virtual void OnAStarEnemySpawn()
        {
            MySeeker = GetComponent<Seeker>();

            Target = PlayerTransform;

            if (!MySeeker)
            {
                Debug.LogError("Enemy " + name + " Does not have a a Seeker Component");
            }
        }
        /// <summary>
        /// Called when AI has finished Generating a path
        /// </summary>
        protected virtual void OnPathComplete(Path generatedPath)
        {
            if (!generatedPath.error)
            {
                CurrentPath = generatedPath;
                CurrentWaypoint = 0;
            }
        }

        #region Properties
        /// <summary>
        /// Distance to next waypoint on A* grid
        /// </summary>
        public float NextWaypointDistance { get { return nextWaypointDistance; } }
        /// <summary>
        /// The current path this AI is moving on
        /// </summary>
        public Path CurrentPath { get; protected set; }
        /// <summary>
        /// The current waypoint this AI is on
        /// </summary>
        public int CurrentWaypoint { get; protected set; }
        /// <summary>
        /// Checks to see if the AI has reached the end of it's path
        /// </summary>
        public bool ReachedEndOfPath { get; protected set; }
        /// <summary>
        /// The seeker component that is attached to this AI
        /// </summary>
        public Seeker MySeeker { get; private set; }
        /// <summary>
        /// The AI's current Target
        /// </summary>
        public Transform Target { get; set; }
        #endregion
    }
}