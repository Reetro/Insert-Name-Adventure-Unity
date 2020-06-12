using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private TextMeshPro textMesh = null;
    private float textSpeed = 0f;
    private float upTime = 0f;
    private Color textColor;
    private static int sortingOrder;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        transform.position += new Vector3(textSpeed, textSpeed) * Time.deltaTime;

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

    public static DamageText CreateDamageText(float damage, Vector3 position, float speed, float upTime)
    {
        Transform damageTextTransform = Instantiate(GameAssets.instance.damgeText, position, Quaternion.identity);
        DamageText spawnedDamageText = damageTextTransform.GetComponent<DamageText>();

        spawnedDamageText.SetupText(damage, speed, upTime);

        return spawnedDamageText;
    }

    public void SetupText(float damage, float speed, float currentUpTime)
    {
        textMesh.SetText(damage.ToString());
        textSpeed = speed;
        upTime = currentUpTime;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        textColor = textMesh.color;
    }
}
