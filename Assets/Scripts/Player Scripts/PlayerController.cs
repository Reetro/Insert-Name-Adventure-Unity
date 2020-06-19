using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerGun currentGun = null;
    [SerializeField] GameObject leechCollision = null;
    [SerializeField] GameObject playerState = null;
    [SerializeField] PlayerUIManager uiManager = null;
    
    [Header("Run Settings")]
    [SerializeField] float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    Animator myAnimator = null;
    CharacterController2D controller = null;
    HealthComponent myHealthComp = null;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        myHealthComp = GetComponent<HealthComponent>();

        SpawnLeechCollision();
        SpawnPlayerState();
    }

    private void Update()
    {
        if (!myHealthComp.GetIsDead())
        {
            horizontalMove = CrossPlatformInputManager.GetAxisRaw("Horizontal") * runSpeed;

            myAnimator.SetFloat("Speed", horizontalMove);

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

            if (CrossPlatformInputManager.GetAxis("TriggerFire") > 0.1)
            {
                currentGun.FireGun();
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
        jump = false;
    }

    public void OnDeath()
    {
        uiManager.ShowDeathUI();
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
