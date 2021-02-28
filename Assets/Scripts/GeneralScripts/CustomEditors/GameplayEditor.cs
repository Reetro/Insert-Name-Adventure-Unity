#if UNITY_EDITOR
using StatusEffects;
using UnityEditor;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    public class GameplayEditor : EditorWindow
    {
        #region Status Effects Editors
        private Editor leechingSeEditor;
        private Editor playerSlowSeEditor;
        private Editor healSeEditor;
        #endregion

        #region Spell Editors
        private Editor playerDashEditor;
        #endregion

        #region Enemy Editors

        private PlayerEditor playerEditor;
        private LeechEditor leechEditor;
        private LeechFatherEditor fatherEditor;
        private LeechMotherEditor motherEditor;
        private ShamanEditor shamanEditor;
        private AxeThrowerEditor throwerEditor;
        private SlugEditor slugEditor;
        private TikiHeadEditor headEditor;
        private GameplayManagerEditor managerEditor;
        #endregion

        private Vector2 scrollPosition = Vector2.zero;
        private int tabs;
        private const int IndentLevel = 1;

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
                leechingSeEditor = Editor.CreateEditor(leechEffect);
            }
            else
            {
                Debug.LogError("Failed to get leechEffect in SetupStatusEffects Function in GameplayEditor");
            }

            if (playerSlowEffect)
            {
                playerSlowSeEditor = Editor.CreateEditor(playerSlowEffect);
            }
            else
            {
                Debug.LogError("Failed to get playerSlowEffect in SetupStatusEffects Function in GameplayEditor");
            }

            if (healEffect)
            {
                healSeEditor = Editor.CreateEditor(healEffect);
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
            tabs = GUILayout.Toolbar(tabs, new[] { "Player Settings", "Enemy Settings", "Game Settings", "Status Effects", "Spells" });

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
                EditorGUI.indentLevel += IndentLevel;

                leechEditor.OnInspectorGUI();
            }

            if (Foldout("Leech Father Settings", this))
            {
                EditorGUI.indentLevel += IndentLevel;

                fatherEditor.OnInspectorGUI();
            }

            if (Foldout("Leech Mother Settings", this))
            {
                EditorGUI.indentLevel += IndentLevel;

                motherEditor.OnInspectorGUI();
            }

            if (Foldout("Shaman Settings", this))
            {
                EditorGUI.indentLevel += IndentLevel;

                shamanEditor.OnInspectorGUI();
            }

            if (Foldout("Axe Thrower Settings", this))
            {
                EditorGUI.indentLevel += IndentLevel;

                throwerEditor.OnInspectorGUI();
            }

            if (Foldout("Slug Settings", this))
            {
                EditorGUI.indentLevel += IndentLevel;

                slugEditor.OnInspectorGUI();
            }

            if (Foldout("Tiki Head Settings", this))
            {
                EditorGUI.indentLevel += IndentLevel;

                headEditor.OnInspectorGUI();
            }
        }
        #endregion

        #region Status Effect UI
        private void SetupStatusEffectsUI()
        {
            if (Foldout("Leeching", leechingSeEditor.target))
            {
                EditorGUI.indentLevel += IndentLevel;

                leechingSeEditor.OnInspectorGUI();
            }

            if (Foldout("Player Slow", playerSlowSeEditor.target))
            {
                EditorGUI.indentLevel += IndentLevel;

                playerSlowSeEditor.OnInspectorGUI();
            }

            if (!Foldout("Heal", healSeEditor.target)) return;
            EditorGUI.indentLevel += IndentLevel;

            healSeEditor.OnInspectorGUI();
        }
        #endregion

        #region Spell UI
        private void SetupSpellUI()
        {
            if (!Foldout("Player Dash", playerDashEditor.target)) return;
            EditorGUI.indentLevel += IndentLevel;

            playerDashEditor.OnInspectorGUI();
        }
        #endregion

        private static bool Foldout(string newName, Object @object)
        {
            var prefKey = @object.GetInstanceID() + ".Foldout." + newName;

            var foldoutState = EditorPrefs.GetBool(prefKey, false);
            
            var newFoldoutState = EditorGUILayout.Foldout(foldoutState, newName, true);
            
            if (newFoldoutState != foldoutState)
            {
                EditorPrefs.SetBool(prefKey, newFoldoutState);
            }
            
            return newFoldoutState;
        }
    }
}
#endif
      