using UnityEngine;
using Cinemachine;

namespace ComponentLibrary
{
    public class CameraShake : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera = null;
        private float shakeTimer = 0f;
        private float shakeTimerToatal = 0f;
        private float startingIntensity = 0f;

        /// <summary>
        /// Get the virtual camera in the current scene
        /// </summary>
        private void Awake()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

            if (!virtualCamera)
            {
                Debug.LogError(gameObject.name + " failed to get virtualCamera");
            }
        }
        /// <summary>
        /// Shack the virtual camera with the given intensity
        /// </summary>
        /// <param name="intensity"></param>
        /// <param name="time"></param>
        public void ShakeCamera(float intensity, float time)
        {
            var perlinChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            perlinChannel.m_AmplitudeGain = intensity;

            startingIntensity = intensity;
            shakeTimer = time;
            shakeTimerToatal = time;
        }
        /// <summary>
        /// Track shake time and slowly fade it out
        /// </summary>
        private void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    var perlinChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    perlinChannel.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerToatal));
                }
            }
        }
    }
}