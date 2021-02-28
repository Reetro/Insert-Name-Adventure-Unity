using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class CameraTriggerCollider : MonoBehaviour
    {
        [SerializeField] private LayerMask whatCanIOverlap = new LayerMask();

        private void Awake()
        {
            whatCanIOverlap = LayerMask.GetMask("Enemy");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(whatCanIOverlap, collision.gameObject))
            {
                GeneralFunctions.GetGameplayManager().cameraBoxOverlap.Invoke(gameObject.name);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(whatCanIOverlap, collision.gameObject))
            {
                GeneralFunctions.GetGameplayManager().cameraBoxOverlap.Invoke(gameObject.name);
            }
        }
    }
}