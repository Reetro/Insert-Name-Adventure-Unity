using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using PlayerUI;
using PlayerCharacter.GameSaving;

namespace PlayerCharacter.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerGun currentGun = null;

        private PlayerUIManager uiManager = null;
        private float horizontalMove = 0f;
        private bool jump = false;
        private Animator myAnimator = null;
        private PlayerMovement playerMovement = null;
        private HealthComponent healthComponent = null;

        /// <summary>
        /// Reference to player currently in the world
        /// </summary>
        public PlayerState MyPlayerState { get; set; } = null;

        private void Start()
        {
            myAnimator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            uiManager = FindObjectOfType<PlayerUIManager>();
            healthComponent = GetComponent<HealthComponent>();

            healthComponent.OnDeath.AddListener(OnDeath);
            healthComponent.onTakeAnyDamage.AddListener(OnTakeAnyDamage);
        }

        private void Update()
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                horizontalMove = CrossPlatformInputManager.GetAxisRaw("Horizontal");

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

                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    jump = true;
                    myAnimator.SetBool("IsJumping", true);
                }

                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                {
                    currentGun.FireGun();
                }

                if (CrossPlatformInputManager.GetAxis("Fire1") > 0)
                {
                    currentGun.FireGun();
                }
            }
        }

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

            if (CrossPlatformInputManager.GetButton("Jump"))
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