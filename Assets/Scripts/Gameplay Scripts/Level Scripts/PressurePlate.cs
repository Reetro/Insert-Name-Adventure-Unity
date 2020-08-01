using UnityEngine;
using UnityEngine.Events;

namespace LevelObjects.Trap
{
    public class PressurePlate : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The layers that can activate this pressure plate")]
        public LayerMask whatCanPressPlate;

        [Header("Events")]
        public UnityEvent onPressurePlatePressed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer(whatCanPressPlate, collision.gameObject))
            {
                onPressurePlatePressed.Invoke();
            }
        }
    }
}