using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer = null;
        private Vector3 defaultPlayerScale = Vector3.zero;
        private Vector3 deafultSpearLocation = Vector3.zero;
        private float defaultOpacity = 0f;

        private const float groundTraceDistance = 0.35f;

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<WormSegment> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        [HideInInspector]
        public UnityEvent OnSquishedPlayer;

        [HideInInspector]
        public UnityEvent OnUnSquishedPlayer;

        [HideInInspector]
        public UnityEvent DamagedPlayer;

        #region Collision Functions
        /// <summary>
        /// Set all needed references
        /// </summary>
        private void Awake()
        {
            IDObject = GetComponent<GameplayObjectID>();
            MyHealthComponent = GetComponent<HealthComponent>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            MyBoxCollider2D = GetComponent<BoxCollider2D>();
            MyCapsuleCollider2D = GetComponent<CapsuleCollider2D>();

            MyWidth = GeneralFunctions.GetSpriteWidth(GetComponent<SpriteRenderer>());

            defaultPlayerScale = GeneralFunctions.GetPlayerGameObject().transform.localScale;

            deafultSpearLocation = GeneralFunctions.GetPlayerGameObject().transform.GetChild(1).transform.localPosition;

            MyHealthComponent.ConstructHealthComponent();
            MyHealthComponent.OnDeath.AddListener(OnDeath);

            defaultOpacity = spriteRenderer.color.a;
        }
        /// <summary>
        /// Called when segment dies disables both collision and sprite renderer then invokes an OnSegmentDeath event
        /// </summary>
        private void OnDeath()
        {
            spriteRenderer.enabled = false;
            MyBoxCollider2D.enabled = false;
            MyCapsuleCollider2D.enabled = false;

            SegmentDeath.Invoke(this);
        }
        /// <summary>
        /// Check to see if segment is in the ground if not enable collision
        /// </summary>
        public void CheckCollision()
        {
            Collider2D collider2D = Physics2D.OverlapCapsule(transform.position, MyCapsuleCollider2D.size, MyCapsuleCollider2D.direction, GeneralFunctions.GetObjectEulerAngle(gameObject), WhatIsGround);

            if (collider2D)
            {
                AboveGround = false;
            }
            else
            {
                AboveGround = true;
            }
        }
        /// <summary>
        /// Does a constant trace to see if segment is touching the ground layer 
        /// </summary>
        private void Update()
        {
            if (AboveGround)
            {
                CheckForGround();

                CheckForSquish();
            }
        }
        /// <summary>
        /// Raycast towards the current side the player is on and check to see if segment is touching the ground
        /// </summary>
        private void CheckForGround()
        {
            RaycastHit2D raycastHit2D;

            if (!IsPlayerLeft)
            {
                var traceStart = transform.position;
                var traceEnd = GeneralFunctions.GetFaceingDirectionX(gameObject);

                raycastHit2D = Physics2D.Raycast(traceStart, traceEnd, groundTraceDistance, WhatIsGround);

                Debug.DrawRay(traceStart, traceEnd * groundTraceDistance, Color.green);
            }
            else
            {
                var traceStart = transform.position;
                var traceEnd = GeneralFunctions.GetFaceingDirectionX(gameObject);

                raycastHit2D = Physics2D.Raycast(traceStart, -traceEnd, groundTraceDistance, WhatIsGround);

                Debug.DrawRay(traceStart, -traceEnd * groundTraceDistance, Color.green);
            }

            if (raycastHit2D)
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
        /// <summary>
        /// If segment is rotating check to see if player has been hit if so squish the player
        /// </summary>
        private void CheckForSquish()
        {
            if (IsRotating)
            {
                Collider2D collider2D = Physics2D.OverlapBox(transform.position, MyBoxCollider2D.size, GeneralFunctions.GetObjectEulerAngle(gameObject), LayerMask.GetMask("Player"));

                if (collider2D)
                {
                    if (GeneralFunctions.IsPlayerTouchingGround())
                    {
                        if (IsGrounded)
                        {
                            DisableCollision();
                        }

                        SquishPlayer(PlayerObject);
                    }
                }
                else if (HasPlayerBeenSquished)
                {
                    if (IsGrounded)
                    {
                        DisableCollision();
                    }
                }
            }
        }
        /// <summary>
        /// When hit by player deal damage
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                if (CanDamage && IsRotating)
                {
                    GeneralFunctions.DamageTarget(collision.gameObject, DamageToApply, true, gameObject);

                    DamagedPlayer.Invoke();
                }
            }
        }
        /// <summary>
        /// Set player scale to equal the squish scale
        /// </summary>
        /// <param name="player"></param>
        public void SquishPlayer(GameObject player)
        {
            player.transform.localScale = SquishScale;

            if (!GeneralFunctions.GetGameObjectHealthComponent(player).IsCurrentlyDead)
            {
                GeneralFunctions.GetPlayerSpear().DisableSpear();

                OnSquishedPlayer.Invoke();

                StartCoroutine(UnSquishPlayer(player));
            }
        }
        /// <summary>
        /// Return player back to the default scale
        /// </summary>
        private IEnumerator UnSquishPlayer(GameObject player)
        {
            yield return new WaitForSeconds(SquishTime);

            player.transform.localScale = defaultPlayerScale;

            GeneralFunctions.GetPlayerSpear().EnableSpear();

            // Move spear back to it's default location
            GeneralFunctions.GetPlayerSpear().transform.localPosition = deafultSpearLocation;

            OnUnSquishedPlayer.Invoke();
        }
        /// <summary>
        /// Disable both MyBoxCollider2D and MyCapsuleCollider2D
        /// </summary>
        public void DisableCollision()
        {
            MyBoxCollider2D.enabled = false;
            MyCapsuleCollider2D.enabled = false;
        }
        /// <summary>
        /// Enable both MyBoxCollider2D and MyCapsuleCollider2D
        /// </summary>
        public void EnableCollision()
        {
            MyBoxCollider2D.enabled = true;
            MyCapsuleCollider2D.enabled = true;

            Color tmp = spriteRenderer.color;

            tmp.a = defaultOpacity;

            spriteRenderer.color = tmp;
        }
        /// <summary>
        /// Set the worm sprites opacity
        /// </summary>
        /// <param name="newOpacity"></param>
        public void SetOpacity(float newOpacity)
        {
            Color tmp = spriteRenderer.color;

            tmp.a = newOpacity;

            spriteRenderer.color = tmp;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Amount of damage to apply to the player
        /// </summary>
        public float DamageToApply { get; set; }
        /// <summary>
        /// Gets this Gameobjects ID
        /// </summary>
        public int MyID { get { return IDObject.ID; } }
        /// <summary>
        /// Get this Gameobjects health component
        /// </summary>
        public HealthComponent MyHealthComponent { get; private set; } = null;
        /// <summary>
        /// BoxCollider2D attached to segment
        /// </summary>
        public BoxCollider2D MyBoxCollider2D { get; private set; } = null;
        /// <summary>
        /// CapsuleCollider2D attached to segment
        /// </summary>
        public CapsuleCollider2D MyCapsuleCollider2D { get; private set; } = null;
        /// <summary>
        /// What layers are ground
        /// </summary>
        public LayerMask WhatIsGround { get; set; }
        /// <summary>
        /// Whether or not to draw debug lines
        /// </summary>
        public bool DrawDebug { get; set; }
        /// <summary>
        /// Checks to see if the current segment is above ground
        /// </summary>
        public bool AboveGround { get; private set; }
        /// <summary>
        /// Get the width of the worm segment sprite
        /// </summary>
        public float MyWidth { get; private set; }
        /// <summary>
        /// Check to see if the worm is rotating
        /// </summary>
        public bool IsRotating { get; set; }
        /// <summary>
        /// The amount of time the player is squished for
        /// </summary>
        public float SquishTime { get; set; }
        /// <summary>
        /// The new scale to apply to the player
        /// </summary>
        public Vector3 SquishScale { get; set; }
        /// <summary>
        /// Check to see if the given segment is currently able to damage the player
        /// </summary>
        public bool CanDamage { get; set; }
        /// <summary>
        /// Checks to see if this is the segment currently the topmost segment
        /// </summary>
        public bool IsTopSegment { get; set; }
        /// <summary>
        /// Check to see if the player is left of this object if false player is right
        /// </summary>
        public bool IsPlayerLeft { get; set; }
        /// <summary>
        /// Get this games objects id component
        /// </summary>
        public GameplayObjectID IDObject { get; private set; }
        /// <summary>
        /// Gets the height of the actual worm sprite
        /// </summary>
        public float SpriteWidth { get; set; }
        /// <summary>
        /// A reference to the player object in the current level
        /// </summary>
        public GameObject PlayerObject { get; set; }
        /// <summary>
        /// Checks to see if the segment is touching the ground
        /// </summary>
        public bool IsGrounded { get; private set; }
        /// <summary>
        /// Check to see if the player has been squished
        /// </summary>
        public bool HasPlayerBeenSquished { get; set; } = false;
        #endregion
    }
}