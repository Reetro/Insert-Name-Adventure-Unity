﻿using UnityEngine;

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
    // Checks to see if the given number is negative
    public static bool IsNumberNegative(float number)
    {
        return number < 0;
    }
    // Will rotate the object the opposite way it's currently facing
    public static void FlipObject(GameObject objectToFlip)
    {
        objectToFlip.transform.Rotate(0f, 180f, 0f);
    }
    // Gets the directional vector of an angle 
    public static Vector2 GetDirectionVector2DFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
    // Get the distance between to vectors
    public static Vector2 GetDistanceBetweenVectors(Vector2 vector1, Vector2 vector2)
    {
        return vector1 - vector2;
    }
    // Gets the directional vector of 2 vectors 
    public static Vector2 GetDirectionVectroFrom2Vectors(Vector2 position1, Vector2 position2)
    {
        return position1 - position2.normalized;
    }

    // Rotate rigid body to a specified point at a specified speed
    public static void RotateRigidBody(Vector2 target, Rigidbody2D rigidBody, float speed, GameObject gameObject)
    {
        Vector2 direction = (Vector2)target - rigidBody.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, gameObject.transform.up).z;

        rigidBody.angularVelocity = rotateAmount * speed;
    }
    // Checks to see if position 1 is above position 2
    public static bool IsObjectAbove(Vector2 position1, Vector2 position2)
    {
        return (position1.normalized.y >= position2.normalized.y) ? true : false;
    }
    public static void LookAt2D(Vector2 currentLocation, Vector2 LookAt, GameObject objectToRotate)
    {
        var dir = currentLocation - LookAt;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        objectToRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    // Spawns a leech and attaches it to the given transform then applies the leeching debuff to player
    public static AttachedLeech SpawnLeechAttach(AuraManager auraManager, AttachedLeech attachedLeech, Transform transform, float health,  GameObject player)
    {
        AttachedLeech localLeech = GameObject.Instantiate(attachedLeech, transform.position, Quaternion.identity);

        localLeech.transform.parent = transform;

        localLeech.OnLeechSpawn(health, auraManager, player);

        return localLeech;
    }
    // Finds a point to attach a leech to by look for the point by tag
    public static Transform GetLeechAttachPointByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).transform;
    }
    // Checks to see if leech is already attached on the given tag
    public static bool CanLeechAttach(string tag)
    {
        return (GameObject.FindGameObjectWithTag(tag).transform.childCount <= 0) ? true : false;
    }
    // Checks to see if leech that is trying to attach to player is current dead
    public static bool IsLeechDead(GameObject leech)
    {
        var leechHelthComp = leech.GetComponent<HealthComponent>();

        if (leechHelthComp)
        {
            return leechHelthComp.GetIsDead();
        }
        else
        {
            Debug.LogError("Failed to check leech " + leech.name.ToString() + " did not have a health component");
            return false;
        }
    }
    // Calls the construct health component on the given game object
    public static void ConstructHPComponent(GameObject gameObject)
    {
        var health = gameObject.GetComponent<HealthComponent>();

        if (health)
        {
            health.ConstructHealthComponent();
        }
        else
        {
            Debug.LogError("Failed to construct Health Component on " + gameObject.name.ToString() + " does not have a health component");
        }
    }
    // Gets the targets health component then heals target
    public static void HealTarget(GameObject target, float amount)
    {
        var health = target.GetComponent<HealthComponent>();

        if (health)
        {
            health.AddHealth(amount);
        }
        else
        {
            Debug.LogError("Failed to heal " + target.name.ToString() + " does not have a health component");
        }
    }
    // Pauses the game
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    // Resumes the game
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
    // Creates random Vector 2 coordinates
    public static Vector2 CreateRandomVector2(float minX, float maxX, float minY, float maxY)
    {
        var randomX = Random.Range(minX, maxX);
        var randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
    // Creates a random Vector 2 but only on the Y axis
    public static Vector2 CreateRandomVector2OnlyY(float minY, float maxY)
    {
        var randomY = Random.Range(minY, maxY);

        return new Vector2(0, randomY);
    }
    // Creates a random Vector 2 but only on the X axis
    public static Vector2 CreateRandomVector2OnlyX(float minX, float maxX)
    {
        var randomX = Random.Range(minX, maxX);

        return new Vector2(randomX, 0);
    }
    // Finds all the enemies in the current scene
    public static GameObject[] GetAllEnimesInScene()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");

        return objects;
    }
    // Gets a given enemy object by it's id
    public static GameObject GetEnemyByID(GameObject idObject)
    {
        return idObject.transform.parent.gameObject;
    }
}