using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Create New Buff")]
public class ScriptableBuff : ScriptableObject
{
    [Tooltip("Name of the buff")]
    public new string name;
    [Tooltip("Description about what the buff does")]
    public string description;

    [Tooltip("Artwork to display on the buff icon")]
    public Sprite artwork;

    [Tooltip("This is the actual code that is called that applies the buff")]
    public BuffEffect buffEffect;

    [Tooltip("This kind of depends on the buff but an example of this might is the player's health")]
    public float buffAmount;
    [Tooltip("Time the buff lasts in seconds")]
    public float duration;
}
