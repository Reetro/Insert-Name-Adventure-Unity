using UnityEngine;
using UnityEngine.Events;

namespace LevelObjects.Trap
{
    public class PressurePlate : MonoBehaviour
    {
        [Header("Settings")]
        public string[] whatCanPressPlate;

        [Header("Events")]
        public UnityEvent onPressurePlatePressed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string currentTag in whatCanPressPlate)
            {
                if (collision.gameObject.CompareTag(currentTag))
                {
                    onPressurePlatePressed.Invoke();
                }
            }
        }
    }
}