using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class JointToggle : MonoBehaviour
    {
        [SerializeField] private Joint2D joint;
        private Rigidbody2D connectedBody;

        private void Awake()
        {
            joint = joint ? joint : GetComponent<Joint2D>();
            if (joint) connectedBody = joint.connectedBody;
            else Debug.LogError("No joint found.", this);
        }

        private void OnEnable() { joint.connectedBody = connectedBody; }

        private void OnDisable()
        {
            joint.connectedBody = null;
            connectedBody.WakeUp();
        }
    }
}