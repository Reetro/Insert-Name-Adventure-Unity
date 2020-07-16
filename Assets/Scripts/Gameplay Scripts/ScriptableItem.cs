using System.Text;
using UnityEngine;

namespace PlayerUI.ToolTipUI
{
    public abstract class ScriptableItem : ScriptableObject
    {
        [Tooltip("Item title font size")]
        [SerializeField] private float titleFontSize = 36f;

        [Tooltip("Name of the item")]
        [SerializeField] private new string name = null;

        [Tooltip("Description about what the item does")]
        [SerializeField] private string description = null;

        [Tooltip("Artwork to display on the item icon")]
        [SerializeField] private Sprite artwork = null;

        public string Name { get { return name; } }

        public string Description { get { return description; } }

        public Sprite Artwork { get { return artwork; } }

        public virtual string GetToolTipInfoText()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<size=" + titleFontSize + ">").Append(Name).Append("</size>").AppendLine();
            builder.Append(Description).AppendLine();

            return builder.ToString();
        }
    }
}