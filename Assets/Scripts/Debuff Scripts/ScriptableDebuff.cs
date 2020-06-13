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

    [Tooltip("How often the debuff effect is fired off")]
    public float occurrence;

    public float GetTotalTime()
    {
        return ticks * occurrence;
    }
}
