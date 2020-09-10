using UnityEngine;
using UnityEngine.Events;
using AuraSystem;
using System.Collections;
using PlayerCharacter.Controller;
using System.Collections.Generic;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        private GameplayObjectID idObject = null;
        private SpriteRenderer spriteRenderer = null;
        private Vector3 defaultPlayerScale = Vector3.zero;
        private Vector3 deafultSpearLocation = Vector3.zero;

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<WormSegment> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        [HideInInspector]
        public UnityEvent SquishedPlayer;

        #region Collision Functions
        /// <summary>
        /// Set all needed references
        /// </summary>
        private void Awake()
        {
            idObject = GetComponent<GameplayObjectID>();
            MyHealthComponent = GetComponent<HealthComponent>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            MyBoxCollider2D = GetComponent<BoxCollider2D>();
            MyCapsuleCollider2D = GetComponent<CapsuleCollider2D>();

            MyWidth = GeneralFunctions.GetSpriteWidth(GetComponent<SpriteRenderer>());

            defaultPlayerScale = GeneralFunctions.GetPlayerGameObject().transform.localScale;

            deafultSpearLocation = GeneralFunctions.GetPlayerGameObject().transform.GetChild(1).transform.localPosition;

            idObject.ConstructID();
            MyHealthComponent.ConstructHealthComponent();
            MyHealthComponent.OnDeath.AddListener(OnDeath);
        }
        /// <summary>
        /// Called when segment dies disables both collision and sprite renderer then invokes an OnSegmentDeath event
        /// </summary>
        private void OnDeath()
        {
            spriteRenderer.enabled = false;
            MyBoxCollider2D.enabled = false;
            MyCapsuleCollider2D.enabled = false;

            SegmentDeath.Invoke(this);
        }
        /// <summary>
        /// Check to see if segment is in the ground if not enable collision
        /// </summary>
        public void CheckCollision()
        {
            Collider2D collider2D = Physics2D.OverlapCapsule(transform.position, MyCapsuleCollider2D.size, MyCapsuleCollider2D.direction, GeneralFunctions.GetObjectEulerAngle(gameObject), WhatIsGround);

            if (collider2D)
            {
                AboveGround = false;
            }
            else
            {
                AboveGround = true;
            }
        }
        /// <summary>
        /// When hit by player deal damage
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!MyHealthComponent.IsCurrentlyDead && IsRotating)
                {
                    GeneralFunctions.DamageTarget(collision.gameObject, DamageToApply, true, gameObject);

                    var playerLegs = collision.transform.GetChild(0).GetComponent<PlayerLegs>();
                    
                    if (playerLegs)
                    {
                        if (playerLegs.TouchingGround())
                        {
                            SquishPlayer(collision.gameObject);
                        }
                    }
                    else
                    {
                        Debug.LogError(gameObject.name + " failed to get player legs");
                    }
                }
            }
        }
        /// <summary>
        /// Set player scale to equal the squish scale
        /// </summary>
        /// <param name="player"></param>
        private void SquishPlayer(GameObject player)
        {
            player.transform.localScale = SquishScale;

            if (!GeneralFunctions.GetGameObjectHealthComponent(player).IsCurrentlyDead)
            {
                GeneralFunctions.ApplyDebuffToTarget(player, DebuffToApply, true);

                SquishedPlayer.Invoke();

                StartCoroutine(UnSquishPlayer(player));
            }
        }
        /// <summary>
        /// Return player back to the default scale
        /// </summary>
        private IEnumerator UnSquishPlayer(GameObject player)
        {
            yield return new WaitForSeconds(SquishTime);

            player.transform.localScale = defaultPlayerScale;

            // Move spear back to it's default location
            player.transform.GetChild(1).transform.localPosition = deafultSpearLocation;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Amount of damage to apply to the player
        /// </summary>
        public float DamageToApply { get; set; }
        /// <summary>
        /// Gets this Gameobjects ID
        /// </summary>
        public int MyID { get { return idObject.ID; } }
        /// <summary>
        /// Get this Gameobjects health component
        /// </summary>
        public HealthComponent MyHealthComponent { get; private set; } = null;
        /// <summary>
        /// BoxCollider2D attached to segment
        /// </summary>
        public BoxCollider2D MyBoxCollider2D { get; private set; } = null;
        /// <summary>
        /// CapsuleCollider2D attached to segment
        /// </summary>
        public CapsuleCollider2D MyCapsuleCollider2D { get; private set; } = null;
        /// <summary>
        /// What layers are ground
        /// </summary>
        public LayerMask WhatIsGround { get; set; }
        /// <summary>
        /// Whether or not to draw debug lines
        /// </summary>
        public bool DrawDebug { get; set; }
        /// <summary>
        /// Checks to see if the current segment is above ground
        /// </summary>
        public bool AboveGround { get; private set; }
        /// <summary>
        /// Get the width of the worm segment sprite
        /// </summary>
        public float MyWidth { get; private set; }
        /// <summary>
        /// Check to see if the worm is rotating
        /// </summary>
        public bool IsRotating { get; set; }
        /// <summary>
        /// The debuff to apply to the player
        /// </summary>
        public ScriptableDebuff DebuffToApply { get; set; }
        /// <summary>
        /// The amount of time the player is squished for
        /// </summary>
        public float SquishTime { get; set; }
        /// <summary>
        /// The new scale to apply to the player
        /// </summary>
        public Vector3 SquishScale { get; set; }
        #endregion
    }
}