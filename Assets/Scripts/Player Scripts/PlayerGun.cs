using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] PlayerController player = null;

    void Update()
    {
        RotateGunToMouse();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousepos.x < transform.position.x)
        {
            player.transform.eulerAngles = new Vector3(transform.position.x, 180f, transform.position.z);
        }
        else
        {
            player.transform.eulerAngles = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }

    private void RotateGunToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        float gunAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        bool leftOrRight = Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x;

        if (leftOrRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunAngle));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunAngle));
        }
    }
}
