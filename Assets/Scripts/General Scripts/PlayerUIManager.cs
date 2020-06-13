using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup buffGridLayoutGroup = null;
    [SerializeField] GridLayoutGroup debuffGridLayoutGroup = null;
    [SerializeField] BuffIcon buffIconPrefab = null;

    private List<BuffIcon> buffIcons = new List<BuffIcon>();

    public BuffIcon AddBuffIcon(ScriptableBuff buff)
    {
        BuffIcon icon = Instantiate(buffIconPrefab, buffGridLayoutGroup.transform);
        buffIcons.Add(icon);

        icon.StartCooldown(buff);

        return icon;
    }

    public void RemoveIcon(BuffIcon iconToRemove)
    {
        buffIcons.Remove(iconToRemove);

        Destroy(iconToRemove.gameObject);
    }
}
