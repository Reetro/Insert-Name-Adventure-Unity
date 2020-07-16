using UnityEngine;
using PlayerUI.ToolTipUI;
using AuraSystem;

[CreateAssetMenu(fileName = "Debuff", menuName = "Create New Debuff")]
public class ScriptableDebuff : ScriptableItem
{
    [Tooltip("This is the actual code that is called that applies the debuff")]
    public DebuffEffect debuffEffect;

    [Tooltip("How many times the debuff is fired before it's removed")]
    public float ticks;

    [Tooltip("Should this debuff count down it's ticks or make the player have to remove it")]
    public bool useTicks = true;

    [Tooltip("If a another debuff of this type is applied to the player should the debuff restart")]
    public bool refresh = false;

    [Tooltip("If a another debuff of this type is applied should stack count be increased")]
    public bool stack = false;

    [Tooltip("How often the debuff effect is fired off")]
    public float occurrence;

    [Tooltip("Amount of damage to apply to player")]
    public float damage;

    [Tooltip("Visual effect that is attached to target")]
    public GameObject visualEffect;

    public enum DebuffType 
    {
        Leeching,
        Fire
    };

    public DebuffType debuffType;

    public float GetTotalTime()
    {
        return ticks * occurrence;
    }
}