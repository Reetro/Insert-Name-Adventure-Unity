using GameplayManagement;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        /// <summary>
        /// Gets the gameplay manager in the current scene
        /// </summary>
        public GameplayManager MyGameplayManager { get; private set; } = null;

        public string Name { get { return name; } }

        public string Description { get { return description; } }

        public Sprite Artwork { get { return artwork; } }

        private void OnEnable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnStart();
        }

        protected virtual void OnStart()
        {
            MyGameplayManager = GameObject.FindGameObjectWithTag("Gameplay Manager").GetComponent<GameplayManager>();
        }

        public virtual string GetToolTipInfoText()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<size=" + MyGameplayManager.nameFontSize + ">").Append(Name).Append("</size>").AppendLine();
            builder.Append("<size=" + MyGameplayManager.descriptionFontSize + ">").Append(Description).Append("</size>").AppendLine();

            return builder.ToString();
        }
    }
}