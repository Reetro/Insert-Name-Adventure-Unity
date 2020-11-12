using UnityEngine;
using EnemyCharacter.AI;

namespace EnemyCharacter
{
    [RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(GameplayObjectID))]
    [RequireComponent(typeof(Animator))]
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