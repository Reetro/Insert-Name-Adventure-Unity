using GameplayScripts;
using UnityEditor;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    public class GameplayManagerEditor : Editor
    {
        #region Gameplay Manager Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerTextSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerTextUpTime;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerTextDisappearTime;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerNameFontSize;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerDescriptionFontSize;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerWhatCanBeDamaged;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerDefaultControllerCheckTimer;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerDebugSave;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerPlayerStartingSpells;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty managerTextDistance;
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
            managerDescriptionFontSize = gameplayManagerObject.FindProperty("descriptionFontSize");
            managerNameFontSize = gameplayManagerObject.FindProperty("nameFontSize");
            managerWhatCanBeDamaged = gameplayManagerObject.FindProperty("whatCanBeDamaged");
            managerTextSpeed = gameplayManagerObject.FindProperty("combatTextSpeed");
            managerTextUpTime = gameplayManagerObject.FindProperty("combatTextUpTime");
            managerTextDisappearTime = gameplayManagerObject.FindProperty("disappearTime");
            managerTextDistance = gameplayManagerObject.FindProperty("textDistance");
            managerDefaultControllerCheckTimer = gameplayManagerObject.FindProperty("defaultControllerCheckTimer");
            managerPlayerStartingSpells = gameplayManagerObject.FindProperty("startingSpells");
            managerDebugSave = gameplayManagerObject.FindProperty("debugSave");
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

            GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
        }
        #endregion
    }
}