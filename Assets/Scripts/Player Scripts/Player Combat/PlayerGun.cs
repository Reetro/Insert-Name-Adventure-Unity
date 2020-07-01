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

    void Update()
    {
        RotateGun();
        RotatePlayer();
    }

    public void FireGun()
    {
        if (CanGunFire())
        {
            PlayerProjectile gunHit = Instantiate(hitBoxToSpawn, (Vector2)gunFireLocation.position, gunFireLocation.rotation);

            gunHit.ConstructBox(gunDamage, laserUpTime);

            cooldownBar.StartCooldown(gunCooldown);
        }
    }

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

    private bool CanGunFire()
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        touchingGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touchingGround = false;
    }

    private bool MouseLeftOrRight()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(controller.transform.position);
        float mouseX = Input.mousePosition.x;

        return mouseX < playerScreenPoint.x ? true : false;
    }
}
