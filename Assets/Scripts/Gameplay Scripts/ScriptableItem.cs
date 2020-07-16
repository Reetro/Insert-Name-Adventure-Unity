using UnityEngine;

namespace PlayerUI.ToolTipUI
{
    public abstract class ScriptableItem : ScriptableObject
    {
        [Tooltip("Name of the item")]
        [SerializeField] private new string name = null;

        [Tooltip("Description about what the item does")]
        [SerializeField] private string description = null;

        [Tooltip("Artwork to display on the item icon")]
        [SerializeField] private Sprite artwork = null;

        public string Name { get { return name; } }

        public string Description { get { return description; } }

        public Sprite Artwork { get { return artwork; } }

        public abstract string GetToolTipInfo();
    }
}