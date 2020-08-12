using UnityEngine;
using UnityEngine.Events;

public class RigidbodyManager : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D = null;
    private bool isZFrozen = false;
    private bool wasFrozenByPlatfrom = false;

    [HideInInspector]
    public UnityEvent OnPlatformEnter;
    [HideInInspector]
    public UnityEvent OnPlatformExit;

    void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        isZFrozen = myRigidBody2D.freezeRotation;
    }

    public void OnPlatfromEnterCall()
    {
        OnPlatformEnter.Invoke();

        if (!isZFrozen)
        {
            myRigidBody2D.freezeRotation = false;
            wasFrozenByPlatfrom = true;
        }
    }

    public void OnPlatfromExitCall()
    {
        OnPlatformExit.Invoke();

        if (wasFrozenByPlatfrom && !isZFrozen)
        {
            myRigidBody2D.freezeRotation = false;
            wasFrozenByPlatfrom = false;
        }
    }
}
