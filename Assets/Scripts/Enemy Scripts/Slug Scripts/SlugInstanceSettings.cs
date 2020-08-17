using UnityEngine;

namespace EnemyCharacter.AI
{
    public class SlugInstanceSettings : MonoBehaviour
    {
        [Tooltip("Should be true if slug is on a platform that is not connected to any walls")]
        [SerializeField] private bool onFloatingPlatform = false;
        [Tooltip("Whether or not to draw debug lines")]
        [SerializeField] private bool drawDebug = false;

        private SlugMovement slugMovement = null;

        void Awake()
        {
            slugMovement = GetComponent<SlugMovement>();

            slugMovement.drawDebug = drawDebug;
            slugMovement.onFloatingPlatform = onFloatingPlatform;
        }
    }
}
