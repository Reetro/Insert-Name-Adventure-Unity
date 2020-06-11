﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] PlayerController controller = null;

    [Header("Gun Settings")]
    [SerializeField] float gunRange = 10f;
    [SerializeField] float laserUpTime = 2f;
    [SerializeField] float gunDamage = 3.5f;

    [Header("Config Settings")]
    public LayerMask m_WhatCanIHit;
    public Transform fireLocation = null;
    public LineRenderer lineRender = null;

    [Header("Debug Settings")]
    [SerializeField] bool debugGun = false;
    [SerializeField] float debugLineDuration = 2f;

    float gunAngle = 0f;

    void Update()
    {
        RotateGunToMouse();
        RotatePlayer();
    }

    public IEnumerator FireGun()
    {
        var fireDirection = GeneralFunctions.GetDirectionVector2D(gunAngle);
        lineRender.positionCount = 2;

        if (debugGun)
        {
            Debug.DrawRay(fireLocation.position, fireDirection * gunRange, Color.red, debugLineDuration);
        }

        // Fire gun ray cast from given fire locations
        RaycastHit2D hit2D = Physics2D.Raycast(fireLocation.position, fireLocation.TransformDirection(fireDirection), gunRange, m_WhatCanIHit);
        
        if (hit2D)
        {
            if (debugGun)
            {
                Debug.Log(hit2D.collider.name);
            }

            // see if hit object can be damaged
            var healtComp = hit2D.transform.GetComponent<HealthComponent>();

            if (healtComp)
            {
                healtComp.ProccessDamage(gunDamage);
            }

            // Draw laser
            lineRender.SetPosition(0, fireLocation.position);
            lineRender.SetPosition(1, hit2D.point);
        }
        else
        {
            Vector3 newFireDirection = fireDirection;

            // Get trace end point
            var endPoint = fireLocation.position + newFireDirection * gunRange;

            // Draw laser
            lineRender.SetPosition(0, fireLocation.position);
            lineRender.SetPosition(1, endPoint);
        }

        // Show laser
        lineRender.enabled = true;

        yield return new WaitForSeconds(laserUpTime);

        // hide laser
        lineRender.enabled = false;
        lineRender.positionCount = 0;
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
