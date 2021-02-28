using System.Text;
using GeneralScripts;
using GeneralScripts.CustomEditors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayScripts
{
    public abstract class ScriptableItem : ScriptableObject
    {
        protected bool hasArtwork = false;

        [Tooltip("Name of the item")]
        [SerializeField] private new string name;

        [TextArea(10, 14)]
        [Tooltip("Description about what the item does")]
        [SerializeField] private string description;
        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(hasArtwork))]
        [Tooltip("Artwork to display on the item icon")]
        [SerializeField] private Sprite artwork;

        #region Properties
        /// <summary>
        /// Gets the gameplay manager in the current scene
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public GameplayManager MyGameplayManager { get; private set; }
        /// <summary>
        /// Gets the name of the item
        /// </summary>
        public string Name => name;
        /// <summary>
        /// Gets the description of the item
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public string Description => description;

        /// <summary>
        /// Gets the art icon assigned to this item
        /// </summary>
        public Sprite Artwork => artwork;

        /// <summary>
        /// The current stack count on the given item
        /// </summary>
        private int CurrentStackCount { get; set; }
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
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnStart();
        }
        /// <summary>
        /// Called when a scene is loaded
        /// </summary>
        protected virtual void OnStart()
        {
            MyGameplayManager = GeneralFunctions.GetGameplayManager();
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