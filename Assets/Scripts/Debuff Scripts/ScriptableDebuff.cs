using UnityEngine;

[CreateAssetMenu(fileName = "Debuff", menuName = "Create New Debuff")]
public class ScriptableDebuff : ScriptableObject
{
    [Tooltip("Name of the debuff")]
    public new string name;
    [Tooltip("Description about what the debuff does")]
    public string description;

    [Tooltip("Artwork to display on the debuff icon")]
    public Sprite artwork;

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