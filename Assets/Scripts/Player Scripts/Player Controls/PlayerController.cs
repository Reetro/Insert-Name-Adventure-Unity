using UnityEngine;
using PlayerUI;
using PlayerCharacter.GameSaving;
using PlayerControls;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;

namespace PlayerCharacter.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSpear currentSpear = null;

        #region Player Controls
        public Controls controls = null;
        private float horizontalMove = 0f;
        private bool jump = false;
        private bool jumpHeldDown = false;
        private bool swingSpear = false;
        #endregion

        #region Player Components
        private Animator myAnimator = null;
        private PlayerMovement playerMovement = null;
        private HealthComponent healthComponent = null;
        private PlayerUIManager uiManager = null;
        private Rigidbody2D myRigidBody2D = null;
        #endregion

        #region Properties
        /// <summary>
        /// Reference to player state currently in the world
        /// </summary>
        public PlayerState MyPlayerState { get; set; } = null;
        /// <summary>
        /// Checks to see if the player is stunned
        /// </summary>
        public bool ControlDisabled { get; private set; } = false;
        /// <summary>
        /// Reference to the players box collider
        /// </summary>
        public BoxCollider2D MyBoxCollider { get; private set; } = null;
        #endregion

        #region Setup Functions
        /// <summary>
        /// Create new player controls object
        /// </summary>
        private void Awake()
        {
            controls = new Controls();

            controls.Player.Jump.started += OnJumpHeld;
            controls.Player.Jump.canceled += OnJumpReleased;

            controls.Player.Fire.started += OnFirePressed;
            controls.Player.Fire.canceled += OnFireReleased;

            controls.Player.SaveGame.started += OnSavePressed;
            controls.Player.LoadGame.started += OnLoadPressed;

            controls.Player.DeleteSavedGame.started += OnDeletePressed;

            transform.GetChild(0).GetComponent<PlayerLegs>().OnLandEvent.AddListener(OnLanding);

            ControlDisabled = false;
        }
        /// <summary>
        /// Enable player input
        /// </summary>
        private void OnEnable()
        {
            controls.Player.Enable();
        }
        /// <summary>
        /// Disable player input
        /// </summary>
        private void OnDisable()
        {
            controls.Player.Disable();
        }
        /// <summary>
        /// Get a reference to all player components
        /// </summary>
        private void Start()
        {
            myAnimator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            uiManager = FindObjectOfType<PlayerUIManager>();
            healthComponent = GetComponent<HealthComponent>();

            healthComponent.OnDeath.AddListener(OnDeath);
            healthComponent.OnTakeAnyDamage.AddListener(OnTakeAnyDamage);

            MyBoxCollider = GetComponent<BoxCollider2D>();
            myRigidBody2D = GetComponent<Rigidbody2D>();

            Move();
        }
        #endregion

        #region Debug Functions
        /// <summary>
        /// Save all player game data
        /// </summary>
        /// <param name="context"></param>
        private void OnSavePressed(InputAction.CallbackContext context)
        {
            GeneralFunctions.SaveGameToSlot(1);
        }
        /// <summary>
        /// Load saved player data
        /// </summary>
        /// <param name="context"></param>
        private void OnLoadPressed(InputAction.CallbackContext context)
        {
            GeneralFunctions.LoadGameFromSlot(1);
        }
        /// <summary>
        /// Delete the current saved game
        /// </summary>
        /// <param name="context"></param>
        private void OnDeletePressed(InputAction.CallbackContext context)
        {
            GeneralFunctions.DeleteGameInSlot(1);
        }
        #endregion

        #region Input Actions
        /// <summary>
        /// Called when player presses the fire key
        /// </summary>
        /// <param name="context"></param>
        private void OnFirePressed(InputAction.CallbackContext context)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                swingSpear = true;
            }
        }
        /// <summary>
        /// Called when fire button is released
        /// </summary>
        /// <param name="context"></param>
        private void OnFireReleased(InputAction.CallbackContext context)
        {
            swingSpear = false;
        }
        /// <summary>
        /// Called when jump button is pressed
        /// </summary>
        /// <param name="context"></param>
        private void OnJumpHeld(InputAction.CallbackContext context)
        {
            jumpHeldDown = true;
        }
        /// <summary>
        /// Called when jump button is released
        /// </summary>
        /// <param name="context"></param>
        private void OnJumpReleased(InputAction.CallbackContext context)
        {
            jumpHeldDown = false;
        }
        /// <summary>
        /// Event to tell the player to start jumping
        /// </summary>
        public void Jump()
        {
            if (!healthComponent.IsCurrentlyDead)
            {
                jump = true;
                myAnimator.SetBool("IsJumping", true);
            }
        }
        /// <summary>
        /// Fires the player's current gun
        /// </summary>
        private void StartSpearPush()
        {
            if (!healthComponent.IsCurrentlyDead)
            {
                if (swingSpear)
                {
                    currentSpear.StartSpearPush();
                }
            }
        }
        #endregion

        #region Movement Functions
        /// <summary>
        /// Move the player character
        /// </summary>
        private void Move()
        {
            if (!healthComponent.IsCurrentlyDead)
            {
                horizontalMove = controls.Player.Movement.ReadValue<Vector2>().x;

                switch (transform.localEulerAngles.y >= 180)
                {
                    case true:
                        myAnimator.SetFloat("Speed", -horizontalMove);
                        break;
                    case false:
                        myAnimator.SetFloat("Speed", horizontalMove);
                        break;
                }

                if (horizontalMove == 0)
                {
                    myAnimator.SetBool("Idle", true);
                }
                else
                {
                    myAnimator.SetBool("Idle", false);
                }
            }
        }
        /// <summary>
        /// Check for player movement input and gun input
        /// </summary>
        private void Update()
        {
            if (!ControlDisabled)
            {
                Move();
                StartSpearPush();
            }
        }
        /// <summary>
        /// Check for jump input if true set movement state to jumping
        /// </summary>
        private void FixedUpdate()
        {
            if (!ControlDisabled)
            {
                playerMovement.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
                jump = false;
            }
        }
        /// <summary>
        /// Checks to see if player is moving left or right
        /// </summary>
        /// <returns>Returns true if moving left</returns>
        public bool IsMovingRightOrLeft(out bool isIdle)
        {
            if (horizontalMove == 0)
            {
                isIdle = true;

                return false;
            }
            else if (horizontalMove < 0)
            {
                isIdle = false;

                return true;
            }
            else
            {
                isIdle = false;

                return false;
            }
        }
        #endregion

        #region Player Events
        /// <summary>
        /// Called when player touches ground
        /// </summary>
        public void OnLanding()
        {
            myAnimator.SetBool("IsJumping", false);

            if (jumpHeldDown)
            {
                jump = true;
            }
        }
        /// <summary>
        /// Disables player input for the given time
        /// </summary>
        /// <param name="stunTime"></param>
        public void ApplyStun(float stunTime)
        {
            ControlDisabled = true;

            StartCoroutine(StunTimer(stunTime));
        }
        /// <summary>
        /// The actual stun timer
        /// </summary>
        /// <param name="time"></param>
        private IEnumerator StunTimer(float time)
        {
            yield return new WaitForSeconds(time);

            ControlDisabled = false;
        }
        /// <summary>
        /// Completely disable player control
        /// </summary>
        public void DisableControl(bool makeKinematic)
        {
            ControlDisabled = true;

            if (makeKinematic)
            {
                myRigidBody2D.isKinematic = true;
            }
        }
        /// <summary>
        /// Enable player control
        /// </summary>
        public void EnableControl()
        {
            ControlDisabled = false;

            myRigidBody2D.isKinematic = false;
        }
        /// <summary>
        /// Called when player dies will stop all player movement
        /// </summary>
        public void OnDeath()
        {
            uiManager.ShowDeathUI();

            myAnimator.SetBool("Idle", true);

            playerMovement.StopMovement();
        }
        /// <summary>
        /// Completely disable player collision
        /// </summary>
        public void DisableCollision()
        {
            MyBoxCollider.enabled = false;

            myRigidBody2D.isKinematic = true;
        }
        /// <summary>
        /// Enable collision on the player
        /// </summary>
        public void EnableCollision()
        {
            MyBoxCollider.enabled = true;

            myRigidBody2D.isKinematic = false;
        }
        /// <summary>
        /// Update PlayerState health when ever player takes damage
        /// </summary>
        /// <param name="damage"></param>
        private void OnTakeAnyDamage(float damage)
        {
            if (MyPlayerState)
            {
                MyPlayerState.UpdatePlayerStateHP(healthComponent.CurrentHealth, healthComponent.MaxHealth);
            }
            else
            {
                MyPlayerState = FindObjectOfType<PlayerState>();

                MyPlayerState.UpdatePlayerStateHP(healthComponent.CurrentHealth, healthComponent.MaxHealth);
            }
        }
        #endregion
    }
}