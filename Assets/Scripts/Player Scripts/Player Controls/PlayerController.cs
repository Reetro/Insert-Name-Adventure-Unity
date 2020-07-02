﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerGun currentGun = null;
    [SerializeField] GameObject leechCollision = null;
    [SerializeField] GameObject playerState = null;
    [SerializeField] ScriptableBuff buff = null;

    [Header("Run Settings")]
    [SerializeField] float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    Animator myAnimator = null;
    PlayerMovement playerMovement = null;
    HealthComponent myHealthComp = null;
    PlayerUIManager uiManager = null;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        myHealthComp = GetComponent<HealthComponent>();
        uiManager = FindObjectOfType<PlayerUIManager>();

        SpawnLeechCollision();
        SpawnPlayerState();
    }

    private void Update()
    {
        horizontalMove = CrossPlatformInputManager.GetAxisRaw("Horizontal") * runSpeed;

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

        if (Input.GetKeyDown(KeyCode.J))
        {
            var aura = GetComponent<AuraManager>();

            aura.ApplyBuff(gameObject, buff, true);
        }
    }

    private void FixedUpdate()
    {
        playerMovement.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
        jump = false;
    }

    public void OnDeath()
    {
        uiManager.ShowDeathUI();

        playerMovement.StopMovement();
    }    

    public void OnLanding()
    {
        myAnimator.SetBool("IsJumping", false);
    }

    private void SpawnLeechCollision()
    {
        Instantiate(leechCollision, new Vector2(1000, 1000), Quaternion.identity);
    }

    private void SpawnPlayerState()
    {
        var playerStateCount = FindObjectsOfType<PlayerState>().Length;

        if (playerStateCount <= 0)
        {
            Instantiate(playerState, new Vector2(1000, 1000), Quaternion.identity);
        }

        var checkpoint = FindObjectOfType<Checkpoint>();

        if (checkpoint)
        {
            checkpoint.ConstructCheckpoint();
        }

        myHealthComp.FindPlayerState();
    }
}