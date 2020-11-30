using UnityEngine;

public class Socket : MonoBehaviour
{
    /// <summary>
    /// Checks to if the socket actually a object attached to it
    /// </summary>
    private bool isObjectAttached = false;
    /// <summary>
    /// The object to attach
    /// </summary>
    public GameObject ObjectToAttach { get; private set; } = null;
    /// <summary>
    /// How much to offset ObjectToAttach
    /// </summary>
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
    /// Deattach the current attached item from the socket
    /// </summary>
    public void DeattachObject()
    {
        isObjectAttached = false;

        ObjectToAttach = null;
    }
    /// <summary>
    /// Update the location of ObjectToAttach
    /// </summary>
    void LateUpdate()
    {
        if (isObjectAttached)
        {
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