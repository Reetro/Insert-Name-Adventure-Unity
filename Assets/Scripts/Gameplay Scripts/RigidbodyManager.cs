using UnityEngine;

public class RigidbodyManager : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D = null;
    private bool isZFrozen = false;
    private bool wasFrozenByPlatfrom = false;

    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        isZFrozen = myRigidBody2D.freezeRotation;
    }

    public void OnPlatfromEnter()
    {
        if (!isZFrozen)
        {
            myRigidBody2D.freezeRotation = true;
            wasFrozenByPlatfrom = true;
        }
    }

    public void OnPlatfromExit()
    {
        if (wasFrozenByPlatfrom && !isZFrozen)
        {
            myRigidBody2D.freezeRotation = false;
            wasFrozenByPlatfrom = false;
        }
    }
}
