using UnityEngine;
using PlayerUI;
using GameplayManagement;
using System;
using PlayerControls;
using System.Collections;

namespace PlayerCharacter.Controller
{
    [Serializable]
    public class PlayerSpear : MonoBehaviour
    {
        [Header("Spear Settings")]
        [SerializeField] private float spearDamage = 3.5f;
        [SerializeField] private float spearCooldown = 1f;
        [SerializeField] private float spearReturnDelay = 1f;
        [SerializeField] private float spearTravelDistance = 1f;
        [SerializeField] private GameObject damageToSpawn = null;

        #region Gun Components
        private PlayerController controller = null;
        private CooldownBar cooldownBar = null;
        private HealthComponent playerHealthComp = null;
        #endregion

        #region Local Varabiles
        private float spearAngle = 0f;
        private bool touchingGround = false;
        private const float traceLength = 1f;
        private GameplayManager gameplayManager = null;
        private PlayerDamage playerDamage = null;
        private Transform damageSpawn = null;
        private Controls controls = null;
        private bool canRotate = false;
        private bool isSpearOut = false;
        #endregion

        #region JoyStick Rotation
        private Vector2 stickInput;
        private float spearRotation = 0f;
        private float spearRotationX = 0f;
        private bool facingRight = true;
        #endregion

        /// <summary>
        /// Create new player controls object
        /// </summary>
        private void Awake()
        {
            controls = new Controls();

            SetupSpear();
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
        /// Make player look in the direction of the provided input and rotate player spear
        /// </summary>
        void Update()
        {
            if (canRotate)
            {
                RotateSpear();
            }

            if (!gameplayManager._IsGamepadActive)
            {
                RotatePlayerWithMouse();
            }
        }
        /// <summary>
        /// Will try to push the spear in the direction its facing
        /// </summary>
        public void StartPushSpear()
        {
            if (CanPushSpear())
            {
                isSpearOut = true;
                canRotate = false;

                var damage = Instantiate(damageToSpawn, damageSpawn);
                playerDamage = damage.GetComponent<PlayerDamage>();

                cooldownBar.StartCooldown(spearCooldown);

                StartCoroutine(PushSpear());
            }
            else
            {
                if (!GeneralFunctions.IsPlayerDead())
                {
                    // if spear was not able to be pushed check to see if there is an enemy in front of the player and damage it
                    CheckForEnemy();
                }
            }
        }
        /// <summary>
        /// Push spear forward
        /// </summary>
        private IEnumerator PushSpear()
        {
            playerDamage.ConstructBox(spearDamage);
            transform.Translate(spearTravelDistance, 0, 0);

            yield return new WaitForSeconds(spearReturnDelay);

            transform.Translate(-spearTravelDistance, 0, 0);

            Destroy(playerDamage.gameObject);
            canRotate = true;
            isSpearOut = false;
        }
        /// <summary>
        /// Fire a raycast to check for enemies around the player if player projectile failed to spawn
        /// </summary>
        private void CheckForEnemy()
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.transform.position, transform.right, traceLength, gameplayManager.whatCanBeDamaged);

            if (hit)
            {
                GeneralFunctions.DamageTarget(hit.transform.gameObject, spearDamage, true, gameObject);
            }
        }
        /// <summary>
        /// Rotate the spear either to the mouse location or to the Gamepad joystick position
        /// </summary>
        private void RotateSpear()
        {
            if (!playerHealthComp.IsCurrentlyDead)
            {
                if (!gameplayManager._IsGamepadActive)
                {
                    RotateSpearWithMouse();
                }
                else
                {
                    UpdateJoystick();

                    RotateSpearWithGamepad();
                }
            }
        }
        /// <summary>
        /// If a Gamepad is not active get mouse location and rotate spear around the player character with mouse
        /// </summary>
        private void RotateSpearWithMouse()
        {
            Vector3 mousePos = controls.Player.MousePostion.ReadValue<Vector2>();
            Vector3 spearPos = Camera.main.WorldToScreenPoint(transform.position);

            mousePos.x = mousePos.x - spearPos.x;
            mousePos.y = mousePos.y - spearPos.y;
            spearAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            if (MouseLeftOrRight())
            {
                transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -spearAngle));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, spearAngle));
            }
        }
        /// <summary>
        /// Rotate the player left or right to match the direction the spear is facing with mouse controls
        /// </summary>
        private void RotatePlayerWithMouse()
        {
            if (!playerHealthComp.IsCurrentlyDead)
            {
                if (!gameplayManager._IsGamepadActive)
                {
                    if (MouseLeftOrRight())
                    {
                        controller.transform.eulerAngles = new Vector3(0, 180f, transform.position.z);
                    }
                    else
                    {
                        controller.transform.eulerAngles = new Vector3(0, 0f, transform.position.z);
                    }
                }
            }
        }
        /// <summary>
        /// Rotate player with right Gamepad joy stick
        /// </summary>
        private void RotatePlayerWithGamepad()
        {
            facingRight = !facingRight;

            GeneralFunctions.FlipObject(controller.gameObject);
        }
        /// <summary>
        /// If a Gamepad is active get value from right stick and calculate spear rotation
        /// </summary>
        private void RotateSpearWithGamepad()
        {
            if (stickInput.sqrMagnitude > 0)
            {
                spearRotationX = stickInput.x;

                if (facingRight)
                {
                    spearRotation = stickInput.x + stickInput.y * 90;
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, spearRotation);
                }
                else
                {
                    spearRotation = stickInput.x + stickInput.y * -90;
                    gameObject.transform.rotation = Quaternion.Euler(0, 180f, -spearRotation);
                }

                if (spearRotationX < 0 && facingRight)
                {
                    RotatePlayerWithGamepad();
                }
                else if (spearRotationX > 0 && !facingRight)
                {
                    RotatePlayerWithGamepad();
                }
            }
        }
        /// <summary>
        /// Update rotation value when right joystick is moved
        /// </summary>
        /// <param name="RotationValue"></param>
        private void UpdateJoystick()
        {
            stickInput = controls.Player.Rotate.ReadValue<Vector2>();
        }
        /// <summary>
        /// Checks to see if the spear is being block by ground
        /// </summary>
        /// <returns>A bool that determines if the player can push the spear</returns>
        private bool CanPushSpear()
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                if (!isSpearOut)
                {
                    if (!cooldownBar.GetIsActive())
                    {
                        if (!touchingGround)
                        {
                            return true;
                        }
                        else
                        {
                            cooldownBar.StartCooldown(spearCooldown);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            touchingGround = true;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            touchingGround = false;
        }
        /// <summary>
        /// Look see if the mouse is on the left or right of the screen
        /// </summary>
        private bool MouseLeftOrRight()
        {
            var playerScreenPoint = Camera.main.WorldToScreenPoint(controller.transform.position);
            float mouseX = controls.Player.MousePostion.ReadValue<Vector2>().x;

            return mouseX < playerScreenPoint.x ? true : false;
        }
        /// <summary>
        /// Collects all need components for the spear to work
        /// </summary>
        private void SetupSpear()
        {
            controller = GetComponentInParent<PlayerController>();
            cooldownBar = transform.GetChild(2).transform.GetChild(0).GetComponent<CooldownBar>();
            playerHealthComp = GetComponentInParent<HealthComponent>();
            gameplayManager = FindObjectOfType<GameplayManager>();
            damageSpawn = transform.GetChild(3).transform.GetComponent<Transform>();

            canRotate = true;
        }
    }
}