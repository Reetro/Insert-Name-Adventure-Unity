using UnityEngine;
using TMPro;


public class DamageText : MonoBehaviour
{
    private TextMeshPro textMesh = null;
    private float textSpeed = 0f;
    private float upTime = 0f;
    private Color textColor;
    private static int sortingOrder;
    private Vector3 textEndPoint;
    private bool startAnimation = false;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();

        startAnimation = false;

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
                float dissapearTime = 3f;

                textColor.a -= dissapearTime * Time.deltaTime;

                if (textColor.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public static DamageText CreateDamageText(float damage, Vector3 position, float speed, float upTime, float endPointMinX, float endPointMaxX, float endPointMinY, float endPointMaxY)
    {
        Transform damageTextTransform = Instantiate(GameAssets.instance.damgeText, position, Quaternion.identity);
        DamageText spawnedDamageText = damageTextTransform.GetComponent<DamageText>();

        spawnedDamageText.SetupText(damage, speed, upTime, endPointMinX, endPointMaxX, endPointMinY, endPointMaxY);

        return spawnedDamageText;
    }

    private void SetupText(float damage, float speed, float currentUpTime, float endPointMinX, float endPointMaxX, float endPointMinY, float endPointMaxY)
    { 
        textEndPoint = GeneralFunctions.CreateRandomVector2(endPointMinX, endPointMaxX, endPointMinY, endPointMaxY);

        textMesh.SetText(damage.ToString());
        textSpeed = speed;
        upTime = currentUpTime;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        textColor = textMesh.color;

        startAnimation = true;
    }
}
