using UnityEngine;
using PlayerUI;
using GameplayManagement;
using System;
using PlayerControls;
using System.Collections;
using System.Collections.Generic;
using EnemyCharacter.SceneObject;
using EnemyCharacter.AI;

namespace PlayerCharacter.Controller
{
    [Serializable]
    public class PlayerSpear : MonoBehaviour
    {
        [Header("Spear Settings")]
        [Tooltip("How much damage the spear deals")]
        [SerializeField] private float spearDamage = 3.5f;
        [Tooltip("Spear cooldown length")]
        [SerializeField] private float spearCooldown = 1f;
        [Tooltip("How long till the spear returns to it's starting postilion")]
        [SerializeField] private float spearReturnDelay = 1f;
        [Tooltip("How far the spear gets pushed out")]
        [SerializeField] private float spearTravelDistance = 1f;

        [Space]

        [Header("Layer Settings")]
        [Tooltip("A mask determining what is ground to the spear")]
        public LayerMask whatIsGround;

        #region Properties
        /// <summary>
        /// A list off all colliders to damage
        /// </summary>
        private List<Collider2D> collidersToDamage = new List<Collider2D>();
        /// <summary>
        /// Looks to see if the spear is touching the ground
        /// </summary>
        public bool TouchingGround { get; set; }
        /// <summary>
        /// Gets the layers the spear can collide with 
        /// </summary>
        public LayerMask WhatIsGround { get { return whatIsGround; } }
        #endregion

        #region Spear Components
        private PlayerController controller = null;
        private CooldownBar cooldownBar = null;
        private HealthComponent playerHealthComp = null;
        #endregion

        #region Local Varabiles
        private float spearAngle = 0f;
        private const float traceLength = 1f;
        private GameplayManager gameplayManager = null;
        private Controls controls = null;
        private bool canRotate = false;
        private bool isSpearOut = false;
        private GameObject playerObject = null;
        private PolygonCollider2D meleeCollision = null;
        #endregion

        #region JoyStick Rotation
        private Vector2 stickInput;
        private float spearRotation = 0f;
        private float spearRotationX = 0f;
        private bool facingRight = true;
        #endregion

        #region Setup Functions
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
            if (!controller.IsPlayerStuned)
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
            meleeCollision = GetComponent<PolygonCollider2D>();

            meleeCollision.enabled = false;

            canRotate = true;
        }
        /// <summary>
        /// Update rotation value when right joystick is moved
        /// </summary>
        /// <param name="RotationValue"></param>
        private void UpdateJoystick()
        {
            stickInput = controls.Player.Rotate.ReadValue<Vector2>();
        }
        #endregion

        #region Spear Movement Functions
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
            meleeCollision.enabled = true;
            cooldownBar.StartCooldown(spearCooldown);
            transform.Translate(spearTravelDistance, 0, 0);

            yield return new WaitForSeconds(spearReturnDelay);

            transform.Translate(-spearTravelDistance, 0, 0);

            meleeCollision.enabled = false;
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
                        if (!TouchingGround)
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
        /// Look see if the mouse is on the left or right of the screen
        /// </summary>
        private bool MouseLeftOrRight()
        {
            var playerScreenPoint = Camera.main.WorldToScreenPoint(controller.transform.position);
            float mouseX = controls.Player.MousePostion.ReadValue<Vector2>().x;

            return mouseX < playerScreenPoint.x ? true : false;
        }
        /// <summary>
        /// Disable spear rotation
        /// </summary>
        public void DisableSpear()
        {
            canRotate = false;
        }
        /// <summary>
        /// Enable spear rotation
        /// </summary>
        public void EnableSpear()
        {
            canRotate = true;
        }
        #endregion

        #region Damage Functions
        /// <summary>
        /// If spear collision is active check to see if spear has hit any objects
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision)
            {
                if (GeneralFunctions.IsObjectOnLayer(gameplayManager.whatCanBeDamaged, collision.gameObject))
                {
                    if (!collision.gameObject.CompareTag("Player") && !GeneralFunctions.DoesItemExistInArray(collidersToDamage.ToArray(), collision) && !collision.isTrigger)
                    {
                        collidersToDamage.Add(collision);
                    }

                    if (collidersToDamage.Count > 0)
                    {
                        DamageAllObjects();
                    }
                }
            }
        }
        /// <summary>
        /// Damage all overlapped Gameobjects
        /// </summary>
        private void DamageAllObjects()
        {
            foreach (Collider2D collider2 in collidersToDamage)
            {
                if (collider2)
                {
                    if (!collider2.gameObject.CompareTag("Player"))
                    {
                        var leechEggRipe = collider2.gameObject.GetComponent<LeechEggRipe>();
                        var leechEggCold = collider2.gameObject.GetComponent<LeechEggCold>();
                        var wormSegment = collider2.gameObject.GetComponent<WormSegment>();

                        if (leechEggRipe)
                        {
                            leechEggRipe.SpawnLeech();
                        }
                        else if (leechEggCold)
                        {
                            leechEggCold.SpawnLeech();
                        }
                        else if (wormSegment)
                        {
                            if (wormSegment.AboveGround)
                            {
                                GeneralFunctions.DamageTarget(collider2.gameObject, spearDamage, true, gameObject);
                            }
                        }
                        else if (!leechEggRipe && !leechEggCold && !wormSegment)
                        {
                            GeneralFunctions.DamageTarget(collider2.gameObject, spearDamage, true, gameObject);
                        }
                    }
                }
            }

            collidersToDamage.Clear();
        }
        #endregion
    }
}