using UnityEngine;

namespace ComponentLibrary
{
    public class EyeTrace : MonoBehaviour
    {
        [Tooltip("The object the trace starts from")]
        [SerializeField] private GameObject traceOriginObject = null;
        [Tooltip("How far this trace can travel")]
        [SerializeField] private float traceDistance = 1.0f;
        [Tooltip("What layers this object can see")]
        [SerializeField] private LayerMask whatCanThisObjectSee = new LayerMask();
        [Tooltip("Will toggle on and off a line that will display the current direction of the raycast")]
        [SerializeField] private bool drawDebug = false;

        /// <summary>
        /// Get the layers this object can see
        /// </summary>
        public LayerMask EyeLayerMask { get { return whatCanThisObjectSee; } }

        /// <summary>
        /// Will trace left or right (depends on look direction) of the trace origin object
        /// </summary>
        /// <returns>The Raycast Hit</returns>
        public RaycastHit2D TraceFromEyes()
        {
            if (transform.localEulerAngles.y >= 180f)
            {
                Vector2 position = traceOriginObject.transform.position;
                Vector2 direction = Vector2.right;

                RaycastHit2D hit = Physics2D.Raycast(position, -direction, traceDistance, whatCanThisObjectSee);

                if (drawDebug)
                {
                    Debug.DrawRay(position, -direction, Color.green);
                }

                return hit;
            }
            else
            {
                Vector2 position = traceOriginObject.transform.position;
                Vector2 direction = Vector2.right;

                RaycastHit2D hit = Physics2D.Raycast(position, direction, traceDistance, whatCanThisObjectSee);

                if (drawDebug)
                {
                    Debug.DrawRay(position, direction, Color.green);
                }

                return hit;
            }
        }
    }
}