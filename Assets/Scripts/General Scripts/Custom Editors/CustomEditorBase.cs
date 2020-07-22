#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class CustomEditorBase : EditorWindow
    {
        /// <summary>
        /// Whether or not this editor should tick
        /// </summary>
        protected bool _ShouldTick { get; set; } = false;

        public virtual void OnDisable()
        {
            if (_ShouldTick)
            {
                EditorApplication.update -= Update;
            }
        }

        public virtual void OnEnable()
        {
            if (_ShouldTick)
            {
                EditorApplication.update += Update;
            }
        }

        /// <summary>
        /// Called update every frame while window is visible only called if shouldTick is true
        /// </summary>
        protected virtual void Update()
        {
            // To be used in children
        }
        /// <summary>
        /// Get all ScriptableObjects of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An array of ScriptableObjects</returns>
        public static T[] GetAllScriptInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;
        }
    }
}
#endif