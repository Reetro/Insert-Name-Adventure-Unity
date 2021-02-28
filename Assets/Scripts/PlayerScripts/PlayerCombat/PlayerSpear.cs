using System;
using System.Collections;
using System.Collections.Generic;
using EnemyScripts.LeechScripts;
using GameplayScripts;
using GeneralScripts;
using GeneralScripts.GeneralComponents;
using GeneralScripts.UI;
using PlayerControls;
using PlayerScripts.PlayerControls;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts.PlayerCombat
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
        [FormerlySerializedAs("BlockLayers")] [Tooltip("Which layers will block the spear from moving")]
        public LayerMask blockLayers;

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
        public LayerMask WhatIsGround => blockLayers;

        #endregion

        #region Spear Components
        private PlayerController controller;
        private CooldownBar cooldownBar;
        private HealthComponent playerHealthComp;
        #endregion

        #region Local Varabiles
        private float spearAngle;
        private const float TraceLength = 1f;
        private GameplayManager gameplayManager;
        private Controls controls;
        private bool canRotate;
        private bool isSpearOut;
        private bool isSpearDisabled;
        private GameObject playerObject;
        private PolygonCollider2D meleeCollision;
        #endregion

        #region JoyStick Rotation
        private Vector2 stickInput;
        private float spearRotation;
        private float spearRotationX;
        private bool facingRight = true;
        private Camera camera1;

        #endregion

        #region Setup Functions

        private void Start()
        {
            camera1 = Camera.main;
        }

        /// <summary>
        /// Create new player controls object
        /// </summary>
        private void Awake()
        {
            controls = new Controls();
        }
        /// <summary>
        /// When scene is done loading setup spear references
        /// </summary>
        public void OnSceneLoaded()
        {
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
            if (!GeneralFunctions.GetGameplayManager().IsGamePaused)
            {
                if (!controller.ControlDisabled)
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
        private void UpdateJoystick()
        {
            stickInput = controls.Player.Rotate.ReadValue<Vector2>();
        }
        #endregion

        #region Spear Movement Functions
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Will try to push the spear in the direction its facing
        /// </summary>
        public void StartSpearPush()
        {
            if (CanPushSpear(out var nextToWall, out var doCooldown))
            {
                isSpearOut = true;
                canRotate = false;

                StartCoroutine(PushSpear(doCooldown));
            }
            else if (!GeneralFunctions.IsPlayerDead() && !cooldownBar.GetIsActive())
            {
                if (doCooldown)
                {
                    cooldownBar.StartCooldown(spearCooldown);
                }

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
        private IEnumerator PushSpear(bool doCooldown)
        {
            meleeCollision.enabled = true;

            if (doCooldown)
            {
                cooldownBar.StartCooldown(spearCooldown);
            }

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
            RaycastHit2D hit = Physics2D.Raycast(controller.transform.position, transform.right, TraceLength, gameplayManager.whatCanBeDamaged);

            if (hit)
            {
                if (GeneralFunctions.GetGameObjectHealthComponent(hit.transform))
                {
                    GeneralFunctions.ApplyDamageToTarget(hit.transform.gameObject, spearDamage, true, gameObject);
                }
            }
        }
        /// <summary>
        /// Rotate the spear either to the mouse location or to the Gamepad joystick position
        /// </summary>
        private void RotateSpear()
        {
            if (!GeneralFunctions.GetGameplayManager().IsGamePaused)
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
        }
        /// <summary>
        /// If a Gamepad is not active get mouse location and rotate spear around the player character with mouse
        /// </summary>
        private void RotateSpearWithMouse()
        {
            Vector3 mousePos = controls.Player.MousePostion.ReadValue<Vector2>();
            if (!(camera1 is null))
            {
                Vector3 spearPos = camera1.WorldToScreenPoint(transform.position);

                mousePos.x -= spearPos.x;
                mousePos.y -= spearPos.y;
            }

            spearAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(MouseLeftOrRight() ? new Vector3(180f, 0f, -spearAngle) : new Vector3(0f, 0f, spearAngle));
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
                    var position = transform.position;
                    controller.transform.eulerAngles = MouseLeftOrRight() ? new Vector3(0, 180f, position.z) : new Vector3(0, 0f, position.z);
                }
            }
        }
        /// <summary>
        /// Rotate player with right Gamepad joy stick
        /// </summary>
        private void RotatePlayerWithGamepad()
        {
            if (!GeneralFunctions.GetGameplayManager().IsGamePaused)
            {
                facingRight = !facingRight;

                GeneralFunctions.FlipObject(controller.gameObject);
            }
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
        private bool CanPushSpear(out bool nextToWall, out bool doCooldown)
        {
           if (!isSpearDisabled)
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
                               doCooldown = true;
                               return true;
                           }
                           else
                           {
                               var hit = GeneralFunctions.TraceFromEyes(playerObject);

                               if (hit)
                               {
                                   doCooldown = true;
                                   nextToWall = true;
                               }
                               else
                               {
                                   doCooldown = true;
                                   nextToWall = false;
                               }

                               return false;
                           }
                       }
                       else
                       {
                           doCooldown = false;
                           nextToWall = false;
                           return false;
                       }
                   }
                   else
                   {
                       doCooldown = true;
                       nextToWall = false;
                       return false;
                   }
               }
               else
               {
                   nextToWall = false;
                   doCooldown = false;
                   return false;
               }
           }
           else
           {
               doCooldown = false;
               nextToWall = false;
               return false;
           }
        }
        /// <summary>
        /// Look see if the mouse is on the left or right of the screen
        /// </summary>
        private bool MouseLeftOrRight()
        {
            if (camera1 is null) return false;
            var playerScreenPoint = camera1.WorldToScreenPoint(controller.transform.position);
            var mouseX = controls.Player.MousePostion.ReadValue<Vector2>().x;

            return mouseX < playerScreenPoint.x;
        }
        /// <summary>
        /// Disable spear rotation
        /// </summary>
        public void DisableSpear()
        {
            canRotate = false;
            isSpearDisabled = true;
        }
        /// <summary>
        /// Enable spear rotation
        /// </summary>
        public void EnableSpear()
        {
            canRotate = true;
            isSpearDisabled = false;
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
                        //var wormSegment = collider2.gameObject.GetComponent<WormSegment>();

                        if (leechEggRipe)
                        {
                            leechEggRipe.SpawnLeech();
                        }
                        else if (leechEggCold)
                        {
                            leechEggCold.SpawnLeech();
                        }
                        /*else if (wormSegment)
                        {
                            if (wormSegment.AboveGround)
                            {
                                GeneralFunctions.DamageTarget(collider2.gameObject, spearDamage, true, gameObject);
                            }
                        }*/
                        else if (!leechEggRipe && !leechEggCold)
                        {
                            GeneralFunctions.ApplyDamageToTarget(collider2.gameObject, spearDamage, true, gameObject);
                        }
                    }
                }
            }

            collidersToDamage.Clear();
        }
        #endregion
    }
}