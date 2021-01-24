#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using StatusEffects;

namespace CustomEditors
{
    public class GameplayEditor : EditorWindow
    {
        #region Status Effects Editors
        private Editor leechingSEEditor = null;
        private Editor playerSlowSEEditor = null;
        private Editor healSEEditor = null;
        #endregion

        #region Spell Editors
        private Editor playerDashEditor = null;
        #endregion

        #region Enemy Editors
        PlayerEditor playerEditor = null;
        LeechEditor leechEditor = null;
        LeechFatherEditor fatherEditor = null;
        LeechMotherEditor motherEditor = null;
        ShamanEditor shamanEditor = null;
        AxeThrowerEditor throwerEditor = null;
        SlugEditor slugEditor = null;
        TikiHeadEditor headEditor = null;
        GameplayManagerEditor managerEditor = null;
        #endregion

        private Vector2 scrollPosition = Vector2.zero;
        private const float foldoutSpaceing = 10f;
        int tabs = 0;
        private const int indentLevel = 1;

        [MenuItem("Window/Gameplay Editor")]
        public static void ShowWindow()
        {
            GetWindow<GameplayEditor>("Gameplay Editor");
        }

        private void OnEnable()
        {
            playerEditor = CreateInstance<PlayerEditor>();

            managerEditor = CreateInstance<GameplayManagerEditor>();

            CreateEnemyEditorInstances();

            SetupStatusEffects();

            SetupSpells();
        }

        private void CreateEnemyEditorInstances()
        {
            leechEditor = CreateInstance<LeechEditor>();

            fatherEditor = CreateInstance<LeechFatherEditor>();

            motherEditor = CreateInstance<LeechMotherEditor>();

            shamanEditor = CreateInstance<ShamanEditor>();

            throwerEditor = CreateInstance<AxeThrowerEditor>();

            slugEditor = CreateInstance<SlugEditor>();

            headEditor = CreateInstance<TikiHeadEditor>();
        }

        #region Status Effect Functions
        private void SetupStatusEffects()
        {
            var leechEffect = Resources.Load("Status Effects/Leeching_SSE") as ScriptableStatusEffect;
            var playerSlowEffect = Resources.Load("Status Effects/PlayerSlowing_SSE") as ScriptableStatusEffect;
            var healEffect = Resources.Load("Status Effects/Heal_SSE");

            if (leechEffect)
            {
                leechingSEEditor = Editor.CreateEditor(leechEffect);
            }
            else
            {
                Debug.LogError("Failed to get leechEffect in SetupStatusEffects Function in GameplayEditor");
            }

            if (playerSlowEffect)
            {
                playerSlowSEEditor = Editor.CreateEditor(playerSlowEffect);
            }
            else
            {
                Debug.LogError("Failed to get playerSlowEffect in SetupStatusEffects Function in GameplayEditor");
            }

            if (healEffect)
            {
                healSEEditor = Editor.CreateEditor(healEffect);
            }
            else
            {
                Debug.LogError("Failed to get healEffect in SetupStatusEffects Function in GameplayEditor");
            }
        }
        #endregion

        #region Spell Functions
        private void SetupSpells()
        {
            var playerDash = Resources.Load("Spells/Player_Dash_S");

            if (playerDash)
            {
                playerDashEditor = Editor.CreateEditor(playerDash);
            }
            else
            {
                Debug.LogError("Failed to get playerDash in Spell Functions in GameplayEditor");
            }
        }
        #endregion

        public void OnGUI()
        {
            tabs = GUILayout.Toolbar(tabs, new string[] { "Player Settings", "Enemy Settings", "Game Settings", "Status Effects", "Spells" });

            switch (tabs)
            {
                case 0:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    playerEditor.OnInspectorGUI();

                    GUILayout.EndScrollView();
                    break;

                case 1:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupEnemyFoldOuts();

                    GUILayout.EndScrollView();
                    break;

                case 2:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    managerEditor.OnInspectorGUI();

                    GUILayout.EndScrollView();
                    break;
                case 3:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupStatusEffectsUI();

                    GUILayout.EndScrollView();
                    break;
                case 4:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupSpellUI();

                    GUILayout.EndScrollView();
                    break;
            }
        }

        #region Enemy UI
        private void SetupEnemyFoldOuts()
        {
            if (Foldout("Leech Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                leechEditor.OnInspectorGUI();
            }

            if (Foldout("Leech Father Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                fatherEditor.OnInspectorGUI();
            }

            if (Foldout("Leech Mother Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                motherEditor.OnInspectorGUI();
            }

            if (Foldout("Shaman Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                shamanEditor.OnInspectorGUI();
            }

            if (Foldout("Axe Thrower Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                throwerEditor.OnInspectorGUI();
            }

            if (Foldout("Slug Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                slugEditor.OnInspectorGUI();
            }

            if (Foldout("Tiki Head Settings", this))
            {
                EditorGUI.indentLevel += indentLevel;

                headEditor.OnInspectorGUI();
            }
        }
        #endregion

        #region Status Effect UI
        private void SetupStatusEffectsUI()
        {
            if (Foldout("Leeching", leechingSEEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                leechingSEEditor.OnInspectorGUI();
            }

            if (Foldout("Player Slow", playerSlowSEEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                playerSlowSEEditor.OnInspectorGUI();
            }

            if (Foldout("Heal", healSEEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                healSEEditor.OnInspectorGUI();
            }
        }
        #endregion

        #region Spell UI
        private void SetupSpellUI()
        {
            if (Foldout("Player Dash", playerDashEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                playerDashEditor.OnInspectorGUI();
            }
        }
        #endregion

        private bool Foldout(string name, Object @object)
        {
            string prefKey = @object.GetInstanceID() + ".Foldout." + name;

            bool foldoutState = EditorPrefs.GetBool(prefKey, false);
            
            bool newFoldoutState = EditorGUILayout.Foldout(foldoutState, name, true);
            
            if (newFoldoutState != foldoutState)
            {
                EditorPrefs.SetBool(prefKey, newFoldoutState);
            }
            
            return newFoldoutState;
        }
    }
}
#endif
      