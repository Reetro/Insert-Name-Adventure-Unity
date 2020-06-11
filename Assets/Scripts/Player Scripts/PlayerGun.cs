using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] PlayerController player = null;
    public Transform fireLocation = null;
    public LineRenderer lineRender = null;
    [SerializeField] float gunRange = 10f;

    void Update()
    {
        RotateGunToMouse();
        RotatePlayer();
    }

    public void FireGun()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(fireLocation.position, fireLocation.rotation.eulerAngles);

        if (hitInfo)
        {
            Debug.Log(hitInfo.collider.name);

            Debug.DrawLine(fireLocation.position, hitInfo.point, Color.green, 2.4f);
        }
        else
        {
            
        }
    }

    private void RotatePlayer()
    {
        if (MouseLeftOrRight())
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

        if (MouseLeftOrRight())
        {
            transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunAngle));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunAngle));
        }
    }

    private bool MouseLeftOrRight()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(player.transform.position);
        float mouseX = Input.mousePosition.x;

        if (mouseX < playerScreenPoint.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
