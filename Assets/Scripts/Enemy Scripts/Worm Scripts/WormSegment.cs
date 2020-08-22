using UnityEngine;
using UnityEngine.Events;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        private GameplayObjectID idObject = null;
        private SpriteRenderer spriteRenderer = null;
        private BoxCollider2D boxCollider2D = null;

        [HideInInspector]
        public UnityEvent OnSegmentDeath;

        /// <summary>
        /// Set all needed references
        /// </summary>
        private void Awake()
        {
            MyRigidbody2D = GetComponent<Rigidbody2D>();
            idObject = GetComponent<GameplayObjectID>();
            MyHealthComponent = GetComponent<HealthComponent>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();

            idObject.ConstructID();
            MyHealthComponent.ConstructHealthComponent();
            MyHealthComponent.OnDeath.AddListener(OnDeath);
        }
        /// <summary>
        /// Called when segment dies
        /// </summary>
        private void OnDeath()
        {
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = false;

            OnSegmentDeath.Invoke();
        }
        /// <summary>
        /// When hit by player deal damage
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //GeneralFunctions.DamageTarget(collision.gameObject, DamageToApply, true, gameObject);
            }
        }
        /// <summary>
        /// Amount of damage to apply to the player
        /// </summary>
        public float DamageToApply { get; set; }
        /// <summary>
        /// Rigidbody2D attached to segment
        /// </summary>
        public Rigidbody2D MyRigidbody2D { get; private set; } = null;
        /// <summary>
        /// Gets this Gameobjects ID
        /// </summary>
        public int MyID { get { return idObject.ID; } }
        /// <summary>
        /// Get this Gameobjects health component
        /// </summary>
        public HealthComponent MyHealthComponent { get; private set; } = null;
    }
}