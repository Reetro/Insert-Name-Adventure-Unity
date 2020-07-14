using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private Text toolTipText;
    private RectTransform backgroundRectTransform;
    private static ToolTip instance;

    private void Awake()
    {
        backgroundRectTransform = GameObject.Find("ToolTipBackground").GetComponent<RectTransform>();
        toolTipText = GameObject.Find("ToolTipText").GetComponent<Text>();

        instance = this;

        print(instance);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void ShowTooltip(string text)
    {
        gameObject.SetActive(true);

        toolTipText.text = text;

        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + GeneralFunctions.GetPlayerUIManager().toolTipTextPadding * 2, toolTipText.preferredHeight + GeneralFunctions.GetPlayerUIManager().toolTipTextPadding * 2);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string text)
    {
        instance.ShowTooltip(text);
    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }
}
