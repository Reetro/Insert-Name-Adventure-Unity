using UnityEngine;
using TMPro;
using GameplayManagement.Assets;

namespace PlayerUI
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshPro textMesh = null;
        private float textSpeed = 0f;
        private float upTime = 0f;
        private static int sortingOrder;
        private Vector3 textEndPoint;
        private bool startAnimation = false;
        private float dissapearTime = 3f;
        private bool startedFade = false;

        // Time when the movement started.
        private float startTime;

        // Total distance between the markers.
        private float journeyLength;

        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();

            startAnimation = false;

            startedFade = false;

            // Keep a note of the time the movement started.
            startTime = Time.time;

            // Calculate the journey length.
            journeyLength = Vector3.Distance(transform.position, textEndPoint);
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

                        StartCoroutine(GeneralFunctions.fadeInAndOut(gameObject, false, dissapearTime));
                    }
                }
            }
        }

        public static DamageText CreateDamageText(float damage, Vector3 position, float speed, float upTime, float dissapearTime)
        {
            Transform damageTextTransform = Instantiate(GameAssets.instance.damgeText, position, Quaternion.identity);
            DamageText spawnedDamageText = damageTextTransform.GetComponent<DamageText>();

            spawnedDamageText.SetupText(damage, speed, upTime, dissapearTime, position);

            return spawnedDamageText;
        }

        private void SetupText(float damage, float speed, float currentUpTime, float dissapearTime, Vector3 position)
        {
            textEndPoint = GeneralFunctions.GetPoint(transform.up, position, 1f);
            
            textMesh.SetText(damage.ToString());
            textSpeed = speed;
            upTime = currentUpTime;

            sortingOrder++;
            textMesh.sortingOrder = sortingOrder;
            this.dissapearTime = dissapearTime;

            startAnimation = true;
        }
    }
}