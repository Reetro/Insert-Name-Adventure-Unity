using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerGun currentGun = null;
    [SerializeField] GameObject leechCollision = null;
    [SerializeField] GameObject playerState = null;
    [SerializeField] GameObject playerHud = null;
    [SerializeField] GameObject levelLoader = null;

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

        SpawnLeechCollision();
        SetupScene();
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

    public void OnDeath()
    {
        uiManager.ShowDeathUI();

        playerMovement.StopMovement();
    }    

    public void OnLanding()
    {
        myAnimator.SetBool("IsJumping", false);

        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            jump = true;
        }
    }

    private void SpawnLeechCollision()
    {
        Instantiate(leechCollision, new Vector2(1000, 1000), Quaternion.identity);
    }

    private void SetupScene()
    {
        var loaderCount = FindObjectsOfType<LevelLoader>().Length;

        if (loaderCount <= 0)
        {
            levelLoader = Instantiate(levelLoader, new Vector2(1000, 1000), Quaternion.identity);
        }

        var levelExit = FindObjectOfType<LevelExit>();

        if (levelExit)
        {
            levelExit.ConsturctExit(levelLoader.GetComponent<LevelLoader>());
        }

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

        var hudCount = FindObjectsOfType<PlayerUIManager>().Length;

        if (hudCount <= 0)
        {
            playerHud = Instantiate(playerHud, new Vector2(1000, 1000), Quaternion.identity);
        }

        myHealthComp.FindPlayerState(playerHud.GetComponent<PlayerUIManager>().GetHealthBar());

        var auraManager = GetComponent<AuraManager>();

        if (auraManager)
        {
            auraManager.SetUIManager(playerHud.GetComponent<PlayerUIManager>());
        }
        else
        {
            Debug.LogWarning("Player has no Aura manager");
        }

        uiManager = FindObjectOfType<PlayerUIManager>();
    }
}