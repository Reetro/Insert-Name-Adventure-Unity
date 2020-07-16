﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerGun currentGun = null;
    [SerializeField] private PlayerUIManager uiManager = null;

    private float horizontalMove = 0f;
    private bool jump = false;
    private Animator myAnimator = null;
    private PlayerMovement playerMovement = null;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        uiManager = FindObjectOfType<PlayerUIManager>();
    }

    private void Update()
    {
        if (!GeneralFunctions.IsPlayerDead())
        {
            horizontalMove = CrossPlatformInputManager.GetAxisRaw("Horizontal");

            if (transform.localEulerAngles.y >= 180)
            {
                myAnimator.SetFloat("Speed", -horizontalMove);
            }
            else
            {
                myAnimator.SetFloat("Speed", horizontalMove);
            }

            if (horizontalMove == 0)
            {
                myAnimator.SetBool("Idle", true);
            }
            else
            {
                myAnimator.SetBool("Idle", false);
            }

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                jump = true;
                myAnimator.SetBool("IsJumping", true);
            }

            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                currentGun.FireGun();
            }

            if (CrossPlatformInputManager.GetAxis("Fire1") > 0)
            {
                currentGun.FireGun();
            }
        }
    }

    private void FixedUpdate()
    {
        playerMovement.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
        jump = false;
    }
    /// <summary>
    /// Called when player touches ground
    /// </summary>
    public void OnLanding()
    {
        myAnimator.SetBool("IsJumping", false);

        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            jump = true;
        }
    }
    /// <summary>
    /// Called when player dies will stop all player movement
    /// </summary>
    public void OnDeath()
    {
        uiManager.ShowDeathUI();

        playerMovement.StopMovement();
    }
}