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
    [SerializeField] DebuffIcon debuffIconPrefab = null;

    private List<BuffIcon> buffIcons = new List<BuffIcon>();
    private List<DebuffIcon> debuffIcons = new List<DebuffIcon>();

    public BuffIcon AddBuffIcon(ScriptableBuff buff)
    {
        BuffIcon icon = Instantiate(buffIconPrefab, buffGridLayoutGroup.transform);
        buffIcons.Add(icon);

        icon.StartCooldown(buff);

        return icon;
    }

    public void RemoveBuffIcon(BuffIcon iconToRemove)
    {
        buffIcons.Remove(iconToRemove);

        Destroy(iconToRemove.gameObject);
    }

    public DebuffIcon AddDebuffIcon(ScriptableDebuff debuff, bool hasFillAmount, bool useTick)
    {
        DebuffIcon icon = Instantiate(debuffIconPrefab, debuffGridLayoutGroup.transform);
        debuffIcons.Add(icon);

        icon.StartCooldown(debuff, hasFillAmount, useTick);

        return icon;
    }

    public void RemoveDebuffIcon(DebuffIcon iconToRemove)
    {
        debuffIcons.Remove(iconToRemove);

        Destroy(iconToRemove.gameObject);
    }
}
