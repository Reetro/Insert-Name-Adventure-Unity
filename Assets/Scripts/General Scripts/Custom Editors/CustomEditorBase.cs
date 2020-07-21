#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class CustomEditorBase : EditorWindow
    {
        /// <summary>
        /// Whether or not this editor should tick
        /// </summary>
        protected void ShouldTick(bool value)
        {
            _ShouldTick = value;
        }

        protected bool _ShouldTick { get; private set; } = false;

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
        /// <summary>
        /// Finds all items in the provided path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>An array of strings</returns>
        public static List<String> FindObjectsAtPath(string path)
        {
            string[] assetsPaths = AssetDatabase.GetAllAssetPaths();

            List<string> prefabsPaths = new List<string>();

            foreach (string assetPath in assetsPaths)
            {
                if (assetPath.Contains(path))
                {
                    prefabsPaths.Add(assetPath);
                }
            }

            return prefabsPaths;
        }
    }
}
#endif