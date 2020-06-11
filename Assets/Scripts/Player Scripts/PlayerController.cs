using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Run Settings")]
    [SerializeField] float runSpeed = 40f;
    [SerializeField] PlayerGun currentGun = null;

    float horizontalMove = 0f;
    bool jump = false;
    Animator myAnimator = null;
    CharacterController2D controller = null;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        myAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            myAnimator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            currentGun.FireGun();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<HealthComponent>().ProccessDamage(1);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, false);
        jump = false;
    }

    public void OnDeath()
    {
        Debug.Log("you dead");
    }    

    public void OnLanding()
    {
        myAnimator.SetBool("IsJumping", false);
    }
}
