﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a function library that contains useful functions for gameplay management
/// </summary>
public class GeneralFunctions
{
    /// <summary>
    ///  will check to see if the given object has a Rigidbody2D and if it does will see if it's velocity is greater than zero
    /// </summary>
    /// <param name="objectToTest"></param>
    /// <returns>True if the given object is moving false if not</returns>
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
    /// <summary>
    ///  Will rotate the object the opposite way it's currently facing
    /// </summary>
    /// <param name="objectToFlip"></param>
    public static void FlipObject(GameObject objectToFlip)
    {
        objectToFlip.transform.Rotate(0f, 180f, 0f);
    }
    /// <summary>
    ///  Gets the directional vector of an angle 
    /// </summary>
    /// <param name="angle"></param>
    /// <returns>A vector2 that is pointing a direction</returns>
    public static Vector2 GetDirectionVector2DFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
    /// <summary>
    ///  Get the distance between to vectors
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns>The distance between the two given vectors</returns>
    public static Vector2 GetDistanceBetweenVectors(Vector2 vector1, Vector2 vector2)
    {
        return vector1 - vector2;
    }
    /// <summary>
    /// Gets the directional vector of 2 vectors 
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns>A vector2 that is pointing a direction</returns>
    public static Vector2 GetDirectionVectroFrom2Vectors(Vector2 position1, Vector2 position2)
    {
        return position1 - position2.normalized;
    }
    /// <summary>
    ///  Rotate rigid body to a specified point at a specified speed
    /// </summary>
    /// <param name="target">Target location</param>
    /// <param name="rigidBody">RigidBody to rotate</param>
    /// <param name="speed">Speed to rotate at</param>
    /// <param name="gameObject">Game object the rigidBody is on</param>
    public static void RotateRigidBody(Vector2 target, Rigidbody2D rigidBody, float speed, GameObject gameObject)
    {
        if (rigidBody)
        {
            Vector2 direction = (Vector2)target - rigidBody.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, gameObject.transform.up).z;

            rigidBody.angularVelocity = rotateAmount * speed;
        }
        else
        {
            Debug.LogError(gameObject.ToString() + "didn't have a a Rigidbody2D was unable to rotate");
        }
    }
    /// <summary>
    /// Checks to see if position 1 is above position 2
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns>A bool that determines if position 1 is above or below position 2</returns>
    public static bool IsObjectAbove(Vector2 position1, Vector2 position2)
    {
        return (position1.normalized.y >= position2.normalized.y) ? true : false;
    }
    /// <summary>
    /// Will rotate a given object to face a given vector
    /// </summary>
    /// <param name="currentLocation">Current objects location</param>
    /// <param name="LookAt">The look at location</param>
    /// <param name="objectToRotate">Object to rotate</param>
    public static void LookAt2D(Vector2 currentLocation, Vector2 LookAt, GameObject objectToRotate)
    {
        var dir = currentLocation - LookAt;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        objectToRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// Spawns a leech and attaches it to the given transform then applies the leeching debuff to player
    /// </summary>
    /// <param name="auraManager">The aura manager to spawn the leech debuff on</param>
    /// <param name="attachedLeech">The leech to attach</param>
    /// <param name="transform">Transform of the parent object</param>
    /// <param name="health">Incoming leech's health</param>
    /// <param name="player">Player reference</param>
    /// <returns>The spawned leech attached to the player</returns>
    public static AttachedLeech SpawnLeechAttach(AuraManager auraManager, AttachedLeech attachedLeech, Transform transform, float health,  GameObject player)
    {
        AttachedLeech localLeech = GameObject.Instantiate(attachedLeech, transform.position, Quaternion.identity);

        localLeech.transform.parent = transform;

        localLeech.OnLeechSpawn(health, auraManager, player);

        return localLeech;
    }
    /// <summary>
    ///  Finds a point to attach a leech to by look for the point by tag
    /// </summary>
    /// <param name="tag">The leech collision tag</param>
    /// <returns>The transfrom of the found leeh attach point</returns>
    public static Transform GetLeechAttachPointByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).transform;
    }
    /// <summary>
    /// Checks to see if leech is already attached on the given tag
    /// </summary>
    /// <param name="tag">the collision tag the leech is attaching to</param>
    /// <returns>A bool that determines if a leech can attach to a given point</returns>
    public static bool CanLeechAttach(string tag)
    {
        return (GameObject.FindGameObjectWithTag(tag).transform.childCount <= 0) ? true : false;
    }
    /// <summary>
    ///  Checks to see if leech that is trying to attach to player is current dead
    /// </summary>
    /// <param name="leech">The leech to check</param>
    /// <returns>A bool that determines if the given leech is dead</returns>
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
    /// <summary>
    /// Calls the construct health component on the given game object
    /// </summary>
    /// <param name="gameObject">Game object with the health component</param>
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
    /// <summary>
    ///  Gets the targets health component then heals the target
    /// </summary>
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
    /// <summary>
    /// Pauses the game
    /// </summary>
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// Resumes the game
    /// </summary>
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
    /// <summary>
    /// Creates random Vector 2 coordinates
    /// </summary>
    /// <param name="minX">minimum amount on the x coordinate</param>
    /// <param name="maxX">maximum amount on the x coordinate</param>
    /// <param name="minY">minimum amount on the y coordinate</param>
    /// <param name="maxY">maximum amount on the y coordinate</param>
    /// <returns>A a random vector 2</returns>
    public static Vector2 CreateRandomVector2(float minX, float maxX, float minY, float maxY)
    {
        var randomX = Random.Range(minX, maxX);
        var randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
    /// <summary>
    /// Creates a random Vector 2 but only on the Y axis
    /// </summary>
    /// <param name="minY">minimum amount on the y coordinate</param>
    /// <param name="maxY">>maximum amount on the y coordinate</param>
    /// <returns>A vector2 with a random y coordinate</returns>
    public static Vector2 CreateRandomVector2OnlyY(float minY, float maxY)
    {
        var randomY = Random.Range(minY, maxY);

        return new Vector2(0, randomY);
    }
    /// <summary>
    /// Creates a random Vector 2 but only on the X axis
    /// </summary>
    /// <param name="minX">minimum amount on the x coordinate</param>
    /// <param name="maxX">maximum amount on the x coordinate</param>
    /// <returns>A vector2 with a random x coordinate</returns>
    public static Vector2 CreateRandomVector2OnlyX(float minX, float maxX)
    {
        var randomX = Random.Range(minX, maxX);

        return new Vector2(randomX, 0);
    }
    /// <summary>
    /// Finds all Gameobjects on the specified layer
    /// </summary>
    /// <returns>An array of Gameobjects</returns>
    public static GameObject[] GetAllObjectsInLayer(string layerName)
    {
        var objectArray = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var objectList = new List<GameObject>();

        for (int index = 0; index < objectArray.Length; index++)
        {
            if (objectArray[index].layer == LayerMask.NameToLayer(layerName))
            {
                objectList.Add(objectArray[index]);
            }
        }
        if (objectList.Count == 0)
        {
            return null;
        }

        return objectList.ToArray();
    }
    /// <summary>
    /// Finds all gameplay ID scripts in scene
    /// </summary>
    /// <returns>An array of all gameplay ID components</returns>
    public static GameplayObjectID[] GetAllGameplayIDObjects()
    {
        var objects = GameObject.FindObjectsOfType<GameplayObjectID>();

        return objects;
    }
    /// <summary>
    /// Finds the specific GameObject with the given id in scene
    /// </summary>
    /// <param name="id">the ID found on the Gameobject</param>
    /// <returns>The found GameObject</returns>
    public static GameObject GetGameObjectByID(int id)
    {
        var objects = GetAllGameplayIDObjects();
        GameObject foundObject = null;

        foreach (GameplayObjectID currentGameObject in objects)
        {
            if (currentGameObject.GetID() == id)
            {
                foundObject = currentGameObject.gameObject;
                break;
            }
            else
            {
                foundObject = null;
                continue;
            }
        }

        if (!foundObject)
        {
            Debug.LogWarning("Failed to find object by ID");
        }

        return foundObject;
    }
    /// <summary>
    ///  Returns the player GameObject
    /// </summary>
    /// <returns>The player Gameobject</returns>
    public static GameObject GetPlayerGameObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
    /// <summary>
    /// Gets the health component from the given object
    /// </summary>
    /// <param name="objectToGet">The object you want to get component on</param>
    /// <returns>The objects health component</returns>
    public static HealthComponent GetGameObjectHealthComponent(GameObject objectToGet)
    {
        return objectToGet.GetComponent<HealthComponent>();
    }
    /// <summary>
    ///  Gets the gameplay manager and generates an ID
    /// </summary>
    /// <returns>A random int that will be unique to this object</returns>
    public static int GenID()
    {
        var manager = GameObject.FindGameObjectWithTag("Gameplay Manager");

        return manager.GetComponent<GameplayManager>().GenID();
    }
    /// <summary>
    /// Gets all gameplay IDs from Gameplay Manager
    /// </summary>
    /// <returns>A list of integers</returns>
    public static List<int> GetAllIDs()
    {
        var manager = GameObject.FindGameObjectWithTag("Gameplay Manager");

        return manager.GetComponent<GameplayManager>().GetAllIDs();
    }
    /// <summary>
    /// Will parent the given object to provided transform
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="objectToAttach"></param>
    public static void AttachObjectToTransfrom(Transform transform, GameObject objectToAttach)
    {
        objectToAttach.transform.parent = transform;
    }
    /// <summary>
    /// Will detach the given object from its parent
    /// </summary>
    /// <param name="objectToDetach"></param>
    public static void DetachFromParent(GameObject objectToDetach)
    {
        objectToDetach.transform.parent = null;
    }
    /// <summary>
    /// Checks to see if the given object is the player
    /// </summary>
    /// <param name="objectToTest"></param>
    public static bool IsObjectPlayer(GameObject objectToTest)
    {
        return objectToTest.CompareTag("Player");
    }

    public static bool IsObjectOnLayer(string layer, GameObject gameObject)
    {
        bool localBool = gameObject.layer == LayerMask.NameToLayer(layer);

        return localBool;
    }
}