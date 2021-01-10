using UnityEngine;

namespace ComponentLibrary
{
    public class Boundaries : MonoBehaviour
    {
        private Vector2 screenBounds;
        private Vector3 viewPos;
        private float objectWidth;
        private float objectHeight;

        /// <summary>
        /// Should this perform a boundary check
        /// </summary>
        public bool DoBounderyCheck { get; set; } = true;
        /// <summary>
        /// Container for maximum Y and X cords
        /// </summary>
        public Vector2 MaxCords { get; set; } = Vector2.zero;
        /// <summary>
        /// The current state of boundary check
        /// </summary>
        public ConstrainStates BoundaryConstrainStates { get; set; }

        /// <summary>
        /// All Boundary Constrain States
        /// </summary>
        public enum ConstrainStates
        {
            /// <summary>
            /// If true will constrain the X coordinate to camera
            /// </summary>
            ConstrainX,
            /// <summary>
            /// If true will constrain the Y coordinate to camera
            /// </summary>
            ConstrainY
        }

        /// <summary>
        /// Get all needed values
        /// </summary>
        private void Start()
        {
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

            MaxCords = new Vector2(screenBounds.x - objectWidth, screenBounds.y - objectHeight);
        }
        /// <summary>
        /// Called every frame after Update
        /// </summary>
        private void LateUpdate()
        {
            if (DoBounderyCheck)
            {
                switch(BoundaryConstrainStates)
                {
                    default:
                        viewPos = transform.position;
                        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
                        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
                        transform.position = viewPos;
                        break;
                    case ConstrainStates.ConstrainX:
                        viewPos = transform.position;
                        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
                        viewPos.y = transform.position.y;
                        transform.position = viewPos;
                        break;
                    case ConstrainStates.ConstrainY:
                        viewPos = transform.position;
                        viewPos.x = transform.position.x;
                        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
                        transform.position = viewPos;
                        break;
                }
            }
        }
    }
}