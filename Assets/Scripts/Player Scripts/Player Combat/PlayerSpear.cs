using UnityEngine;
using PlayerUI;
using GameplayManagement;
using System;
using PlayerControls;
using System.Collections;
using System.Collections.Generic;
using EnemyCharacter.SceneObject;

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
        [SerializeField] private Vector2 spearCastSize = new Vector2(1f, 0.1f);
        [Space]
        [Header("Layer Settings")]
        public LayerMask whatIsGround;  // A mask determining what is ground to the spear

        private List<Collider2D> colliders = new List<Collider2D>();

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
        private Controls controls = null;
        private bool canRotate = false;
        private bool isSpearOut = false;
        private GameObject playerObject = null;
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
        public void StartSpearPush()
        {
            bool nextToWall;
            if (CanPushSpear(out nextToWall))
            {
                isSpearOut = true;
                canRotate = false;

                StartCoroutine(PushSpear());
            }
            else if (!GeneralFunctions.IsPlayerDead() && !cooldownBar.GetIsActive())
            {
                cooldownBar.StartCooldown(spearCooldown);

                if (nextToWall)
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
            cooldownBar.StartCooldown(spearCooldown);
            transform.Translate(spearTravelDistance, 0, 0);
            CheckForHit();

            yield return new WaitForSeconds(spearReturnDelay);

            transform.Translate(-spearTravelDistance, 0, 0);

            canRotate = true;
            isSpearOut = false;
        }
        /// <summary>
        /// Fire a raycast to check for enemies around the player if player damage failed to spawn
        /// </summary>
        private void CheckForEnemy()
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.transform.position, transform.right, traceLength, gameplayManager.whatCanBeDamaged);

            if (hit)
            {
                if (GeneralFunctions.GetGameObjectHealthComponent(hit.transform))
                {
                    GeneralFunctions.DamageTarget(hit.transform.gameObject, spearDamage, true, gameObject);
                }
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
        private bool CanPushSpear(out bool nextToWall)
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                if (!isSpearOut)
                {
                    if (!cooldownBar.GetIsActive())
                    {
                        if (!touchingGround)
                        {
                            nextToWall = false;
                            return true;
                        }
                        else
                        {
                            var hit = GeneralFunctions.TraceFromEyes(playerObject);

                            if (hit)
                            {
                                nextToWall = true;
                            }
                            else
                            {
                                nextToWall = false;
                            }

                            return false;
                        }
                    }
                    else
                    {
                        nextToWall = false;
                        return false;
                    }
                }
                else
                {
                    nextToWall = false;
                    return false;
                }
            }
            else
            {
                nextToWall = false;
                return false;
            }
        }
        /// <summary>
        /// Check to see if spear is touching the ground
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(whatIsGround, collision.gameObject))
            {
                touchingGround = true;
            }
        }
        /// <summary>
        /// Check to see if spear is no longer touching the ground
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(whatIsGround, collision.gameObject))
            {
                touchingGround = false;
            }
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
            playerObject = GeneralFunctions.GetPlayerGameObject();

            canRotate = true;
        }
        /// <summary>
        /// Look to see if spear hit objects that can be damaged
        /// </summary>
        private void CheckForHit()
        {
            Collider2D[] colliders2D = Physics2D.OverlapBoxAll(transform.position, spearCastSize, spearAngle, gameplayManager.whatCanBeDamaged);

            foreach (Collider2D collider2 in colliders2D)
            {
                if (collider2)
                {
                    if (!collider2.gameObject.CompareTag("Player"))
                    {
                        colliders.Add(collider2);
                    }
                }
            }

            if (colliders.Count > 0)
            {
                DamageAllObjects();
            }
        }
        /// <summary>
        /// Damage all overlapped Gameobjects
        /// </summary>
        private void DamageAllObjects()
        {
           foreach (Collider2D collider2 in colliders)
           {
                if (collider2)
                {
                    if (!collider2.gameObject.CompareTag("Player"))
                    {
                        var leechEggRipe = collider2.gameObject.GetComponent<LeechEggRipe>();
                        var leechEggCold = collider2.gameObject.GetComponent<LeechEggCold>();

                        if (leechEggRipe)
                        {
                            leechEggRipe.SpawnLeech();
                        }
                        else if (leechEggCold)
                        {
                            leechEggCold.SpawnLeech();
                        }
                        else if (!leechEggRipe && !leechEggCold)
                        {
                            GeneralFunctions.DamageTarget(collider2.gameObject, spearDamage, true, gameObject);
                        }
                    }
                }
           }

            colliders.Clear();
        }
    }
}