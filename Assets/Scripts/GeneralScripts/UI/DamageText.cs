using System.Globalization;
using GameplayScripts;
using TMPro;
using UnityEngine;

namespace GeneralScripts.UI
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshPro textMesh;
        private float textSpeed;
        private float upTime;
        private static int sortingOrder;
        private Vector3 textEndPoint;
        private bool startAnimation;
        private float disappearTime = 3f;
        private bool startedFade;

        // Time when the movement started.
        private float startTime;

        // Total distance between the markers.
        private float journeyLength;

        private void Awake()
        {
            Transform transform1;
            textMesh = (transform1 = transform).GetComponent<TextMeshPro>();

            startAnimation = false;

            startedFade = false;

            // Keep a note of the time the movement started.
            startTime = Time.time;

            // Calculate the journey length.
            journeyLength = Vector3.Distance(transform1.position, textEndPoint);
        }

        private void Update()
        {
            if (startAnimation)
            {
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * textSpeed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(transform.position, textEndPoint, fractionOfJourney);

                upTime -= Time.deltaTime;

                if (upTime <= 0)
                {
                    if (!startedFade)
                    {
                        startedFade = true;

                        StartCoroutine(GeneralFunctions.FadeInAndOut(gameObject, false, disappearTime, true));
                    }
                }
            }
        }

        /// <summary>
        /// Will spawn in damage text object
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        /// <param name="upTime"></param>
        /// <param name="disappearTime"></param>
        /// <param name="distance"></param>
        /// <returns>The created DamageText</returns>
        public static DamageText CreateDamageText(float damage, Vector3 position, float speed, float upTime, float disappearTime, float distance)
        {
            Transform damageTextTransform = Instantiate(GameAssets.Instance.damageText, position, Quaternion.identity);
            DamageText spawnedDamageText = damageTextTransform.GetComponent<DamageText>();

            spawnedDamageText.SetupText(damage, speed, upTime, disappearTime, position, distance);

            return spawnedDamageText;
        }

        /// <summary>
        /// Setups the actual text, speed, and end location on the spawned DamageText
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="speed"></param>
        /// <param name="currentUpTime"></param>
        /// <param name="newDisappearTime"></param>
        /// <param name="position"></param>
        /// <param name="distance"></param>
        private void SetupText(float damage, float speed, float currentUpTime, float newDisappearTime, Vector3 position, float distance)
        {
            textEndPoint = GeneralFunctions.GetPoint(transform.up, position, distance);
            
            textMesh.SetText(damage.ToString(CultureInfo.InvariantCulture));
            textSpeed = speed;
            upTime = currentUpTime;

            sortingOrder++;
            textMesh.sortingOrder = sortingOrder;
            this.disappearTime = newDisappearTime;

            startAnimation = true;
        }
    }
}