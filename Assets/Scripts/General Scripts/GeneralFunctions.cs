using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunctions
{
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

    public static bool IsNumberNegative(float number)
    {
        Debug.Log(number);

        if (number < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
