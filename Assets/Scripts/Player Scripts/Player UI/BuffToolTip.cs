using CodeMonkey.Utils;
using UnityEngine;

public class BuffToolTip : MonoBehaviour
{
    private Button_UI buttonUI = null;

    public void ConstructToolTip(ScriptableBuff buff)
    {
        buttonUI = gameObject.GetComponent<Button_UI>();

        buttonUI.MouseOverOnceFunc = () => ToolTip.ShowToolTip_Static(buff.description);
        buttonUI.MouseOutOnceFunc = () => ToolTip.HideToolTip_Static();
    }
}
