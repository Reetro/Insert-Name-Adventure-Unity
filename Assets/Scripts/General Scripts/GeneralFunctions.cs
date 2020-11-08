﻿using System.Collections.Generic;
using UnityEngine;
using StatusEffects;
using PlayerUI;
using GameplayManagement;
using EnemyCharacter;
using StatusEffects.Effects;
using PlayerCharacter.Controller;
using PlayerCharacter.GameSaving;
using LevelObjects.SceneLoading;
using GameplayManagement.Assets;

/// <summary>
/// This is a function library that contains useful functions for gameplay management
/// </summary>
public class GeneralFunctions
{
    #region Vector Functions
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
    /// <summary>
    /// Checks to see if the given number is negative
    /// </summary>
    /// <param name="number"></param>
    public static bool IsNumberNegative(float number)
    {
        return number < 0;
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
    /// Gets the directional vector of 2 transforms 
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns>A vector2 that is pointing in a direction</returns>
    public static Vector2 GetDirectionVectorFrom2Vectors(Transform transfrom1, Transform transfrom2)
    {
        var direction = transfrom1.position - transfrom2.position;

        return direction.normalized;
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

    public static bool IsObjectLeftOrRight(Transform currentTransform, Transform otherTransform)
    {
        var realativePoint = currentTransform.InverseTransformPoint(otherTransform.position);

        if (realativePoint.x < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    /// Gets the current direction of the X axis
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns>A Vector2</returns>
    public static Vector2 GetFaceingDirectionX(GameObject gameObject)
    {
        return gameObject.transform.rotation * Vector2.right;
    }
    /// <summary>
    /// Gets the current direction of the Y axis
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns>A Vector2</returns>
    public static Vector2 GetFaceingDirectionY(GameObject gameObject)
    {
        return gameObject.transform.rotation * Vector2.up;
    }
    /// <summary>
    /// Get a point X units behind the given transform
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <param name="distanceBehind"></param>
    /// <returns></returns>
    public static Vector2 GetPointBehind(Transform targetTransform, float distanceBehind)
    {
        return targetTransform.position - (targetTransform.right * distanceBehind);
    }
    #endregion

    #region Rotation Functions
    /// <summary>
    /// Will calculate the needed angle to make the current location look at the look at vector
    /// </summary>
    /// <param name="currentLocation">Current objects location</param>
    /// <param name="LookAt">The look at location</param>
    /// <param name="objectToRotate">Object to rotate</param>
    public static Quaternion LookAt2D(Vector2 currentLocation, Vector2 LookAt)
    {
        var dir = currentLocation - LookAt;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return Quaternion.AngleAxis(angle, Vector3.forward);
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
    /// Gets the given object current angle
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns>A float</returns>
    public static float GetObjectEulerAngle(GameObject gameObject)
    {
        var angle = gameObject.transform.localEulerAngles.z;

        if (angle.Equals(270))
        {
            angle = -90;
        }

        return angle;
    }
    #endregion

    #region Leech Functions
    /// <summary>
    /// Spawns a leech and attaches it to the given transform then applies the leeching debuff to the target
    /// </summary>
    /// <param name="auraManager">The aura manager to spawn the leech debuff on</param>
    /// <param name="attachedLeech">The leech to attach</param>
    /// <param name="transform">Transform of the parent object</param>
    /// <param name="health">Incoming leech's health</param>
    /// <param name="player">Player reference</param>
    /// <returns>The spawned leech attached to the player</returns>
    public static AttachedLeech SpawnLeechAttach(AttachedLeech attachedLeech, Transform transform, float health, GameObject target, int leechID)
    {
        AttachedLeech localLeech = GameObject.Instantiate(attachedLeech, transform.position, Quaternion.identity);

        localLeech.transform.parent = transform;

        localLeech.OnLeechSpawn(health, target, leechID);

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
    /// Checks to see if leech is already attached on the given tag and checks to see if a new scene is currently loading then as one final check it sees if an attached leech with the given ID is already attached to the player
    /// </summary>
    /// <param name="tag">the collision tag the leech is attaching to</param>
    /// <param name="id"></param>
    /// <returns>A bool that determines if a leech can attach to a given point</returns>
    public static bool CanLeechAttach(string tag, int id)
    {
        return (GameObject.FindGameObjectWithTag(tag).transform.childCount <= 0 && !GameObject.FindObjectOfType<PlayerState>().IsLoadingScene && !DoesAttachedLeechExist(id)) ? true : false;
    }
    /// <summary>
    /// Loops through all attached leeches in the scene and checks to see if one with the same ID exists
    /// </summary>
    /// <param name="id"></param>
    public static bool DoesAttachedLeechExist(int id)
    {
        var attachedLeeches = GetAllAttachedLeeches();
        bool localBool = false;

        foreach (AttachedLeech attachedLeech in attachedLeeches)
        {
            if (attachedLeech)
            {
                if (attachedLeech.MyID == id)
                {
                    localBool = true;
                    break;
                }
            }
        }

        return localBool;
    }
    /// <summary>
    /// Gets all attached leeches in the current level
    /// </summary>
    /// <returns>A list of attached leeches</returns>
    public static List<AttachedLeech> GetAllAttachedLeeches()
    {
        var leechObjects = GetAllObjectsInLayer("Attached Leech");
        List<AttachedLeech> attachedLeeches = new List<AttachedLeech>();

        foreach (GameObject gameObject in leechObjects)
        {
            var attachedLeech = gameObject.GetComponent<AttachedLeech>();

            if (attachedLeech)
            {
                attachedLeeches.Add(attachedLeech);
            }
        }

        return attachedLeeches;
    }
    #endregion

    #region General Health Functions
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
    /// Gets the health component from the given object
    /// </summary>
    /// <param name="objectToGet">The object you want to get component on</param>
    /// <returns>The objects health component</returns>
    public static HealthComponent GetGameObjectHealthComponent(GameObject objectToGet)
    {
        return objectToGet.GetComponent<HealthComponent>();
    }
    /// <summary>
    /// Gets the health component from the given Transform
    /// </summary>
    /// <param name="objectToGet">The object you want to get component on</param>
    /// <returns>The objects health component</returns>
    public static HealthComponent GetGameObjectHealthComponent(Transform objectTransform)
    {
        return objectTransform.GetComponent<HealthComponent>();
    }
    #endregion

    #region Gameplay Management Functions

    #region Game pausing functions
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
    #endregion

    #region Transform functions
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
    /// Will parent the given object to the provided transform forces Gameobject to keep it's current rotation
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="objectToAttach"></param>
    public static void AttachObjectToTransfrom(Transform transform, GameObject objectToAttach, Quaternion rotation)
    {
        objectToAttach.transform.parent = transform;

        objectToAttach.transform.rotation = rotation;
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
    #endregion

    #region Layer Functions
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
    #endregion

    #region Game action functions
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
    ///  Apply the given amount of damage to the given target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <param name="showText"></param>
    public static void DamageTarget(GameObject target, float amount, bool showText, GameObject damageDealer)
    {
        if (IsObjectOnLayer(GameAssets.GlobalManager.whatCanBeDamaged, target))
        {
            var health = target.GetComponent<HealthComponent>();

            if (health)
            {
                health.ProccessDamage(amount, showText, GameAssets.GlobalManager.whatCanBeDamaged);
            }
            else
            {
                Debug.LogError(damageDealer.name.ToString() + " Failed to damage " + target.name.ToString() + " does not have a health component");
            }
        }
    }
    /// <summary>
    /// Will instantly kill the given target
    /// </summary>
    /// <param name="target"></param>
    public static void KillTarget(GameObject target)
    {
        if (IsObjectOnLayer(GameAssets.GlobalManager.whatCanBeDamaged, target))
        {
            var health = target.GetComponent<HealthComponent>();

            if (health)
            {
                health.ProccessDamage(1000000000, false, GameAssets.GlobalManager.whatCanBeDamaged);
            }
            else
            {
                Debug.LogError("Failed to kill " + target.name.ToString() + " does not have a health component");
            }
        }
    }
    /// <summary>
    /// Apply a status effect to the given target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffToApply"></param>
    /// <param name="createIcon"></param>
    /// <param name="refresh"></param>
    /// <param name="stack"></param>
    public static StatusEffect ApplyStatusEffectToTarget(GameObject target, ScriptableStatusEffect StatusEffectToApply)
    {
        var auraManager = target.GetComponent<AuraManager>();

        if (auraManager)
        {
            return auraManager.ApplyEffect(target, StatusEffectToApply);
        }
        else
        {
            Debug.LogError("Failed to apply buff to " + target.gameObject.name + " did not have a aura manager component");

            return null;
        }
    }
    /// <summary>
    /// Gets the targets rigidbody and applies force
    /// </summary>
    /// <param name="target"></param>
    /// <param name="force"></param>
    public static void ApplyKnockback(GameObject target, Vector2 force, ForceMode2D forceMode2D)
    {
        var rigidbody2D = target.GetComponent<Rigidbody2D>();

        if (rigidbody2D)
        {
            rigidbody2D.AddForce(force, forceMode2D);
        }
        else
        {
            Debug.LogError("Failed to ApplyKnockback " + target.name.ToString() + " does not have a Rigidbody2D component");
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
    #endregion

    #region Gameobject Functions
    /// <summary>
    /// Get the PlayerUIManager cached in Game Assets
    /// </summary>
    public static PlayerUIManager GetPlayerUIManager()
    {
        return GameAssets.PlayerHUDManager;
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
    /// Will get the player controller then stun the player for the given time
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="stunTime"></param>
    public static void StunPlayer(float stunTime)
    {
        var playerObject = GetPlayerGameObject();

        if (playerObject)
        {
            var playerController = playerObject.GetComponent<PlayerController>();

            if (playerController)
            {
                playerController.ApplyStun(stunTime);
            }
            else
            {
                Debug.LogError("Failed to stun " + playerObject.name + " is not the player object");
            }
        }
        else
        {
            Debug.LogError("Failed to stun " + playerObject.name + " was not valid");
        }
    }
    /// <summary>
    /// Get the PlayerState cached in Game Assets
    /// </summary>
    public static PlayerState GetPlayerState()
    {
        return GameAssets.PlayerCurrentState;
    }
    /// <summary>
    /// Get the LevelLoader cached in Game Assets
    /// </summary>
    public static LevelLoader GetLevelLoader()
    {
        return GameAssets.CurrentLevelLoader;
    }
    /// <summary>
    ///  Get the Player Gameobject cached in Game Assets
    /// </summary>
    /// <returns>The Player Gameobject</returns>
    public static GameObject GetPlayerGameObject()
    {
        return GameAssets.PlayerGameObject;
    }
    /// <summary>
    /// Get the Player Gameobject in the current level and get the player legs component
    /// </summary>
    /// <returns>The player legs component</returns>
    public static PlayerLegs GetPlayerLegs()
    {
        return GameAssets.PlayerObjectLegs;
    }
    /// <summary>
    /// Get the Player Gameobject in the current level and get the player spear component
    /// </summary>
    /// <returns>The player spear component</returns>
    public static PlayerSpear GetPlayerSpear()
    {
        return GetPlayerGameObject().transform.GetChild(1).GetComponent<PlayerSpear>();
    }
    /// <summary>
    /// Check to see if the player is touching the ground
    /// </summary>
    public static bool IsPlayerTouchingGround()
    {
        return GetPlayerLegs().IsGrounded;
    }
    /// <summary>
    ///  Gets the gameplay manager and generates an ID
    /// </summary>
    /// <returns>A random int that will be unique to this object</returns>
    public static int GenID()
    {
        return GameAssets.GlobalManager.GetComponent<GameplayManager>().GenID();
    }
    /// <summary>
    /// Gets the current gameobject under the player legs
    /// </summary>
    /// <returns></returns>
    public static GameObject GetGameObjectUnderPlayer()
    {
        return GameAssets.PlayerObjectLegs.ObjectUnderLeg;
    }
    #endregion

    #region Array Functions
    /// <summary>
    /// Loops through the given array and checks to see if the given object exist in the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="objectToTest"></param>
    public static bool DoesItemExistInArray<T>(T[] array, T objectToTest)
    {
        foreach (T item in array)
        {
            if (item != null)
            {
                if (item.Equals(objectToTest))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Loops through a bool array to see if all items are true
    /// </summary>
    /// <param name="array"></param>
    public static bool IsBoolArrayTrue(bool[] array)
    {
        foreach (bool index in array)
        {
            if (!index)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Will find the given items index in the given array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Array"></param>
    /// <param name="ObjectToFind"></param>
    public static int FindItemIndex<T>(T[] Array, T ObjectToFind)
    {
        if (ObjectToFind != null)
        {
            for (int index = 0; index < Array.Length; index++)
            {
                var item = Array[index];

                if (item != null)
                {
                    if (item.Equals(ObjectToFind))
                    {
                        return index;
                    }
                }
            }

            Debug.LogError("Failed to find item index in " + Array);

            return 0;
        }
        else
        {
            Debug.LogError("Failed to find item index in " + Array + " item was null");

            return 0;
        }
    }
    #endregion

    #region Save Game Functions
    /// <summary>
    /// Save the game to the given slot
    /// </summary>
    /// <param name="slot"></param>
    public static void SaveGameToSlot(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            state.SaveGameToSlot(slot);
        }
        else
        {
            Debug.LogError("Failed to save game unable to find Player State");
        }
    }
    /// <summary>
    /// Load game data in the given slot
    /// </summary>
    /// <param name="slot"></param>
    public static void LoadGameFromSlot(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            state.LoadGame(slot);
        }
        else
        {
            Debug.LogError("Failed to load game unable to find Player State");
        }
    }
    /// <summary>
    /// Finds the current active save file and loads it
    /// </summary>
    public static void LoadActiveSave()
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            state.LoadActiveSave();
        }
        else
        {
            Debug.LogError("Failed to load active game unable to find Player State");
        }
    }
    /// <summary>
    /// Delete saved game in the given slot
    /// </summary>
    /// <param name="slot"></param>
    public static void DeleteGameInSlot(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            state.DeleteSaveGame(slot);
        }
        else
        {
            Debug.LogError("Failed to delete save in slot " + slot + " unable to find Player State");
        }
    }
    /// <summary>
    /// Checks to see if any save slot is active
    /// </summary>
    public static bool IsAnySaveSlotActive()
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            return state.IsAnySlotActive();
        }
        else
        {
            Debug.LogError("Failed to check IsAnySaveSlotActive unable to find Player State");
            return false;
        }
    }
    /// <summary>
    /// Checks to see if the given slot is active
    /// </summary>
    /// <param name="slot"></param>
    public static bool IsSlotActive(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            return state.IsSlotActive(slot);
        }
        else
        {
            Debug.LogError("Failed to check IsSlotActive unable to find Player State");
            return false;
        }
    }
    /// <summary>
    /// Checks to see if there is a save in the given slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public static bool DoesSaveExistInSlot(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            return state.DoesSaveExistInSlot(slot);
        }
        else
        {
            Debug.LogError("Failed to check DoesSaveExistInSlot unable to find Player State");
            return false;
        }
    }
    /// <summary>
    /// Loads the 1st level in the game and creates a new save file
    /// </summary>
    /// <param name="slot"></param>
    public static void CreateNewSave(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            state.StartNewGame(slot);
        }
        else
        {
            Debug.LogError("Unable to create new save unable to find Player State");
        }
    }
    /// <summary>
    /// Set the given slot to be the active game slot
    /// </summary>
    /// <param name="slot"></param>
    public static void SetActiveSlot(int slot)
    {
        GameAssets.UpdateReferences();

        var state = GetPlayerState();

        if (state)
        {
            state.SetActiveSlot(slot);
        }
        else
        {
            Debug.LogError("Unable to SetActiveSlot unable to find Player State");
        }
    }
    #endregion
    #endregion

    #region Component Functions
    /// <summary>
    /// Gets the eye component on the give object and fires a raycast from the objects eyes
    /// </summary>
    /// <returns>The hit information</returns>
    public static RaycastHit2D TraceFromEyes(GameObject objectTraceParent)
    {
        if (objectTraceParent)
        {
            var eyes = objectTraceParent.GetComponent<EyeTrace>();

            if (eyes)
            {
                return eyes.TraceFromEyes();
            }
            else
            {
                Debug.LogError("Failed to trace from " + objectTraceParent.name + " had no eye component");
                return new RaycastHit2D();
            }
        }
        else
        {
            Debug.LogError("Failed to trace from " + objectTraceParent.name + " was not valid");
            return new RaycastHit2D();
        }
    }

    /// <summary>
    /// Gets the total width of the given sprite
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <returns>A float</returns>
    public static float GetSpriteWidth(SpriteRenderer spriteRenderer)
    {
        return spriteRenderer.bounds.extents.x;
    }
    /// <summary>
    /// Gets the total height of the given sprite
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <returns>A float</returns>
    public static float GetSpriteHeight(SpriteRenderer spriteRenderer)
    {
        return spriteRenderer.bounds.extents.y;
    }
    #endregion
}

#region Extension Methods

public static class Rigidbody2DExt
{
    /// <summary>
    /// Applies a force to a rigidbody that simulates explosion effects
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="explosionForce"></param>
    /// <param name="explosionPosition"></param>
    /// <param name="explosionRadius"></param>
    /// <param name="upwardsModifier"></param>
    /// <param name="mode"></param>
    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
    }
}

public static class Extension
{
    /// <summary>
    /// Will reverse the given list order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    public static IEnumerable<T> FastReverse<T>(this IList<T> items)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            yield return items[i];
        }
    }
}
#endregion