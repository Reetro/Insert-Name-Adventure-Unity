using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerGun currentGun = null;
    [SerializeField] GameObject leechCollision = null;
    [SerializeField] GameObject playerState = null;

    [Header("Run Settings")]
    [SerializeField] float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    Animator myAnimator = null;
    CharacterController2D controller = null;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

        SpawnLeechCollision();
        SpawnPlayerState();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        myAnimator.SetFloat("Speed", horizontalMove);

        if (horizontalMove == 0)
        {
            myAnimator.SetBool("Idle", true);
        }
        else
        {
            myAnimator.SetBool("Idle", false);
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            myAnimator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            currentGun.FireGun();
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
        jump = false;
    }

    public void OnDeath()
    {
        // TODO Setup check point system and add a restart button

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
    }
}
