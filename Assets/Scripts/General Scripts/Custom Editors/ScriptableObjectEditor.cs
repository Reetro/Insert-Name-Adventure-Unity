using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CustomEditors
{
    public class ScriptableObjectEditor : CustomEditorBase
    {
        private List<ScriptableDebuff> scriptableDebuffs = new List<ScriptableDebuff>();
        private List<ScriptableBuff> scriptableBuffs = new List<ScriptableBuff>();

        private List<Editor> MyScriptableObjectDebuffEditors = new List<Editor>();
        private List<Editor> MyScriptableObjectBuffEditors = new List<Editor>();
        private Vector2 scrollPosition = Vector2.zero;

        public override void OnEnable()
        {
            base.OnEnable();

            AddItems();
        }

        public override void UpdateWindow()
        {
            AddItems();
        }

        private void AddItems()
        {
            scriptableDebuffs = GeneralFunctions.GetAllScriptInstances<ScriptableDebuff>().ToList();
            scriptableBuffs = GeneralFunctions.GetAllScriptInstances<ScriptableBuff>().ToList();

            MyScriptableObjectBuffEditors.Clear();
            MyScriptableObjectDebuffEditors.Clear();

            foreach (ScriptableDebuff debuff in scriptableDebuffs)
            {
                var editor = Editor.CreateEditor(debuff);

                MyScriptableObjectDebuffEditors.Add(editor);
            }

            foreach (ScriptableBuff buff in scriptableBuffs)
            {
                var editor = Editor.CreateEditor(buff);

                MyScriptableObjectBuffEditors.Add(editor);
            }
        }

        [MenuItem("Window/Gameplay Editor")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableObjectEditor>("Gameplay Editor");
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

            GUILayout.Label("Debuffs", EditorStyles.boldLabel);

            foreach (Editor editor in MyScriptableObjectDebuffEditors)
            {
                editor.OnInspectorGUI();
            }

            GUILayout.Space(50f);

            GUILayout.Label("Buffs", EditorStyles.boldLabel);

            foreach (Editor editor in MyScriptableObjectBuffEditors)
            {
                editor.OnInspectorGUI();
            }

            GUILayout.EndScrollView();
        }
    }
}