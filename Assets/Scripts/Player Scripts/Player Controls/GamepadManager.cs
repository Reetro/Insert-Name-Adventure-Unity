using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GamepadManager : MonoBehaviour
{
    [SerializeField] PlayerGun playerGun = null;

    public bool playstationController, xboxController, keyboard;
    public string[] currentControllers;
    public float controllerCheckTimer = 2;
    public float controllerCheckTimerOG = 2;

    private void Start()
    {
        ControllerCheck();
    }

    void Update()
    {
        bool currentlyMoving = CrossPlatformInputManager.GetAxis("Horizontal") < 0;

        if (!CrossPlatformInputManager.GetButtonDown("Fire1") && !CrossPlatformInputManager.GetButton("Jump") && !currentlyMoving)
        {
            controllerCheckTimer -= Time.deltaTime;
            if (controllerCheckTimer <= 0)
            {
                ControllerCheck();
                controllerCheckTimer = controllerCheckTimerOG;
            }
        }
        else
        {
            controllerCheckTimer = controllerCheckTimerOG;
        }
    }

    public void ControllerCheck()
    {
        System.Array.Clear(currentControllers, 0, currentControllers.Length);
        System.Array.Resize<string>(ref currentControllers, Input.GetJoystickNames().Length);
        int numberOfControllers = 0;
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            currentControllers[i] = Input.GetJoystickNames()[i].ToLower();
            if ((currentControllers[i] == "controller (xbox 360 for windows)" || currentControllers[i] == "controller (xbox 360 wireless receiver for windows)" || currentControllers[i] == "controller (xbox one for windows)"))
            {
                xboxController = true;
                keyboard = false;
                playstationController = false;
            }
            else if (currentControllers[i] == "wireless controller")
            {
                playstationController = true; //not sure if wireless controller is just super generic but that's what DS4 comes up as.
                keyboard = false;
                xboxController = false;
            }
            else if (currentControllers[i] == "")
            {
                numberOfControllers++;
            }
        }
        if (numberOfControllers == Input.GetJoystickNames().Length)
        {
            keyboard = true;
            xboxController = false;
            playstationController = false;
        }

        UpdateGameInput();
    }

    private void UpdateGameInput()
    {
        if (xboxController && !playstationController && !keyboard)
        {
            playerGun.UpdateInput(true);
        }
        else if (playstationController && !playstationController && !keyboard)
        {
            playerGun.UpdateInput(true);
        }
        else if (keyboard && !xboxController && !playstationController)
        {
            playerGun.UpdateInput(false);
        }
    }
}
