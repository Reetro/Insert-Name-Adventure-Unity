using UnityEngine;
using PlayerUI;
using PlayerCharacter.GameSaving;
using PlayerControls;
using UnityEngine.InputSystem;
using GameplayManagement;

namespace PlayerCharacter.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerGun currentGun = null;

        #region Player Controls
        public Controls controls = null;
        private float horizontalMove = 0f;
        private bool jump = false;
        private bool jumpHeldDown = false;
        private bool fireGun = false;
        #endregion

        #region Player Components
        private Animator myAnimator = null;
        private PlayerMovement playerMovement = null;
        private HealthComponent healthComponent = null;
        private PlayerUIManager uiManager = null;
        #endregion

        /// <summary>
        /// Reference to player currently in the world
        /// </summary>
        public PlayerState MyPlayerState { get; set; } = null;

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
            healthComponent.onTakeAnyDamage.AddListener(OnTakeAnyDamage);

            Move();
        }
        /// <summary>
        /// Called when player presses the fire key
        /// </summary>
        /// <param name="context"></param>
        private void OnFirePressed(InputAction.CallbackContext context)
        {
            fireGun = true;
        }
        /// <summary>
        /// Called when fire button is released
        /// </summary>
        /// <param name="context"></param>
        private void OnFireReleased(InputAction.CallbackContext context)
        {
            fireGun = false;
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
            jump = true;
            myAnimator.SetBool("IsJumping", true);
        }
        /// <summary>
        /// Move the player character
        /// </summary>
        private void Move()
        {
            if (!GeneralFunctions.IsPlayerDead())
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
        /// Fires the player's current gun
        /// </summary>
        private void FireGun()
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                if (fireGun)
                {
                    currentGun.FireGun();
                }
            }
        }
        /// <summary>
        /// Check for player movement input and gun input
        /// </summary>
        private void Update()
        {
            Move();
            FireGun();
        }
        /// <summary>
        /// Check for jump input if true set movement state to jumping
        /// </summary>
        private void FixedUpdate()
        {
            playerMovement.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
            jump = false;
        }
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
        /// Called when player dies will stop all player movement
        /// </summary>
        public void OnDeath()
        {
            uiManager.ShowDeathUI();

            myAnimator.SetBool("Idle", true);

            playerMovement.StopMovement();
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
    }
}