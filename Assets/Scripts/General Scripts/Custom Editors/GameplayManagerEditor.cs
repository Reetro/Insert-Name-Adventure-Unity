using UnityEditor;
using UnityEngine;
using GameplayManagement;
using UnityEditor.SceneManagement;

namespace CustomEditors
{
    public class GameplayManagerEditor : Editor
    {
        #region Gameplay Manager Variables
        private SerializedProperty _ManagerTextSpeed;
        private SerializedProperty _ManagerTextUpTime;
        private SerializedProperty _ManagerTextDisappearTime;
        private SerializedProperty _ManagerNameFontSize;
        private SerializedProperty _ManagerDescriptionFontSize;
        private SerializedProperty _ManagerWhatCanBeDamaged;
        private SerializedProperty _ManagerDefaultControllerCheckTimer;
        private SerializedProperty _ManagerDebugSave;
        private SerializedProperty _ManagerPlayerStartingSpells;
        private SerializedProperty _ManagerTextDistance;
        #endregion

        #region Gameplay Manager Editors
        private Editor gameplayManagerEditor;
        #endregion

        #region Gameplay Manager Objects
        private SerializedObject gameplayManagerObject;
        #endregion

        private void OnEnable()
        {
            SetupGameplayManager();
        }

        public override void OnInspectorGUI()
        {
            SetupGameplayManagerUI();
        }

        #region Gameplay Manager Functions
        private void SetupGameplayManager()
        {
            SetupGameplayManagerEditor();

            SetGameplayManagerVars();
        }

        private void SetupGameplayManagerEditor()
        {
            var managerPrefab = Resources.Load("Player/Player") as GameObject;

            if (managerPrefab)
            {
                gameplayManagerEditor = Editor.CreateEditor(managerPrefab.GetComponentInChildren<GameplayManager>());

                gameplayManagerObject = new SerializedObject(managerPrefab.GetComponentInChildren<GameplayManager>());
            }
            else
            {
                Debug.LogError("Failed to get managerPrefab in Gameplay Editor");
            }
        }
        private void SetGameplayManagerVars()
        {
            _ManagerDescriptionFontSize = gameplayManagerObject.FindProperty("descriptionFontSize");
            _ManagerNameFontSize = gameplayManagerObject.FindProperty("nameFontSize");
            _ManagerWhatCanBeDamaged = gameplayManagerObject.FindProperty("whatCanBeDamaged");
            _ManagerTextSpeed = gameplayManagerObject.FindProperty("combatTextSpeed");
            _ManagerTextUpTime = gameplayManagerObject.FindProperty("combatTextUpTime");
            _ManagerTextDisappearTime = gameplayManagerObject.FindProperty("disappearTime");
            _ManagerTextDistance = gameplayManagerObject.FindProperty("textDistance");
            _ManagerDefaultControllerCheckTimer = gameplayManagerObject.FindProperty("defaultControllerCheckTimer");
            _ManagerPlayerStartingSpells = gameplayManagerObject.FindProperty("startingSpells");
            _ManagerDebugSave = gameplayManagerObject.FindProperty("debugSave");
        }
        #endregion

        #region Gameplay Manager UI
        private void SetupGameplayManagerUI()
        {
            // fetch current values from the target
            gameplayManagerObject.Update();
                
            if (gameplayManagerEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                gameplayManagerEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    gameplayManagerObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    gameplayManagerObject.Update();
                }
            }

            // Apply values to the target
            gameplayManagerObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion
    }
}