using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class Boundaries : MonoBehaviour
    {
        private Vector2 screenBounds;
        private Vector3 viewPos;
        private float objectWidth;
        private float objectHeight;

        public Boundaries(ConstrainStates boundaryConstrainStates)
        {
            BoundaryConstrainStates = boundaryConstrainStates;
        }

        /// <summary>
        /// Should this perform a boundary check
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool DoBoundaryCheck { get; set; } = true;
        /// <summary>
        /// Container for maximum Y and X cords
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Vector2 MaxCords { get; set; } = Vector2.zero;
        /// <summary>
        /// The current state of boundary check
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
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
            if (!(Camera.main is null)) screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

            MaxCords = new Vector2(screenBounds.x - objectWidth, screenBounds.y - objectHeight);
        }
        /// <summary>
        /// Called every frame after Update
        /// </summary>
        private void LateUpdate()
        {
            if (DoBoundaryCheck)
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
                        var position = transform.position;
                        viewPos = position;
                        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
                        viewPos.y = position.y;
                        position = viewPos;
                        transform.position = position;
                        break;
                    case ConstrainStates.ConstrainY:
                        var transform1 = transform;
                        var position1 = transform1.position;
                        viewPos = position1;
                        viewPos.x = position1.x;
                        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
                        transform.position = viewPos;
                        break;
                }
            }
        }
    }
}