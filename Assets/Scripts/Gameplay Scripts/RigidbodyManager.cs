using UnityEngine;
using UnityEngine.Events;

public class RigidbodyManager : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D = null;
    private bool isZFrozen = false;
    private bool wasFrozenByPlatfrom = false;

    [Header("Events")]
    [Space]
    public UnityEvent OnPlatformEnter;
    [Space]
    public UnityEvent OnPlatformExit;

    void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        isZFrozen = myRigidBody2D.freezeRotation;
    }

    public void OnPlatfromEnter()
    {
        OnPlatformEnter.Invoke();

        if (!isZFrozen)
        {
            myRigidBody2D.freezeRotation = true;
            wasFrozenByPlatfrom = true;

            myRigidBody2D.gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, 0);
        }
    }

    public void OnPlatfromExit()
    {
        OnPlatformExit.Invoke();

        if (wasFrozenByPlatfrom && !isZFrozen)
        {
            myRigidBody2D.freezeRotation = false;
            wasFrozenByPlatfrom = false;
        }
    }
}
