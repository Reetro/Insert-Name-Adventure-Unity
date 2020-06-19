using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerGun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float laserUpTime = 0.02f;
    [SerializeField] private float gunDamage = 3.5f;
    [SerializeField] private float gunCooldown = 1f;

    [Header("Config Settings")]
    [SerializeField] private HitBox hitBoxToSpawn = null;
    [SerializeField] private Transform gunFireLocation = null;
    [SerializeField] private PlayerController controller = null;
    [SerializeField] private CooldownBar cooldownBar = null;
    [SerializeField] private HealthComponent playerHealthComp = null;

    private float gunAngle = 0f;
    private bool gamePadActive = false;

    void Update()
    {
        RotateGun();
        RotatePlayerToMouse();
    }

    public void FireGun()
    {
        if (!cooldownBar.GetIsActive())
        {
            HitBox gunHit = Instantiate(hitBoxToSpawn, (Vector2)gunFireLocation.position, gunFireLocation.rotation) as HitBox;

            gunHit.ConstructBox(gunDamage, laserUpTime, false, true);

            cooldownBar.StartCooldown(gunCooldown);
        }
    }

    private void RotatePlayerToMouse()
    {
        if (!playerHealthComp.GetIsDead())
        {
            if (!gamePadActive)
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
            if (!gamePadActive)
            {
                Vector3 mousePos = CrossPlatformInputManager.mousePosition;
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
                Vector2 stickInput = new Vector2(CrossPlatformInputManager.GetAxis("HorizontalRightStick"), CrossPlatformInputManager.GetAxis("VerticalRightStick"));

                if (stickInput.magnitude < deadzone)
                {
                    stickInput = Vector2.zero;
                }
                else
                {
                    stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
                }

                if (stickInput.sqrMagnitude >= 0.1)
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

    private bool MouseLeftOrRight()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(controller.transform.position);
        float mouseX = CrossPlatformInputManager.mousePosition.x;

        return mouseX < playerScreenPoint.x ? true : false;
    }

    public void UpdateInput(bool gamePadActive)
    {
        if (gamePadActive)
        {
            this.gamePadActive = true;

            Cursor.visible = false;

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            this.gamePadActive = false;

            Cursor.visible = true;

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
