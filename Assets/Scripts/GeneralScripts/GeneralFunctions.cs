﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnemyScripts.LeechScripts;
using GameplayScripts;
using GameplayScripts.LevelScripts;
using GeneralScripts.GeneralComponents;
using GeneralScripts.UI;
using PlayerScripts.PlayerCombat;
using PlayerScripts.PlayerControls;
using Spells;
using StatusEffects;
using StatusEffects.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralScripts
{
    /// <summary>
    /// This is a function library that contains useful functions for gameplay management
    /// </summary>
    public class GeneralFunctions : MonoBehaviour
    {
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int Mode = Shader.PropertyToID("_Mode");

        #region Vector Functions
        /// <summary>
        ///  will check to see if the given object has a Rigidbody2D and if it does will see if it's velocity is greater than zero
        /// </summary>
        /// <param name="objectToTest"></param>
        /// <returns>True if the given object is moving false if not</returns>
        public static bool IsObjectMovingHorizontally(GameObject objectToTest)
        {
            var rigidBody = objectToTest.GetComponent<Rigidbody2D>();

            if (rigidBody)
            {
                return Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
            }
            else
            {
                Debug.LogError(objectToTest + " didn't have a a Rigidbody2D was unable to test it horizontally");
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
        /// <param name="transfrom1"></param>
        /// <param name="transfrom2"></param>
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
            return (position1.normalized.y >= position2.normalized.y);
        }
        /// <summary>
        /// Returns true if otherTransform is left and false if right
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="otherTransform"></param>
        public static bool IsObjectLeftOrRight(Transform currentTransform, Transform otherTransform)
        {
            var relativePoint = currentTransform.InverseTransformPoint(otherTransform.position);

            return relativePoint.x < 0;
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
        /// Gets the current direction of the X axis
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>A Vector2</returns>
        public static Vector2 GetFacingDirectionX(GameObject gameObject)
        {
            return gameObject.transform.rotation * Vector2.right;
        }
        /// <summary>
        /// Gets the current direction of the Y axis
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>A Vector2</returns>
        public static Vector2 GetFacingDirectionY(GameObject gameObject)
        {
            return gameObject.transform.rotation * Vector2.up;
        }
        /// <summary>
        /// Get a point X units behind the given transform
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="distanceBehind"></param>
        /// <returns>A Vector2</returns>
        public static Vector2 GetPointBehind(Transform targetTransform, float distanceBehind)
        {
            return targetTransform.position - (targetTransform.right * distanceBehind);
        }
        /// <summary>
        /// Gets a point X units away from the facing directions
        /// </summary>
        public static Vector2 GetPoint(Vector2 direction, Vector2 startPosition, float distance)
        {
            return startPosition + direction * distance;
        }
        #endregion

        #region Rotation Functions
        /// <summary>
        /// Will calculate the needed angle to make the current location look at the look at vector
        /// </summary>
        /// <param name="currentLocation">Current objects location</param>
        /// <param name="lookAt">The look at location</param>
        public static Quaternion LookAt2D(Vector2 currentLocation, Vector2 lookAt)
        {
            var dir = currentLocation - lookAt;

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

            if (Mathf.Approximately(angle, 270))
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
        /// <param name="attachedLeech">The leech to attach</param>
        /// <param name="transform">Transform of the parent object</param>
        /// <param name="health">Incoming leech's health</param>
        /// <param name="target"></param>
        /// <param name="leechID"></param>
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
            return (GameObject.FindGameObjectWithTag(tag).transform.childCount <= 0 && !PlayerState.IsLoadingScene && !DoesAttachedLeechExist(id));
        }
        /// <summary>
        /// Loops through all attached leeches in the scene and checks to see if one with the same ID exists
        /// </summary>
        /// <param name="id"></param>
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool DoesAttachedLeechExist(int id)
        {
            var attachedLeeches = GetAllAttachedLeeches();

            return attachedLeeches.Where(attachedLeech => attachedLeech).Any(attachedLeech => attachedLeech.MyID == id);
        }
        /// <summary>
        /// Gets all attached leeches in the current level
        /// </summary>
        /// <returns>A list of attached leeches</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static List<AttachedLeech> GetAllAttachedLeeches()
        {
            var leechObjects = GetAllObjectsInLayer("Attached Leech");

            return leechObjects.Select(gameObject => gameObject.GetComponent<AttachedLeech>()).Where(attachedLeech => attachedLeech).ToList();
        }
        #endregion

        #region General Health Functions
        /// <summary>
        /// Calls the construct health component function on the given game object
        /// </summary>
        /// <param name="gameObject">Game object with the health component</param>
        public static void ConstructHpComponent(GameObject gameObject)
        {
            var health = gameObject.GetComponent<HealthComponent>();

            if (health)
            {
                health.ConstructHealthComponent();
            }
            else
            {
                Debug.LogError("Failed to construct Health Component on " + gameObject.name + " does not have a health component");
            }
        }
        /// <summary>
        /// Checks to see if the player Gameobject is currently dead
        /// </summary>
        /// <returns>A bool that determines if the player is dead</returns>
        public static bool IsPlayerDead()
        {
            return IsObjectDead(GetPlayerGameObject());
        }
        // ReSharper disable Unity.PerformanceAnalysis
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
                Debug.LogError("Failed to check death state on " + gameObject.name + " does not have a health component");
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

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Gets the health component from the given Transform
        /// </summary>
        /// <param name="objectTransform"></param>
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
            GameAssets.GlobalManager.IsGamePaused = true;

            GameAssets.GlobalManager.onGamePause.Invoke();

            GetPlayerSpear().DisableSpear();

            Time.timeScale = 0;
        }
        /// <summary>
        /// Resumes the game by setting time scale to 1
        /// </summary>
        public static void ResumeGame()
        {
            Time.timeScale = 1;

            GameAssets.GlobalManager.IsGamePaused = false;

            GetPlayerSpear().EnableSpear();

            GameAssets.GlobalManager.onGameResume.Invoke();
        }
        #endregion

        #region Transform functions

        /// <summary>
        /// Will parent the given object to the provided transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="objectToAttach"></param>
        public static void AttachObjectToTransfrom(Transform transform, GameObject objectToAttach)
        {
            objectToAttach.transform.parent = transform;
        }

        /// <summary>
        /// Will parent the given object to the provided transform forces Gameobject to keep it's current rotation
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="objectToAttach"></param>
        /// <param name="rotation"></param>
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
        /// <param name="mask"></param>
        /// <param name="gameObject"></param>
        /// <returns>A bool that determines if the Gameobject was on the layer</returns>
        public static bool IsObjectOnLayer(LayerMask mask, GameObject gameObject)
        {
            var localBool = mask == (mask | (1 << gameObject.layer));

            return localBool;
        }
        /// <summary>
        /// Finds all Gameobjects on the specified layer
        /// </summary>
        /// <returns>An array of Gameobjects</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static IEnumerable<GameObject> GetAllObjectsInLayer(string layerName)
        {
            var objectArray = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            var objectList = new List<GameObject>();

            if (objectArray != null) objectList.AddRange(objectArray.Where(t => t.layer == LayerMask.NameToLayer(layerName)));

            return objectList.Count == 0 ? null : objectList.ToArray();
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
                Debug.LogError("Failed to heal " + target.name + " does not have a health component");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        ///  Apply the given amount of damage to the given target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="amount"></param>
        /// <param name="showText"></param>
        /// <param name="damageDealer"></param>
        public static void ApplyDamageToTarget(GameObject target, float amount, bool showText, GameObject damageDealer)
        {
            if (!IsObjectOnLayer(GameAssets.GlobalManager.whatCanBeDamaged, target) || target.CompareTag("Discard Damage")) return;
            var health = target.GetComponent<HealthComponent>();

            var leechTailHitBox = target.GetComponent<LeechTailHitBox>();

            if (health)
            {
                health.ProcessesDamage(amount, showText, GameAssets.GlobalManager.whatCanBeDamaged);
            }
            else if (leechTailHitBox)
            {
                leechTailHitBox.DamageParent(showText, amount, GameAssets.GlobalManager.whatCanBeDamaged);
            }
            else
            {
                Debug.LogError(damageDealer.name + " Failed to damage " + target.name + " does not have a health component");
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
                    health.ProcessesDamage(1000000000, false, GameAssets.GlobalManager.whatCanBeDamaged);
                }
                else
                {
                    Debug.LogError("Failed to kill " + target.name + " does not have a health component");
                }
            }
        }

        /// <summary>
        /// Apply a status effect to the given target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="statusEffectToApply"></param>
        public static StatusEffect ApplyStatusEffectToTarget(GameObject target, ScriptableStatusEffect statusEffectToApply)
        {
            var auraManager = target.GetComponent<AuraManager>();

            if (auraManager)
            {
                return auraManager.ApplyEffect(target, statusEffectToApply);
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
        /// <param name="forceMode2D"></param>
        public static void ApplyKnockback(GameObject target, Vector2 force, ForceMode2D forceMode2D)
        {
            var rigidbody2D = target.GetComponent<Rigidbody2D>();

            if (rigidbody2D)
            {
                rigidbody2D.AddForce(force, forceMode2D);
            }
            else
            {
                Debug.LogError("Failed to ApplyKnockback " + target.name + " does not have a Rigidbody2D component");
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
                Debug.LogError("Failed to ApplyKnockback " + target.name + " does not have a Rigidbody2D component");
            }
        }
        /// <summary>
        /// Will cast the provided spell
        /// </summary>
        public static void CastSpell(ScriptableSpell spellInfo)
        {
            if (spellInfo.SpellToSpawn)
            {
                var spellToCast = Instantiate(spellInfo.SpellToSpawn);

                if (spellToCast)
                {
                    var spell = spellToCast.GetComponent<Spell>();

                    if (spell)
                    {
                        DontDestroyOnLoad(spellToCast);

                        spell.StartSpellCast(spellInfo, spellToCast);
                    }
                    else
                    {
                        Debug.LogError("Failed to start spell GameObject " + spellToCast.name + " does not have a spell component");
                    }
                }
                else
                {
                    Debug.LogError("Failed to start spell " + spellInfo.name + " failed to spawn");
                }
            }
            else
            {
                Debug.LogError("Failed to start spell " + spellInfo.name + " does not have a spell to spawn");
            }
        }

        /// <summary>
        /// Will cast the provided spell
        /// </summary>
        /// <param name="spellInfo"></param>
        /// <param name="spellIcon"></param>
        public static void CastSpell(ScriptableSpell spellInfo, SpellIcon spellIcon)
        {
            if (spellInfo.SpellToSpawn)
            {
                var spellToCast = Instantiate(spellInfo.SpellToSpawn);

                if (spellToCast)
                {
                    var spell = spellToCast.GetComponent<Spell>();

                    if (spell)
                    {
                        DontDestroyOnLoad(spellToCast);

                        spell.StartSpellCast(spellInfo, spellToCast, spellIcon);
                    }
                    else
                    {
                        Debug.LogError("Failed to start spell GameObject " + spellToCast.name + " does not have a spell component");
                    }
                }
                else
                {
                    Debug.LogError("Failed to start spell " + spellInfo.name + " failed to spawn");
                }
            }
            else
            {
                Debug.LogError("Failed to start spell " + spellInfo.name + " does not have a spell to spawn");
            }
        }

        /// <summary>
        /// Checks to see what direction the given rigidbody is moving in
        /// </summary>
        /// <param name="rigidbody2D"></param>
        /// <param name="transform"></param>
        /// <param name="wasIdle"></param>
        /// <returns>Returns true if left</returns>
        public static bool IsRigidBodyMovingLeftOrRight(Rigidbody2D rigidbody2D, Transform transform, out bool wasIdle)
        {
            Vector3 vel = transform.rotation * rigidbody2D.velocity;

            if (vel.x > 0)
            {
                wasIdle = false;

                return true;
            }
            else if (vel.x < 0)
            {
                wasIdle = false;

                return false;
            }
            else
            {
                wasIdle = true;

                return false;
            }
        }
        /// <summary>
        /// Checks to see if player is moving left or right
        /// </summary>
        /// <returns>Returns true if moving left</returns>
        public static bool IsPlayerMovingLeftOrRight(out bool isIdle)
        {
            return GetPlayerController().IsMovingRightOrLeft(out isIdle);
        }
        #endregion

        #region Gameobject Functions
        /// <summary>
        /// Get the PlayerUIManager cached in Game Assets
        /// </summary>
        public static PlayerUIManager GetPlayerUIManager()
        {
            return GameAssets.PlayerHudManager;
        }
        /// <summary>
        /// Gets the GameplayManger cached in Game Assets
        /// </summary>
        /// <returns></returns>
        public static GameplayManager GetGameplayManager()
        {
            return GameAssets.GlobalManager;
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
        // ReSharper disable once MemberCanBePrivate.Global
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
        /// Gets The Player Controller
        /// </summary>
        /// <returns>The player controller component</returns>
        public static PlayerController GetPlayerController()
        {
            return GetPlayerGameObject().GetComponent<PlayerController>();
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
        /// <param name="array"></param>
        /// <param name="objectToFind"></param>
        public static int FindItemIndex<T>(T[] array, T objectToFind)
        {
            if (objectToFind != null)
            {
                for (var index = 0; index < array.Length; index++)
                {
                    var item = array[index];

                    if (item == null) continue;
                    
                    if (item.Equals(objectToFind))
                    {
                        return index;
                    }
                }

                Debug.LogError("Failed to find item index in " + array);

                return 0;
            }
            else
            {
                Debug.LogError("Failed to find item index in " + array + " item was null");

                return 0;
            }
        }
        #endregion

        #region Actionbar Functions
        /// <summary>
        /// Assign a single spell to the Actionbar
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public static void AssignSpellToActionbar(ScriptableSpell scriptableSpell)
        {
            GameAssets.PlayerHudManager.AssignSpell(scriptableSpell);
        }
        /// <summary>
        /// Assign a single spell to Actionbar and check to see if it did get assigned
        /// </summary>
        /// <param name="scriptableSpell"></param>
        /// <param name="spellAssigned"></param>
        public static void AssignSpellToActionbar(ScriptableSpell scriptableSpell, out bool spellAssigned)
        {
            GameAssets.PlayerHudManager.AssignSpell(scriptableSpell, out spellAssigned);
        }
        /// <summary>
        /// Assign multiple spells to Actionbar
        /// </summary>
        /// <param name="scriptableSpells"></param>
        public static void AssignSpellsToActionbar(ScriptableSpell[] scriptableSpells)
        {
            GameAssets.PlayerHudManager.AssignSpells(scriptableSpells);
        }
        /// <summary>
        /// Assign multiple spells to Actionbar and check to see if all spells where assigned
        /// </summary>
        /// <param name="scriptableSpells"></param>
        /// <param name="allSpellsAssigned"></param>
        public static void AssignSpellsToActionbar(ScriptableSpell[] scriptableSpells, out bool allSpellsAssigned)
        {
            GameAssets.PlayerHudManager.AssignSpells(scriptableSpells, out allSpellsAssigned);
        }
        /// <summary>
        /// Find the given spell on the Actionbar
        /// </summary>
        /// <param name="scriptableSpell"></param>
        /// <returns>A ScriptableSpell</returns>
        public static ScriptableSpell FindSpellOnActionbar(ScriptableSpell scriptableSpell)
        {
            return GameAssets.PlayerHudManager.FindSpellOnActionbar(scriptableSpell);
        }
        /// <summary>
        /// Finds the Spell Icon that has the given spell
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public static SpellIcon FindSpellIconOnActionbar(ScriptableSpell scriptableSpell)
        {
            return GameAssets.PlayerHudManager.FindSpellIconOnActionbar(scriptableSpell);
        }
        /// <summary>
        ///  Finds the given ScriptableSpell on the Actionbar then removes it
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public static void RemoveSpellFromActionbar(ScriptableSpell scriptableSpell)
        {
            GameAssets.PlayerHudManager.RemoveSpellFromSlot(scriptableSpell);
        }
        /// <summary>
        /// Finds the given ScriptableSpell on the Actionbar then removes it and check to see if the spell was removed
        /// </summary>
        /// <param name="scriptableSpell"></param>
        /// <param name="wasSpellRemoved"></param>
        public static void RemoveSpellFromActionbar(ScriptableSpell scriptableSpell, out bool wasSpellRemoved)
        {
            GameAssets.PlayerHudManager.RemoveSpellFromSlot(scriptableSpell, out wasSpellRemoved);
        }
        /// <summary>
        /// Finds the given ScriptableSpells on the Actionbar then remove them
        /// </summary>
        /// <param name="scriptableSpells"></param>
        public static void RemoveSpellsFromSlots(ScriptableSpell[] scriptableSpells)
        {
            GameAssets.PlayerHudManager.RemoveSpellsFromSlots(scriptableSpells);
        }
        /// <summary>
        /// Finds the given ScriptableSpells on the Actionbar then remove them and check to see if they where all removed
        /// </summary>
        /// <param name="scriptableSpells"></param>
        /// <param name="whereAllSpellsRemoved"></param>
        public static void RemoveSpellsFromSlots(ScriptableSpell[] scriptableSpells, out bool whereAllSpellsRemoved)
        {
            GameAssets.PlayerHudManager.RemoveSpellsFromSlots(scriptableSpells, out whereAllSpellsRemoved);
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
        /// <returns>The hit information as RaycastHit2D</returns>
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
        /// <summary>
        /// Shack the virtual camera with the given intensity
        /// </summary>
        /// <param name="intensity"></param>
        /// <param name="time"></param>
        public static void ShakeCamera(float intensity, float time)
        {
            GameAssets.CameraShakeComponent.ShakeCamera(intensity, time);
        }
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Fade out / in Any Gameobject must be used in StartCoroutine function
        /// </summary>
        /// <param name="objectToFade"></param>
        /// <param name="fadeIn"></param>
        /// <param name="duration"></param>
        /// <param name="destroyOnFade"></param>
        public static IEnumerator FadeInAndOut(GameObject objectToFade, bool fadeIn, float duration, bool destroyOnFade)
        {
            var counter = 0f;

            //Set Values depending on if fadeIn or fadeOut
            float a, b;
            if (fadeIn)
            {
                a = 0;
                b = 1;
            }
            else
            {
                a = 1;
                b = 0;
            }

            int mode;
            Color currentColor;

            var tempSpRenderer = objectToFade.GetComponent<SpriteRenderer>();
            var tempImage = objectToFade.GetComponent<Image>();
            var tempRawImage = objectToFade.GetComponent<RawImage>();
            var tempRenderer = objectToFade.GetComponent<MeshRenderer>();
            var tempText = objectToFade.GetComponent<TextMeshPro>();

            //Check if this is a Sprite
            if (tempSpRenderer != null)
            {
                currentColor = tempSpRenderer.color;
                mode = 0;
            }
            //Check if Image
            else if (tempImage != null)
            {
                currentColor = tempImage.color;
                mode = 1;
            }
            //Check if RawImage
            else if (tempRawImage != null)
            {
                currentColor = tempRawImage.color;
                mode = 2;
            }
            //Check if Text 
            else if (tempText != null)
            {
                currentColor = tempText.color;
                mode = 3;
            }

            //Check if 3D Object
            else if (tempRenderer != null)
            {
                currentColor = tempRenderer.material.color;
                mode = 4;

                //ENABLE FADE Mode on the material if not done already
                tempRenderer.material.SetFloat(Mode, 2);
                tempRenderer.material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                tempRenderer.material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                tempRenderer.material.SetInt(ZWrite, 0);
                tempRenderer.material.DisableKeyword("_ALPHATEST_ON");
                tempRenderer.material.EnableKeyword("_ALPHABLEND_ON");
                Material material;
                (material = tempRenderer.material).DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
            else
            {
                yield break;
            }

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float alpha = Mathf.Lerp(a, b, counter / duration);

                switch (mode)
                {
                    case 0:
                        tempSpRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                        break;
                    case 1:
                        tempImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                        break;
                    case 2:
                        tempRawImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                        break;
                    case 3:
                        tempText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                        break;
                    case 4:
                        tempRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                        break;
                }

                if (destroyOnFade && counter >= duration)
                {
                    Destroy(objectToFade);
                }

                yield return null;
            }
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
            for (int index = items.Count - 1; index >= 0; index--)
            {
                yield return items[index];
            }
        }
    }
    #endregion
}