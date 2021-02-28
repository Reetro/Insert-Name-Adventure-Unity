using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GeneralScripts.GeneralComponents
{
    public class RigidbodyManager : MonoBehaviour
    {
        private Rigidbody2D myRigidBody2D;
        private bool isZFrozen;
        private bool wasFrozenByPlatform;

        [FormerlySerializedAs("OnPlatformEnter")] [HideInInspector]
        public UnityEvent onPlatformEnter;
        [FormerlySerializedAs("OnPlatformExit")] [HideInInspector]
        public UnityEvent onPlatformExit;

        private void Awake()
        {
            myRigidBody2D = GetComponent<Rigidbody2D>();
            isZFrozen = myRigidBody2D.freezeRotation;
        }
        /// <summary>
        /// Called when the gameobject touches a moving platform
        /// </summary>
        public void OnPlatformEnterCall()
        {
            onPlatformEnter.Invoke();

            if (isZFrozen) return;
            myRigidBody2D.freezeRotation = false;
            wasFrozenByPlatform = true;
        }
        /// <summary>
        /// Called when he gameobject is no longer touches a moving platform
        /// </summary>
        public void OnPlatformExitCall()
        {
            onPlatformExit.Invoke();

            if (!wasFrozenByPlatform || isZFrozen) return;
            myRigidBody2D.freezeRotation = false;
            wasFrozenByPlatform = false;
        }
    }
}