using UnityEngine;
using UnityEngine.Events;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        private FixedJoint2D fixedJoint2D = null;
        private BoxCollider2D boxCollider2D = null;
        private int index = 0;
        private SpriteRenderer spriteRenderer = null;
        private CapsuleCollider2D capsuleCollider2D = null;
        private LayerMask whatIsGround = new LayerMask();
        private Rigidbody2D myRigidbody2D = null;

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<int> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        #region Setup Functions
        /// <summary>
        /// Set all needed internal references
        /// </summary>
        public void SetupSegment(int index, float healthAmount, LayerMask layerMask)
        {
            this.index = index;

            if (this.index >= 0)
            {
                fixedJoint2D = GetComponent<FixedJoint2D>();

                if (fixedJoint2D)
                {
                    boxCollider2D = GetComponent<BoxCollider2D>();

                    if (boxCollider2D)
                    {
                        MyHealthComponent = GetComponent<HealthComponent>();

                        if (MyHealthComponent)
                        {
                            MyHealthComponent.ConstructHealthComponent();

                            MyHealthComponent.SetHealth(healthAmount);

                            MyHealthComponent.OnDeath.AddListener(OnSegmentKillied);

                            spriteRenderer = GetComponent<SpriteRenderer>();

                            if (spriteRenderer)
                            {
                                capsuleCollider2D = GetComponent<CapsuleCollider2D>();

                                if (capsuleCollider2D)
                                {
                                    whatIsGround = layerMask;

                                    myRigidbody2D = GetComponent<Rigidbody2D>();

                                    if (!myRigidbody2D)
                                    {
                                        Debug.LogError(name + " Does not have a Rigidbody 2D Component");
                                    }
                                }
                                else
                                {
                                    Debug.LogError(name + " Does not have a Capsule Collider 2D");
                                }
                            }
                            else
                            {
                                Debug.LogError(name + " Does not have a Sprite Render Component");
                            }
                        }
                        else
                        {
                            Debug.LogError(name + " Does not have a Health Component");
                        }
                    }
                    else
                    {
                        Debug.LogError(name + " Does not have a Box Collider 2D Component");
                    }
                }
                else if (index > 0)
                {
                    Debug.LogError(name + " Does not have a Fixed Joint 2D Component");
                }
                else
                {
                    boxCollider2D = GetComponent<BoxCollider2D>();

                    if (boxCollider2D)
                    {
                        MyHealthComponent = GetComponent<HealthComponent>();

                        if (MyHealthComponent)
                        {
                            MyHealthComponent.ConstructHealthComponent();

                            MyHealthComponent.SetHealth(healthAmount);

                            MyHealthComponent.OnDeath.AddListener(OnSegmentKillied);

                            spriteRenderer = GetComponent<SpriteRenderer>();

                            if (spriteRenderer)
                            {
                                capsuleCollider2D = GetComponent<CapsuleCollider2D>();

                                if (capsuleCollider2D)
                                {
                                    whatIsGround = layerMask;

                                    myRigidbody2D = GetComponent<Rigidbody2D>();

                                    if (!myRigidbody2D)
                                    {
                                        Debug.LogError(name + " Does not have a Rigidbody 2D Component");
                                    }
                                }
                                else
                                {
                                    Debug.LogError(name + " Does not have a Capsule Collider 2D");
                                }
                            }
                            else
                            {
                                Debug.LogError(name + " Does not have a Sprite Render Component");
                            }
                        }
                        else
                        {
                            Debug.LogError(name + " Does not have a Health Component");
                        }
                    }
                }
            }
            else
            {
                Debug.LogError(name + " Index is invalid " + " Index was " + this.index);
            }
        }
        #endregion

        #region Collision functions
        /// <summary>
        /// Check to see if the worm is above ground
        /// </summary>
        public void CheckCollision()
        {
            if (capsuleCollider2D)
            {
                Collider2D collider2D = Physics2D.OverlapCapsule(transform.position, capsuleCollider2D.size, capsuleCollider2D.direction, GeneralFunctions.GetObjectEulerAngle(gameObject), whatIsGround);

                if (collider2D)
                {
                    DisableCollision();
                }
                else
                {
                    EnableCollisioin();
                }
            }
            else
            {
                Debug.LogError(name + " Does not have a Capsule Collider 2D" + " failed to check collision");
            }
        }
        /// <summary>
        /// Enable worm segment box collider, fixed joint, and unfreeze position
        /// </summary>
        public void EnableCollisioin()
        {
            if (index > 0)
            {
                if (fixedJoint2D)
                {
                    fixedJoint2D.enabled = true;

                    if (boxCollider2D)
                    {
                        boxCollider2D.enabled = true;

                        if (myRigidbody2D)
                        {
                            myRigidbody2D.isKinematic = false;

                            myRigidbody2D.freezeRotation = false;
                        }
                        else
                        {
                            Debug.LogError(name + " Does not have a Rigidbody 2D Component failed to unfreeze position");
                        }
                    }
                    else
                    {
                        Debug.LogError(name + " failed to enable Box Collider 2D Component");
                    }
                }
                else
                {
                    Debug.LogError(name + " failed to enable Fixed Joint 2D Component");
                }
            }
            else
            {
                if (boxCollider2D)
                {
                    boxCollider2D.enabled = true;

                    if (myRigidbody2D)
                    {
                        myRigidbody2D.isKinematic = false;

                        myRigidbody2D.freezeRotation = false;
                    }
                    else
                    {
                        Debug.LogError(name + " Does not have a Rigidbody 2D Component failed to unfreeze position");
                    }
                }
                else
                {
                    Debug.LogError(name + " failed to enable Box Collider 2D Component");
                }
            }
        }
        /// <summary>
        /// Disable worm box collider, fixed joint, and freeze position
        /// </summary>
        public void DisableCollision()
        {
            if (index > 0)
            {
                if (fixedJoint2D)
                {
                    fixedJoint2D.enabled = false;

                    if (boxCollider2D)
                    {
                        boxCollider2D.enabled = false;

                        if (myRigidbody2D)
                        {
                            myRigidbody2D.isKinematic = true;

                            myRigidbody2D.freezeRotation = true;
                        }
                        else
                        {
                            Debug.LogError(name + " Does not have a Rigidbody 2D Component failed to freeze position");
                        }
                    }
                    else
                    {
                        Debug.LogError(name + " failed to disable Box Collider 2D Component");
                    }
                }
                else
                {
                    Debug.LogError(name + " failed to disable Fixed Joint 2D Component");
                }
            }
            else
            {
                if (boxCollider2D)
                {
                    boxCollider2D.enabled = false;

                    if (myRigidbody2D)
                    {
                        myRigidbody2D.isKinematic = true;

                        myRigidbody2D.freezeRotation = true;
                    }
                    else
                    {
                        Debug.LogError(name + " Does not have a Rigidbody 2D Component failed to freeze position");
                    }
                }
                else
                {
                    Debug.LogError(name + " failed to disable Box Collider 2D Component");
                }
            }
        }
        #endregion

        #region Gameplay Functions
        /// <summary>
        /// Set the opacity of the worm segment
        /// </summary>
        /// <param name="newOpacity"></param>
        public void SetOpacity(float newOpacity)
        {
            if (spriteRenderer)
            {
                var tmp = spriteRenderer.color;

                tmp.a = newOpacity;

                spriteRenderer.color = tmp;
            }
            else
            {
                Debug.LogError(name + " Does not have a Sprite Render Component " + " failed to change opacity");
            }
        }
        /// <summary>
        /// Called whenever a worm segment is killed
        /// </summary>
        private void OnSegmentKillied()
        {
            boxCollider2D.enabled = false;

            capsuleCollider2D.enabled = false;

            if (fixedJoint2D)
            {
                fixedJoint2D.enabled = false;
            }

            spriteRenderer.enabled = false;

            SegmentDeath.Invoke(index);
        }
        #endregion

        #region Properites
        /// <summary>
        /// Check to see if the worm segment is above ground
        /// </summary>
        public bool isAboveGround { get; private set; }
        /// <summary>
        /// Gets the health component on the worm segment
        /// </summary>
        public HealthComponent MyHealthComponent { get; private set; }
        #endregion
    }
}