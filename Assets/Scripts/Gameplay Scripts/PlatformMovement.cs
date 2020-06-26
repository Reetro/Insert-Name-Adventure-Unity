﻿using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed;
    public Transform startPos;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GeneralFunctions.IsObjectPlayer(collision.gameObject))
        {
            GeneralFunctions.AttachObjectToTransfrom(gameObject.transform, collision.gameObject);

            RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

            if (rigidbodyManager)
            {
                rigidbodyManager.OnPlatfromEnter();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!GeneralFunctions.IsObjectPlayer(collision.gameObject))
        {
            GeneralFunctions.DetachFromParent(collision.gameObject);

            RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

            if (rigidbodyManager)
            {
                rigidbodyManager.OnPlatfromExit();
            }
        }
    }
}
