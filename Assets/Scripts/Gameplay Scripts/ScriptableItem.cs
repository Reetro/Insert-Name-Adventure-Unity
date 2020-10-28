using GameplayManagement;
using GameplayManagement.Assets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerUI.ToolTipUI
{
    public abstract class ScriptableItem : ScriptableObject
    {
        protected bool hasArtwork = false;

        [Tooltip("Name of the item")]
        [SerializeField] private new string name = null;

        [TextArea(10, 14)]
        [Tooltip("Description about what the item does")]
        [SerializeField] private string description = null;
        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(hasArtwork))]
        [Tooltip("Artwork to display on the item icon")]
        [SerializeField] private Sprite artwork = null;

        #region Properties
        /// <summary>
        /// Gets the gameplay manager in the current scene
        /// </summary>
        public GameplayManager MyGameplayManager { get; private set; } = null;

        public string Name { get { return name; } }

        public string Description { get { return description; } }

        public Sprite Artwork { get { return artwork; } }

        /// <summary>
        /// The current stack count on the given item
        /// </summary>
        public int CurrentStackCount { get; private set; } = 0;
        #endregion

        #region Item Functions
        /// <summary>
        /// needs to be placed in the item update method will update the ScriptableItem internal stack count
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateToolTip(int amount)
        {
            CurrentStackCount = amount;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnStart();
        }
        /// <summary>
        /// Called when a scene is loaded
        /// </summary>
        protected virtual void OnStart()
        {
            MyGameplayManager = GameAssets.GlobalManager;
        }

        public virtual string GetToolTipInfoText()
        {
            StringBuilder builder = new StringBuilder();

            if (CurrentStackCount > 1)
            {
                builder.Append("<size=" + MyGameplayManager.nameFontSize + ">").Append(Name).Append("</size>").Append(" ").Append("<size=" + MyGameplayManager.nameFontSize + ">").Append("x").Append(CurrentStackCount).Append("</size>").AppendLine();
                builder.Append("<size=" + MyGameplayManager.descriptionFontSize + ">").Append(Description).Append("</size>").AppendLine();

                return builder.ToString();
            }
            else
            {
                builder.Append("<size=" + MyGameplayManager.nameFontSize + ">").Append(Name).Append("</size>").AppendLine();
                builder.Append("<size=" + MyGameplayManager.descriptionFontSize + ">").Append(Description).Append("</size>").AppendLine();

                return builder.ToString();
            }
        }
        #endregion
    }
}