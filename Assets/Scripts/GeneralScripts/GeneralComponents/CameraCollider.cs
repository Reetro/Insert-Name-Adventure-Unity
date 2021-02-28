using System.Collections.Generic;
using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class CameraCollider : MonoBehaviour
    {
        public float widthOfCollider = 0.5f;
        public float offset = 1f;

        private float zAxis;
        private Dictionary<string, Transform> colliders = new Dictionary<string, Transform>();
        private Vector2 screenSize;
        private Vector3 cameraPos = Vector3.zero;
        private Camera camera1;
        private Camera camera2;
        private Camera camera3;

        private void Start()
        {
            var main = Camera.main;
            camera3 = main;
            camera2 = main;
            camera1 = main;
        }

        /// <summary>
        /// Setup Camera Sub Colliders call backs
        /// </summary>
        public void OnSceneCreated()
        {
            GeneralFunctions.GetGameplayManager().cameraBoxOverlap.AddListener(OnCameraTriggerOverlap);
        }

        private void Awake()
        {
            CreateScreenColliders();
        }
        /// <summary>
        /// Make sure all box's are valid and are triggers
        /// </summary>
        private void CreateScreenColliders()
        {
            var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            if (playerTransform != null)
            {
                zAxis = playerTransform.position.z;
            }
                
            //Create a Dictionary to hold the transforms and their names
            colliders = new Dictionary<string, Transform>
            {
                {"Top", new GameObject().transform},
                {"Bottom", new GameObject().transform},
                {"Right", new GameObject().transform},
                {"Left", new GameObject().transform}
            };
            //Create GameObjects and add their Transform components to the Dictionary created above

            //Calculate world space screenSize based on the MainCamera position
            if (!(Camera.main is null))
            {
                screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)),
                    Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
                screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)),
                    Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
            }

            foreach (KeyValuePair<string, Transform> bc in colliders)
            {
                //Add the collider component
                bc.Value.gameObject.AddComponent<BoxCollider2D>();
                //Add Camera Trigger
                bc.Value.gameObject.AddComponent<CameraTriggerCollider>();
                //Make sure collision is to be a trigger
                bc.Value.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                //give the objects a name
                bc.Value.name = bc.Key + "Collider";
                //Make the object with collider child of the object this script is attached to
                bc.Value.parent = transform;
                //Scale the object to the width and height of the screen
                if (bc.Key == "Left" || bc.Key == "Right")
                {
                    bc.Value.localScale = new Vector3(widthOfCollider, screenSize.y * 2, widthOfCollider);
                }
                else
                {
                    bc.Value.localScale = new Vector3(screenSize.x * 2, widthOfCollider, widthOfCollider);
                }
            }
        }
        /// <summary>
        /// Maintain location relative to camera
        /// </summary>
        private void LateUpdate()
        {
            if (!(camera1 is null))
                if (!(camera3 is null))
                    cameraPos = camera2.transform.position;

            //Change position of the objects to align perfectly with outer-edge of screen
            colliders["Right"].position = new Vector3(cameraPos.x + screenSize.x + (colliders["Right"].localScale.x * 0.5f - offset), cameraPos.y, zAxis);
            colliders["Left"].position = new Vector3(cameraPos.x - screenSize.x - (colliders["Left"].localScale.x * 0.5f - offset), cameraPos.y, zAxis);
            colliders["Top"].position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (colliders["Top"].localScale.y * 0.5f - offset), zAxis);
            colliders["Bottom"].position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (colliders["Bottom"].localScale.y * 0.5f - offset), zAxis);
        }
        /// <summary>
        /// Called when ever a camera trigger box is overlapped
        /// </summary>
        /// <param name="triggerName"></param>
        private static void OnCameraTriggerOverlap(string triggerName)
        {
            switch(triggerName.ToLower())
            {
                case "topcollider":
                    GeneralFunctions.GetGameplayManager().onCameraTopOverlap.Invoke();
                    break;
                case "bottomcollider":
                    GeneralFunctions.GetGameplayManager().onCameraBottomOverlap.Invoke();
                    break;
                case "leftcollider":
                    GeneralFunctions.GetGameplayManager().onCameraLeftOverlap.Invoke();
                    break;
                case "rightcollider":
                    GeneralFunctions.GetGameplayManager().onCameraRightOverlap.Invoke();
                    break;
            }
        }
    }
}