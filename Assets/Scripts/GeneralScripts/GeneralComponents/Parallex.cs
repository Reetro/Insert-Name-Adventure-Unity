using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class Parallex : MonoBehaviour
    {
        public Transform[] backgrounds;
        private float[] parallaxScales;
        public float smoothing = 1f;

        private Transform cam;
        private Vector3 previousCamPos;

        // ReSharper disable Unity.PerformanceAnalysis
        private void Awake()
        {
            if (!(Camera.main is null)) cam = Camera.main.transform;
        }
        // Start is called before the first frame update 
        private void Start()
        {
            previousCamPos = cam.position;
            parallaxScales = new float[backgrounds.Length];

            for (var i = 0; i < backgrounds.Length; i++)
            {
                parallaxScales[i] = backgrounds[i].position.z * -1;
            }

        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            // Calculates parallax depending on asset z distance from camera
            for (var i = 0; i < backgrounds.Length; i++)
            {
                var position = cam.position;
                var parallaxX = (previousCamPos.x - position.x) * parallaxScales[i];
                var parallaxY = (previousCamPos.y - position.y) * parallaxScales[i];
                var backgroundTargetPosX = backgrounds[i].position.x + parallaxX;
                var backgroundTargetPosY = backgrounds[i].position.y + parallaxY;
                var backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);
                backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
            }
            previousCamPos = cam.position;
        }
    }
}