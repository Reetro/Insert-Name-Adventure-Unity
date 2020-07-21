#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class AuraEditor : CustomEditorBase
    {
        // Buffs and Debuff editors
        private List<Editor> MyScriptableObjectDebuffEditors = new List<Editor>();
        private List<Editor> MyScriptableObjectBuffEditors = new List<Editor>();
        private Vector2 scrollPosition = Vector2.zero;

        // Buffs and Debuff lists
        private List<ScriptableDebuff> scriptableDebuffs = new List<ScriptableDebuff>();
        private List<ScriptableBuff> scriptableBuffs = new List<ScriptableBuff>();

        public override void OnEnable()
        {
            base.OnEnable();

            AddScriptableObjects();
        }

        public override void UpdateWindow()
        {
            AddScriptableObjects();
        }

        [MenuItem("Window/Aura Editor")]
        public static void ShowWindow()
        {
            GetWindow<AuraEditor>("Aura Editor");
        }

        private void AddScriptableObjects()
        {
            scriptableDebuffs = GetAllScriptInstances<ScriptableDebuff>().ToList();
            scriptableBuffs = GetAllScriptInstances<ScriptableBuff>().ToList();

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

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

            /// Debuff area start
            GUILayout.Label("Debuffs", EditorStyles.boldLabel);

            foreach (Editor editor in MyScriptableObjectDebuffEditors)
            {
                editor.OnInspectorGUI();
            }

            /// Debuff area end

            GUILayout.Space(50f);

            /// Buff area start
            GUILayout.Label("Buffs", EditorStyles.boldLabel);

            foreach (Editor editor in MyScriptableObjectBuffEditors)
            {
                editor.OnInspectorGUI();
            }

            /// Buff area end

            GUILayout.EndScrollView();
        }
    }
}
#endif