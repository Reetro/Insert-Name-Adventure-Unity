﻿using UnityEngine;

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

    private float gunAngle = 0f;

    void Update()
    {
        RotateGunToMouse();
        RotatePlayer();
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

    private void RotatePlayer()
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

    private void RotateGunToMouse()
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

    private bool MouseLeftOrRight()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(controller.transform.position);
        float mouseX = Input.mousePosition.x;

        return mouseX < playerScreenPoint.x ? true : false;
    }
}
