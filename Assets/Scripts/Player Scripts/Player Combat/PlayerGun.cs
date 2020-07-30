﻿using UnityEngine;
using PlayerUI;
using GameplayManagement;
using System;
using UnityEngine.InputSystem;
using PlayerControls;

namespace PlayerCharacter.Controller
{
    [Serializable]
    public class PlayerGun : MonoBehaviour
    {
        [Header("Gun Settings")]
        [SerializeField] private float laserUpTime = 0.02f;
        [SerializeField] private float gunDamage = 3.5f;
        [SerializeField] private float gunCooldown = 1f;
        
        #region Gun Components
        private Transform gunFireLocation = null;
        private PlayerProjectile projectileToSpawn = null;
        private PlayerController controller = null;
        private CooldownBar cooldownBar = null;
        private HealthComponent playerHealthComp = null;
        #endregion

        #region Local Varabiles
        private float gunAngle = 0f;
        private bool gamepadActive = false;
        private bool touchingGround = false;
        private const float traceLength = 1f;
        private GameplayManager gameplayManager = null;
        private Controls controls = null;
        #endregion

        private void Awake()
        {
            controls = new Controls();

            SetupGun();
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

        void Update()
        {
            RotateGun();

            if (!gamepadActive)
            {
                RotatePlayerWithMouse();
            }
        }

        /// <summary>
        /// Will try to spawn the player projectile to damage Gameobjects
        /// </summary>
        public void FireGun()
        {
            if (CanGunSpawnProjectile())
            {
                PlayerProjectile gunHit = Instantiate(projectileToSpawn, (Vector2)gunFireLocation.position, gunFireLocation.rotation);

                gunHit.ConstructBox(gunDamage, laserUpTime);

                cooldownBar.StartCooldown(gunCooldown);
            }
            else
            {
                if (!GeneralFunctions.IsPlayerDead())
                {
                    // if a projectile was not able to be fired check to see if there is in front of the player and damage it
                    CheckForEnemy();
                }
            }
        }
        /// <summary>
        /// Fire a raycast to check for enemies around the player if player projectile failed to spawn
        /// </summary>
        private void CheckForEnemy()
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.transform.position, transform.right, traceLength, gameplayManager.whatCanBeDamaged);

            if (hit)
            {
                GeneralFunctions.DamageTarget(hit.transform.gameObject, gunDamage, true);
            }
        }
        /// <summary>
        /// Rotate the gun either to the mouse location or to the gamepad joystick position
        /// </summary>
        private void RotateGun()
        {
            if (!playerHealthComp.IsCurrentlyDead)
            {
                if (!gamepadActive)
                {
                    RotateGunWithMouse();
                }
                else
                {
                    RotateGunWithGamepad();
                }
            }
        }
        /// <summary>
        /// If a Gamepad is not active get mouse location and rotate gun around the player character with mouse
        /// </summary>
        private void RotateGunWithMouse()
        {
            Vector3 mousePos = controls.Player.MousePostion.ReadValue<Vector2>();
            Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);

            mousePos.x = mousePos.x - gunPos.x;
            mousePos.y = mousePos.y - gunPos.y;
            gunAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            if (MouseLeftOrRight())
            {
                transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunAngle));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunAngle));
            }
        }
        /// <summary>
        /// Rotate the player left or right to match the direction the gun is facing with mouse controls
        /// </summary>
        private void RotatePlayerWithMouse()
        {
            if (!playerHealthComp.IsCurrentlyDead)
            {
                if (!gamepadActive)
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
        /// If a gamepad is active get value from right stick and calculate gun rotation
        /// </summary>
        private void RotateGunWithGamepad()
        {
            //Vector2 stickInput = new Vector2(CrossPlatformInputManager.GetAxis("Joy X"), CrossPlatformInputManager.GetAxis("Joy Y"));

            //if (stickInput.magnitude > 0)
            //{
            //    var lookAt = GeneralFunctions.LookAt2D(transform.position, transform.position + (Vector3)stickInput);

            //    transform.rotation = Quaternion.Inverse(lookAt);
            //}
        }
        /// <summary>
        /// Check to see if there is a gamepad active if not default input to mouse
        /// </summary>
        public void UpdateInput(bool gamepadActive)
        {
            
        }
        /// <summary>
        /// Checks to see if the gun is being block by ground
        /// </summary>
        /// <returns>A bool that determines if the player projectile can spawn</returns>
        private bool CanGunSpawnProjectile()
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                if (!cooldownBar.GetIsActive())
                {
                    if (!touchingGround)
                    {
                        return true;
                    }
                    else
                    {
                        cooldownBar.StartCooldown(gunCooldown);
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
        /// Collects all need components for the gun to work
        /// </summary>
        private void SetupGun()
        {
            gunFireLocation = transform.GetChild(2).transform;
            controller = GetComponentInParent<PlayerController>();
            cooldownBar = transform.GetChild(3).transform.GetChild(0).GetComponent<CooldownBar>();
            playerHealthComp = GetComponentInParent<HealthComponent>();
            gameplayManager = FindObjectOfType<GameplayManager>();
            GetPlayerProjectile();
        }
        /// <summary>
        /// Finds the player projectile asset in the resource folder
        /// </summary>
        private void GetPlayerProjectile()
        {
            var prefab = Resources.Load("Player/Gun Laser") as GameObject;

            if (prefab)
            {
                projectileToSpawn = prefab.GetComponent<PlayerProjectile>();
            }
        }
    }
}