using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using GameplayManagement.Assets;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer = null;
        private Vector3 defaultPlayerScale = Vector3.zero;
        private Vector3 deafultSpearLocation = Vector3.zero;
        private float defaultOpacity = 0f;
        private bool isPlayerAttachedAndSquishable = false;
        private Quaternion playerAttachedRotation;
        private GameObject playerObject = null;
        private Rigidbody2D playerRigidBody = null;

        private const float groundTraceDistance = 0.35f;
        private const float playerSquishCheckDistance = 0.5f;

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

        [HideInInspector]
        public bool isPlayerWalkingOnWorm = false;

        [HideInInspector]
        public bool keepPlayerRotation = false;

        [HideInInspector]
        public bool isPlayerAttached = false;

        [SerializeField] private BoxCollider2D leftCollision = null;
        [SerializeField] private BoxCollider2D rightCollision = null;

        #region Collision Functions
        /// <summary>
        /// Called right after the SceneCreator has setup the Player Gameobject
        /// </summary>
        public void OnSceneCreated()
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

                if (keepPlayerRotation && isPlayerAttached)
                {
                    if (playerObject)
                    {
                        playerObject.transform.rotation = playerAttachedRotation;
                    }
                    else
                    {
                        Debug.LogError(name + " is unable to keep player rotation player object is not valid");
                    }
                }

                if (!isPlayerWalkingOnWorm)
                {
                    CheckDisable();

                    CheckAttachedSquish();
                }
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
        /// Disables collision that is facing the player
        /// </summary>
        public void UpdateCollision()
        {
            if (IsPlayerLeft)
            {
                leftCollision.enabled = false;
                rightCollision.enabled = true;
            }
            else
            {
                rightCollision.enabled = false;
                leftCollision.enabled = true;
            }
        }
        /// <summary>
        /// Keep player's attached rotation and look for ground under player
        /// </summary>
        private void CheckAttachedSquish()
        {
            if (isPlayerAttachedAndSquishable && !isPlayerWalkingOnWorm)
            {
                if (playerObject)
                {
                    var traceStart = playerObject.transform.position;
                    var traceEnd = -GeneralFunctions.GetFaceingDirectionY(playerObject);

                    RaycastHit2D raycastHit2D = Physics2D.Raycast(traceStart, traceEnd, playerSquishCheckDistance, LayerMask.GetMask("Ground"));

                    if (raycastHit2D)
                    {
                        DisableCollision();

                        DeattachPlayer();

                        SquishPlayer(playerObject);
                    }

                    Debug.DrawRay(traceStart, traceEnd * playerSquishCheckDistance, Color.green);
                }
            }
        }
        /// <summary>
        /// Check to see if hit player and if the player is squished if so disable collision
        /// </summary>
        private void CheckDisable()
        {
            if (IsRotatingDown)
            {
                Collider2D collider2D = Physics2D.OverlapBox(transform.position, MyBoxCollider2D.size, GeneralFunctions.GetObjectEulerAngle(gameObject), LayerMask.GetMask("Player"));

                if (HasPlayerBeenSquished && !collider2D && !isPlayerWalkingOnWorm)
                {
                    if (IsGrounded)
                    {
                        DisableCollision();
                    }
                }
            }
        }
        /// <summary>
        /// If segment is rotating check to see if player has been hit if so squish the player
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                if (!isPlayerWalkingOnWorm && !IsRotatingUp)
                {
                    if (GeneralFunctions.IsPlayerTouchingGround() && !IsIdle)
                    {
                        if (IsGrounded)
                        {
                            DisableCollision();
                        }

                        SquishPlayer(PlayerObject);
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
                if (IsRotatingDown && !HasPlayerBeenSquished && !IsRotatingUp && !isPlayerWalkingOnWorm)
                {
                    GameAssets.PlayerGameController.DisableControl(false);

                    AttachPlayer(collision, true);
                }
                else if (isPlayerWalkingOnWorm && IsRotatingUp && !IsRotatingDown && !IsIdle)
                {
                    GameAssets.PlayerGameController.DisableControl(true);

                    AttachPlayer(collision, true);
                }
            }
        }
        /// <summary>
        /// When player is walking on worm set isPlayerWalkingOnWorm to true
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnPlayer(collision.gameObject))
            {
                isPlayerWalkingOnWorm = true;
            }
        }
        /// <summary>
        /// When player is no longer walking on worm set isPlayerWalkingOnWorm to false
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnPlayer(collision.gameObject))
            {
                isPlayerWalkingOnWorm = false;
            }
        }
        /// <summary>
        /// Set player scale to equal the squish scale
        /// </summary>
        /// <param name="player"></param>
        public void SquishPlayer(GameObject player)
        {
            if (player)
            {
                player.transform.localScale = SquishScale;

                DamagedPlayer.Invoke();

                if (!GeneralFunctions.GetGameObjectHealthComponent(player).IsCurrentlyDead)
                {
                    GeneralFunctions.GetPlayerSpear().DisableSpear();

                    OnSquishedPlayer.Invoke();

                    StartCoroutine(UnSquishPlayer(player));
                }
            }
        }
        /// <summary>
        /// Return player back to the default scale
        /// </summary>
        private IEnumerator UnSquishPlayer(GameObject player)
        {
            if (player)
            {
                yield return new WaitForSeconds(SquishTime);

                player.transform.localScale = defaultPlayerScale;

                GeneralFunctions.GetPlayerSpear().EnableSpear();

                // Move spear back to it's default location
                GeneralFunctions.GetPlayerSpear().transform.localPosition = deafultSpearLocation;

                OnUnSquishedPlayer.Invoke();
            }
            else
            {
                Debug.LogError("Failed to unsquish player player was not valid");

                yield break;
            }
        }
        /// <summary>
        /// Disables MyBoxCollider2D, MyCapsuleCollider2D, and Trigger Boxes
        /// </summary>
        public void DisableCollision()
        {
            if (!isPlayerWalkingOnWorm)
            {
                foreach (WormSegment wormSegment in AllSegments)
                {
                    if (wormSegment)
                    {
                        if (!wormSegment.MyHealthComponent.IsCurrentlyDead)
                        {
                            wormSegment.MyBoxCollider2D.enabled = false;
                            wormSegment.MyCapsuleCollider2D.enabled = false;

                            wormSegment.rightCollision.enabled = false;
                            wormSegment.leftCollision.enabled = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Enables MyBoxCollider2D, MyCapsuleCollider2D, and Trigger Boxes then resets segment opacity
        /// </summary>
        public void EnableCollision()
        {
            foreach (WormSegment wormSegment in AllSegments)
            {
                if (wormSegment)
                {
                    if (!wormSegment.MyHealthComponent.IsCurrentlyDead)
                    {
                        wormSegment.MyBoxCollider2D.enabled = true;
                        wormSegment.MyCapsuleCollider2D.enabled = true;

                        wormSegment.UpdateCollision();
                        wormSegment.SetOpacity(wormSegment.defaultOpacity);
                    }
                }
            }
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
        /// <summary>
        /// Called when the segment is almost straight up
        /// </summary>
        public void AtLaunchPoint()
        {
            if (isPlayerAttached)
            {
                DeattachPlayer();

                playerRigidBody.AddExplosionForce(KnockbackForce, transform.position, 0);
            }
        }
        #endregion

        #region Attachment Code
        /// <summary>
        /// Attach player to the worm segment
        /// </summary>
        /// <param name="playerCollision"></param>
        /// <param name="checkForSquish"></param>
        public void AttachPlayer(Collision2D playerCollision, bool checkForSquish)
        {
            foreach (WormSegment segment in AllSegments)
            {
                segment.isPlayerAttached = false;
                segment.keepPlayerRotation = false;
            }

            playerObject = playerCollision.gameObject;

            playerAttachedRotation = playerCollision.transform.rotation;

            playerRigidBody = playerObject.GetComponent<Rigidbody2D>();

            GeneralFunctions.AttachObjectToTransfrom(transform, playerCollision.gameObject);

            keepPlayerRotation = true;
            isPlayerAttached = true;

            isPlayerAttachedAndSquishable = checkForSquish;
        }
        /// <summary>
        /// Attach player to the worm segment
        /// </summary>
        /// <param name="playerCollision"></param>
        /// <param name="checkForSquish"></param>
        public void AttachPlayer(GameObject playerObject, bool checkForSquish)
        {
            foreach (WormSegment segment in AllSegments)
            {
                segment.isPlayerAttached = false;
                segment.keepPlayerRotation = false;
            }

            this.playerObject = playerObject;

            playerAttachedRotation = playerObject.transform.rotation;

            playerRigidBody = playerObject.GetComponent<Rigidbody2D>();

            GeneralFunctions.AttachObjectToTransfrom(transform, playerObject.gameObject);

            keepPlayerRotation = true;
            isPlayerAttached = true;

            isPlayerAttachedAndSquishable = checkForSquish;
        }
        /// <summary>
        /// Deattach player from worm segment
        /// </summary>
        public void DeattachPlayer()
        {
            if (playerObject)
            {
                isPlayerAttachedAndSquishable = false;
                isPlayerAttached = false;

                keepPlayerRotation = false;

                GeneralFunctions.DetachFromParent(playerObject);

                foreach (WormSegment segment in AllSegments)
                {
                    segment.PlayerObject = null;
                }

                GameAssets.PlayerGameController.EnableControl();
            }
            else
            {
                Debug.LogError("Failed to Deattach player from " + name + " player object was not valid");
            }
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
        /// Check to see if the worm is rotating down towards the ground
        /// </summary>
        public bool IsRotatingDown { get; set; }
        /// <summary>
        /// Check to see if the worm is rotating off the ground
        /// </summary>
        public bool IsRotatingUp { get; set; }
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
        /// <summary>
        /// A list of all worm segments
        /// </summary>
        public WormSegment[] AllSegments { get; set; }
        /// <summary>
        /// Check to see if the worm is not moving
        /// </summary>
        public bool IsIdle { get; set; } = true;
        /// <summary>
        /// How much to knock back the player by when is detaching
        /// </summary>
        public float KnockbackForce { get; set; } = 10000f;
        /// <summary>
        /// Location of this segment in the worm segment array
        /// </summary>
        public int Index { get; set; }
        #endregion
    }
}