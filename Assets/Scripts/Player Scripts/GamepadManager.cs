using UnityEngine;

public class GamepadManager : MonoBehaviour
{
    [SerializeField] PlayerGun playerGun = null;

    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 0)
            {
                PS4_Controller = 0;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 19)
            {
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
                PS4_Controller = 0;
                Xbox_One_Controller = 1;
            }
        }


        if (Xbox_One_Controller == 1)
        {
            //Xbox controller connect
            playerGun.UpdateInput(true);
        }
        else if (PS4_Controller == 1)
        {
            //PS4 controller connected
            playerGun.UpdateInput(true);
        }
        else if (PS4_Controller == 0 && Xbox_One_Controller == 0)
        {
            // there is no controllers
            playerGun.UpdateInput(false);
        }
    }
}