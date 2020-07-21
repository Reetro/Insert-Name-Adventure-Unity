using UnityEngine;
using PlayerUI.ToolTipUI;
using AuraSystem.Effects;

[CreateAssetMenu(fileName = "Buff", menuName = "Create New Buff")]
public class ScriptableBuff : ScriptableItem
{
    [Tooltip("This is the actual code that is called that applies the buff")]
    public BuffEffect buffEffect;

    [Tooltip("If a another buff of this type is applied to the player should the buff restart")]
    public bool refresh = false;

    [Tooltip("If a another buff of this type is applied should stack count be increased")]
    public bool stack = false;

    [Tooltip("This kind of depends on the buff but an example of this might is the player's health")]
    public float buffAmount;

    [Tooltip("Time the buff lasts in seconds")]
    public float duration;

    [Tooltip("Visual effect that is attached to target")]
    public GameObject visualEffect;

    public enum BuffType
    {
        Breather
    };

    public BuffType buffType;
}
