using CodeMonkey.Utils;
using UnityEngine;

public class DebuffToolTip : MonoBehaviour
{
    private Button_UI buttonUI = null;

    public void ConstructToolTip(ScriptableDebuff debuff)
    {
        buttonUI = gameObject.GetComponent<Button_UI>();

        buttonUI.MouseOverOnceFunc = () => ToolTip.ShowToolTip_Static(debuff.description);
        buttonUI.MouseOutOnceFunc = () => ToolTip.HideToolTip_Static();
    }
}
