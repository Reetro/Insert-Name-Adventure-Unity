using System.Collections;
using System.Collections.Generic;
using GeneralScripts;
using GeneralScripts.GeneralComponents;
using GeneralScripts.UI;
using PlayerControls;
using PlayerScripts.PlayerCombat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PlayerScripts.PlayerControls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSpear currentSpear = null;

        #region Player Controls
        private Controls controls;
        private float horizontalMove;
        private bool jump;
        private bool jumpHeldDown;
        private bool swingSpear;
        #endregion

        #region Player Components
        private Animator myAnimator;
        private PlayerMovement playerMovement;
        private HealthComponent healthComponent;
        private PlayerUIManager uiManager;
        private Rigidbody2D myRigidBody2D;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int Speed = Animator.StringToHash("Speed");

        #endregion

        #region Properties
        /// <summary>
        /// Reference to player state currently in the world
        /// </summary>
        public PlayerState MyPlayerState { get; set; }
        /// <summary>
        /// Checks to see if the player is stunned
        /// </summary>
        public bool ControlDisabled { get; private set; }
        /// <summary>
        /// Reference to the players box collider
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public BoxCollider2D MyBoxCollider { get; private set; }
        /// <summary>
        /// List of all Actionbar input actions
        /// </summary>
        public List<InputAction> ActionBarInputs { get; private set; }
        #endregion

        #region Setup Functions
        /// <summary>
        /// Called when player enters a new level
        /// </summary>
        public void OnSceneCreated()
        {
            GetActionbarInputs();
        }
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

            controls.Player.PauseGame.started += OnPausePressed;

            controls.Player.DeleteSavedGame.started += OnDeletePressed;

            transform.GetChild(0).GetComponent<PlayerLegs>().onLandEvent.AddListener(OnLanding);

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

            healthComponent.onDeath.AddListener(OnDeath);
            healthComponent.onTakeAnyDamage.AddListener(OnTakeAnyDamage);

            MyBoxCollider = GetComponent<BoxCollider2D>();
            myRigidBody2D = GetComponent<Rigidbody2D>();

            Move();
        }
        /// <summary>
        /// Find all Actionbar input actions in player controls and add them to the list
        /// </summary>
        private void GetActionbarInputs()
        {
            ActionBarInputs = new List<InputAction>();

            var playerActionMap = GetComponent<PlayerInput>().currentActionMap.actions;

            foreach (var currentItem in playerActionMap)
            {
                if (currentItem.name.Contains("Actionbar Slot"))
                {
                    ActionBarInputs.Add(currentItem);
                }
            }
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
                myAnimator.SetBool(IsJumping, true);
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
        /// <summary>
        /// Pause Game when pause button is pressed
        /// </summary>
        /// <param name="context"></param>
        private void OnPausePressed(InputAction.CallbackContext context)
        {
            if (!GeneralFunctions.GetGameplayManager().IsGamePaused)
            {
                GeneralFunctions.PauseGame();
            }
            else
            {
                GeneralFunctions.ResumeGame();
            }
        }
        #endregion

        #region Movement Functions
        /// <summary>
        /// Move the player character
        /// </summary>
        private void Move()
        {
            if (!GeneralFunctions.GetGameplayManager().IsGamePaused)
            {
                if (!healthComponent.IsCurrentlyDead)
                {
                    horizontalMove = controls.Player.Movement.ReadValue<Vector2>().x;

                    switch (transform.localEulerAngles.y >= 180)
                    {
                        case true:
                            myAnimator.SetFloat(Speed, -horizontalMove);
                            break;
                        case false:
                            myAnimator.SetFloat(Speed, horizontalMove);
                            break;
                    }

                    myAnimator.SetBool(Idle, horizontalMove == 0);
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
            myAnimator.SetBool(IsJumping, false);

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
        private void OnDeath()
        {
            uiManager.ShowDeathUI();

            myAnimator.SetBool(Idle, true);

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
                PlayerState.UpdatePlayerStateHp(healthComponent.CurrentHealth, healthComponent.MaxHealth);
            }
            else
            {
                MyPlayerState = FindObjectOfType<PlayerState>();

                PlayerState.UpdatePlayerStateHp(healthComponent.CurrentHealth, healthComponent.MaxHealth);
            }
        }
        #endregion
    }
}