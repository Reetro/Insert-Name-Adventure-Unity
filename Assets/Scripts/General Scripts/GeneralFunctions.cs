using System.Collections.Generic;
using UnityEngine;
using AuraSystem;
using PlayerUI;
using GameplayManagement;
using EnemyCharacter;
using AuraSystem.Effects;
using PlayerCharacter.Controller;
using System;
using UnityEditor;

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
    /// <returns>A vector2 that is pointing in a direction</returns>
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
    /// <returns>A vector2 that is pointing in a direction</returns>
    public static Vector2 GetDirectionVectorFrom2Vectors(Vector2 position1, Vector2 position2)
    {
        var direction = position1 - position2;

        return direction.normalized;
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
    /// Spawns a leech and attaches it to the given transform then applies the leeching debuff to the player
    /// </summary>
    /// <param name="auraManager">The aura manager to spawn the leech debuff on</param>
    /// <param name="attachedLeech">The leech to attach</param>
    /// <param name="transform">Transform of the parent object</param>
    /// <param name="health">Incoming leech's health</param>
    /// <param name="player">Player reference</param>
    /// <returns>The spawned leech attached to the player</returns>
    public static AttachedLeech SpawnLeechAttach(AttachedLeech attachedLeech, Transform transform, float health, GameObject player)
    {
        AttachedLeech localLeech = GameObject.Instantiate(attachedLeech, transform.position, Quaternion.identity);

        localLeech.transform.parent = transform;

        localLeech.OnLeechSpawn(health, player);

        return localLeech;
    }
    /// <summary>
    ///  Finds a point to attach a leech to by look for the point by tag
    /// </summary>
    /// <param name="tag">The leech collision tag</param>
    /// <returns>The transfrom of the found leech attach point</returns>
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
    /// Calls the construct health component function on the given game object
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
    ///  Gets the targets health component then damages to the target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <param name="showText"></param>
    public static void DamageTarget(GameObject target, float amount, bool showText)
    {
        if (IsObjectOnLayer(GetGameplayManager().whatCanBeDamaged, target))
        {
            var health = target.GetComponent<HealthComponent>();

            if (health)
            {
                health.ProccessDamage(amount, showText, GetGameplayManager().whatCanBeDamaged);
            }
            else
            {
                Debug.LogError("Failed to damage " + target.name.ToString() + " does not have a health component");
            }
        }
    }
    /// <summary>
    /// Will instantly kill the given target
    /// </summary>
    /// <param name="target"></param>
    public static void KillTarget(GameObject target)
    {
        if (IsObjectOnLayer(GetGameplayManager().whatCanBeDamaged, target))
        {
            var health = target.GetComponent<HealthComponent>();

            if (health)
            {
                health.ProccessDamage(1000000000, false, GetGameplayManager().whatCanBeDamaged);
            }
            else
            {
                Debug.LogError("Failed to kill " + target.name.ToString() + " does not have a health component");
            }
        }
    }
    /// <summary>
    /// Checks to see if the player Gameobject is currently dead
    /// </summary>
    /// <returns>A bool that determains if the player is dead</returns>
    public static bool IsPlayerDead()
    {
        return IsObjectDead(GetPlayerGameObject());
    }
    /// <summary>
    /// Checks to see if the given Gameobject is currently dead
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns>A bool that determines if the object is dead</returns>
    public static bool IsObjectDead(GameObject gameObject)
    {
        var health = gameObject.GetComponent<HealthComponent>();

        if (health)
        {
            return health.IsCurrentlyDead;
        }
        else
        {
            Debug.LogError("Failed to check death state on " + gameObject.name.ToString() + " does not have a health component");
            return false;
        }
    }
    /// <summary>
    /// Finds the Gameplay Manager in the current level
    /// </summary>
    public static GameplayManager GetGameplayManager()
    {
        return GameObject.FindGameObjectWithTag("Gameplay Manager").GetComponent<GameplayManager>();
    }
    /// <summary>
    /// Pauses the game by setting time scale to 0
    /// </summary>
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// Resumes the game by setting time scale to 1
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
        var randomX = UnityEngine.Random.Range(minX, maxX);
        var randomY = UnityEngine.Random.Range(minY, maxY);

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
        var randomY = UnityEngine.Random.Range(minY, maxY);

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
        var randomX = UnityEngine.Random.Range(minX, maxX);

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
    ///  Find the Player Gameobject by tag in the current level
    /// </summary>
    /// <returns>The Player Gameobject</returns>
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

        return manager.GetComponent<GameplayManager>().gameIDS;
    }
    /// <summary>
    /// Will parent the given object to the provided transform
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
    /// <summary>
    /// Checks to see if the provided Gameobject is on the provided layer
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="gameObject"></param>
    /// <returns>A bool that determines if the Gameobject was on the layer</returns>
    public static bool IsObjectOnLayer(string layer, GameObject gameObject)
    {
        bool localBool = gameObject.layer == LayerMask.NameToLayer(layer);

        return localBool;
    }
    /// <summary>
    /// Checks to see if the provided Gameobject is on the provided layer
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="gameObject"></param>
    /// <returns>A bool that determines if the Gameobject was on the layer</returns>
    public static bool IsObjectOnLayer(int layer, GameObject gameObject)
    {
        bool localBool = LayerMask.LayerToName(gameObject.layer) == LayerMask.LayerToName(layer);

        return localBool;
    }
    /// <summary>
    /// Checks to see if the provided Gameobject is on the provided layer
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="gameObject"></param>
    /// <returns>A bool that determines if the Gameobject was on the layer</returns>
    public static bool IsObjectOnLayer(LayerMask mask, GameObject gameObject)
    {
        bool localBool = false;

        if (mask == (mask | (1 << gameObject.layer)))
        {
            localBool = true;
        }

        return localBool;
    }
    /// <summary>
    /// Apply a buff to the given target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffToApply"></param>
    /// <param name="createIcon"></param>
    /// <param name="refresh"></param>
    /// <param name="stack"></param>
    public static BuffEffect ApplyBuffToTarget(GameObject target, ScriptableBuff buffToApply, bool createIcon)
    {
        var auraManager = target.GetComponent<AuraManager>();

        if (auraManager)
        {
            return auraManager.ApplyBuff(target, buffToApply, createIcon);
        }
        else
        {
            Debug.LogError("Failed to apply buff to " + target.gameObject.name + " did not have a aura manager component");

            return null;
        }
    }
    /// <summary>
    /// Apply a debuff to the given target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffToApply"></param>
    /// <param name="createIcon"></param>
    /// <param name="refresh"></param>
    /// <param name="stack"></param>
    public static DebuffEffect ApplyDebuffToTarget(GameObject target, ScriptableDebuff debuffToApply, bool createIcon)
    {
        var auraManager = target.GetComponent<AuraManager>();

        if (auraManager)
        {
            return auraManager.ApplyDebuff(target, debuffToApply, createIcon);
        }
        else
        {
            Debug.LogError("Failed to apply debuff to " + target.gameObject.name + " did not have a aura manager component");

            return null;
        }
    }
    /// <summary>
    /// Gets the targets rigidbody and applies force
    /// </summary>
    /// <param name="target"></param>
    /// <param name="force"></param>
    public static void ApplyKnockback(GameObject target, Vector2 force)
    {
        var rigidbody2D = target.GetComponent<Rigidbody2D>();

        if (rigidbody2D)
        {
            rigidbody2D.AddForce(force);
        }
        else
        {
            Debug.LogError("Failed to ApplyKnockback " + target.name.ToString() + " does not have a Rigidbody2D component");
        }
    }
    /// <summary>
    /// Find the Player UI Manager in the current scene
    /// </summary>
    /// <returns></returns>
    public static PlayerUIManager GetPlayerUIManager()
    {
        return GameObject.FindGameObjectWithTag("Player Hud Canvas").GetComponent<PlayerUIManager>();
    }
    /// <summary>
    /// Checks to see if the given object is on the player character
    /// </summary>
    /// <param name="objectToTest"></param>
    /// <returns></returns>
    public static bool IsObjectOnPlayer(GameObject objectToTest)
    {
        var player = objectToTest.GetComponentInParent<PlayerController>();

        return player;
    }
    /// <summary>
    /// Remove an item from an array at a given index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arr"></param>
    /// <param name="index"></param>
    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }
}