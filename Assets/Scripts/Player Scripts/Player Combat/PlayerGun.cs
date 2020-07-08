using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerGun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float laserUpTime = 0.02f;
    [SerializeField] private float gunDamage = 3.5f;
    [SerializeField] private float gunCooldown = 1f;

    [Header("Config Settings")]
    [SerializeField] private PlayerProjectile hitBoxToSpawn = null;
    [SerializeField] private Transform gunFireLocation = null;
    [SerializeField] private PlayerController controller = null;
    [SerializeField] private CooldownBar cooldownBar = null;
    [SerializeField] private HealthComponent playerHealthComp = null;

    private float gunAngle = 0f;
    private bool gamepadActive = false;
    private bool touchingGround = false;
    private const float traceLength = 1f;

    void Update()
    {
        RotateGun();
        RotatePlayer();
    }

    /// <summary>
    /// Will try to spawn the player projectile to damage Gameobjects
    /// </summary>
    public void FireGun()
    {
        if (CanGunSpawnProjectile())
        {
            PlayerProjectile gunHit = Instantiate(hitBoxToSpawn, (Vector2)gunFireLocation.position, gunFireLocation.rotation);

            gunHit.ConstructBox(gunDamage, laserUpTime);

            cooldownBar.StartCooldown(gunCooldown);
        }
        else
        {
            if (!GeneralFunctions.IsPlayerDead())
            {
                // if a projectile was not able to be fired check to see if there is a still a leech attached to the player and damage it
                DamageAttachedLeech();
            }
        }
    }
    /// <summary>
    /// Fire a raycast to check attached leeches and if found will damage it only called if the player projectile failed to spawn
    /// </summary>
    private void DamageAttachedLeech()
    {
        var player = GeneralFunctions.GetPlayerGameObject();

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, transform.right, traceLength, LayerMask.GetMask("Attached Leech"));

        if (hit)
        {
            GeneralFunctions.DamageTarget(hit.transform.gameObject, gunDamage, true);
        }
    }
    /// <summary>
    /// Rotate the player left or right to match the direction the gun is facing
    /// </summary>
    private void RotatePlayer()
    {
        if (!playerHealthComp.GetIsDead())
        {
            if (!gamepadActive)
            {
                if (MouseLeftOrRight())
                {
                    controller.transform.eulerAngles = new Vector3(transform.position.x, 180f, transform.position.z);
                }
                else
                {
                    controller.transform.eulerAngles = new Vector3(transform.position.x, 0f, transform.position.z);
                }
            }
        }
    }
    /// <summary>
    /// Rotate the gun either to the mouse location or to the gamepad joystick position
    /// </summary>
    private void RotateGun()
    {
        if (!playerHealthComp.GetIsDead())
        {
            if (!gamepadActive)
            {
                Vector3 mousePos = Input.mousePosition;
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
            else
            {
                float deadzone = 0.25f;
                Vector2 stickInput = new Vector2(CrossPlatformInputManager.GetAxis("Joy X"), CrossPlatformInputManager.GetAxis("Joy Y"));

                if (stickInput.magnitude < deadzone)
                {
                    stickInput = Vector2.zero;
                }
                else
                {
                    stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
                }

                if (stickInput.sqrMagnitude >= 1)
                {
                    gunAngle = Mathf.Atan2(stickInput.x, stickInput.y) * Mathf.Rad2Deg;

                    if (stickInput.y < 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunAngle));

                        controller.transform.eulerAngles = new Vector3(transform.position.x, 180f, transform.position.z);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunAngle));

                        controller.transform.eulerAngles = new Vector3(transform.position.x, 0f, transform.position.z);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Check to see if there is a gamepad active if not default input to mouse
    /// </summary>
    public void UpdateInput(bool gamepadActive)
    {
        if (gamepadActive)
        {
            this.gamepadActive = true;

            Cursor.visible = false;

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            this.gamepadActive = false;

            Cursor.visible = true;

            Cursor.lockState = CursorLockMode.None;
        }
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
        float mouseX = Input.mousePosition.x;

        return mouseX < playerScreenPoint.x ? true : false;
    }
}