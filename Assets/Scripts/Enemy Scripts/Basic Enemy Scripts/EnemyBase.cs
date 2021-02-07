using UnityEngine;
using EnemyCharacter.AI;
using ComponentLibrary;

namespace EnemyCharacter
{
    [RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(GameplayObjectID))]
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(EnemyMovement))]
    public class EnemyBase : MonoBehaviour
    {
        private GameplayObjectID idObject = null;

        /// <summary>
        /// Called right after the SceneCreator has setup the Player Gameobject
        /// </summary>
        public virtual void OnSceneCreated()
        {
            PlayerTransform = GeneralFunctions.GetPlayerGameObject().transform;
            MyHealthComponent = GetComponent<HealthComponent>();
            MyRigidBody2D = GetComponent<Rigidbody2D>();
            idObject = GetComponent<GameplayObjectID>();
            MyMovementComp = GetComponent<EnemyMovement>();
            MyAnimator = GetComponent<Animator>();

            idObject.ConstructID();
            MyHealthComponent.ConstructHealthComponent();
            MyHealthComponent.OnDeath.AddListener(OnDeath);
            GeneralFunctions.GetGameplayManager().onCameraTopOverlap.AddListener(OnCameraTopCollision);
            GeneralFunctions.GetGameplayManager().onCameraBottomOverlap.AddListener(OnCameraBottomCollision);
            GeneralFunctions.GetGameplayManager().onCameraRightOverlap.AddListener(OnCameraRightCollision);
            GeneralFunctions.GetGameplayManager().onCameraLeftOverlap.AddListener(OnCameraLeftCollision);
        }
        /// <summary>
        /// Setup all scene loader call backs
        /// </summary>
        public void SetupCallbacks()
        {
            GeneralFunctions.GetGameplayManager().onSceneLoadingDone.AddListener(OnSceneLoadingDone);
        }
        /// <summary>
        /// Called after the scene transitions has finished playing
        /// </summary>
        protected virtual void OnSceneLoadingDone()
        {
            // For use in children
        }
        /// <summary>
        /// Called when an enemy overlaps the top of the camera bounds
        /// </summary>
        protected virtual void OnCameraTopCollision()
        {
            // For use in children
        }
        /// <summary>
        /// Called when an enemy overlaps the bottom of the camera bounds
        /// </summary>
        protected virtual void OnCameraBottomCollision()
        {
            // For use in children
        }
        /// <summary>
        /// Called when an enemy overlaps the left of the camera bounds
        /// </summary>
        protected virtual void OnCameraLeftCollision()
        {
            // For use in children
        }
        /// <summary>
        /// Called when an enemy overlaps the right of the camera bounds
        /// </summary>
        protected virtual void OnCameraRightCollision()
        {
            // For use in children
        }
        /// <summary>
        /// Called when the current health on health component is 0 or below by default will only disable enemy collision
        /// </summary>
        protected virtual void OnDeath()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        #region Properties
        /// <summary>
        /// Get this Gameobjects health component
        /// </summary>
        public HealthComponent MyHealthComponent { get; private set; } = null;
        /// <summary>
        /// Get the players current transform
        /// </summary>
        public Transform PlayerTransform { get; private set; } = null;
        /// <summary>
        /// Gets this Gameobjects Rigidbody
        /// </summary>
        public Rigidbody2D MyRigidBody2D { get; private set; } = null;
        /// <summary>
        /// Gets this Gameobjects ID
        /// </summary>
        public int MyID { get { return idObject.ID; } }
        /// <summary>
        /// Gets this Gameobjects movement component
        /// </summary>
        public EnemyMovement MyMovementComp { get; private set; } = null;
        /// <summary>
        /// Get this Gameobjects animator component
        /// </summary>
        public Animator MyAnimator { get; private set; } = null;
        #endregion
    }
}