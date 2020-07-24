using UnityEngine;
using EnemyCharacter.AI;

namespace EnemyCharacter
{
    [RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(GameplayObjectID))]
    [RequireComponent(typeof(EnemyMovement), typeof(Animator))]
    public class EnemyBase : MonoBehaviour
    {
        private GameplayObjectID idObject = null;

        protected virtual void Awake()
        {
            PlayerTransform = GeneralFunctions.GetPlayerGameObject().transform;
            MyHealthComponent = GetComponent<HealthComponent>();
            MyRigidBody2D = GetComponent<Rigidbody2D>();
            idObject = GetComponent<GameplayObjectID>();
            MovementComp = GetComponent<EnemyMovement>();
            MyAnimator = GetComponent<Animator>();

            idObject.ConstructID();
            MyHealthComponent.ConstructHealthComponent();
            MyHealthComponent.OnDeath.AddListener(OnDeath);
        }
        /// <summary>
        /// Fire a raycast from the current AI the player and checks to if there is ground between the player and and the enemy
        /// </summary>
        /// <returns>A bool that determines enemy sight</returns>
        protected bool IsPlayerVisiable(LayerMask layerMask)
        {
            var playerDirection = GeneralFunctions.GetDistanceBetweenVectors(PlayerTransform.position, transform.position);

            Debug.DrawRay(transform.position, playerDirection * 100f, Color.red);

            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, playerDirection, 10f, layerMask);

            if (hit2D)
            {
                if (hit2D.transform.gameObject.CompareTag("Player"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
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
        public int IdGameObject { get { return IdGameObject; } }
        /// <summary>
        /// Gets this Gameobjects movement component
        /// </summary>
        public EnemyMovement MovementComp { get; private set; } = null;
        /// <summary>
        /// Get this Gameobjects animator component
        /// </summary>
        public Animator MyAnimator { get; private set; } = null;
        /// <summary>
        /// Make this Gameobject look towards the player
        /// </summary>
        public void LookAtPlayer()
        {
            MovementComp.LookAtTarget(PlayerTransform);
        }
        /// <summary>
        /// Called when the current health on health component is 0 or below by default will only disable enemy collision
        /// </summary>
        public virtual void OnDeath()
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}