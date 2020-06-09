using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunctions
{
    // if the given object has a Rigidbody2D it will check to see which way the object is moving and flip it's sprite in the given direction
    public static void FlipSprite(GameObject objectToFlip, float horzontial)
    {
        if (horzontial > 0)
        {
            objectToFlip.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            objectToFlip.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    // will check to see if the given object has a Rigidbody2D and if it does will see if it's velocity is greater than zero
    public static bool IsObjectMovingHorizontaly(GameObject objectToTest)
    {
        var rigidBody = objectToTest.GetComponent<Rigidbody2D>();

        if (rigidBody)
        {
            return Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        }
        else
        {
            Debug.LogError(objectToTest.ToString() + "didn't have a a Rigidbody2D was unable to test it horizontally");
            return false;
        }
    }
}
