using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class Socket : MonoBehaviour
    {
        /// <summary>
        /// Checks to if the socket actually a object attached to it
        /// </summary>
        private bool isObjectAttached;
        /// <summary>
        /// The object to attach
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public GameObject ObjectToAttach { get; private set; }
        /// <summary>
        /// How much to offset ObjectToAttach
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public Vector3 Offset { get; private set; } = Vector3.zero;
        /// <summary>
        /// Attach the given object to the socket
        /// </summary>
        /// <param name="objectToAttach"></param>
        /// <param name="offset"></param>
        public void AttachObject(GameObject objectToAttach, Vector3 offset)
        {
            ObjectToAttach = objectToAttach;
            Offset = offset;

            isObjectAttached = true;
        }
        /// <summary>
        /// Update the location of ObjectToAttach
        /// </summary>
        private void LateUpdate()
        {
            if (!isObjectAttached) return;
            
            if (ObjectToAttach)
            {
                ObjectToAttach.transform.position = transform.position + Offset;
            }
            else
            {
                Debug.LogError(" Failed to attach to ObjectToAttach is not valid " + " on socket " + name);
            }
        }
    }
}