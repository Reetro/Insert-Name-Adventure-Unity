using UnityEditor;

namespace CustomEditors
{
    public class CustomEditorBase : EditorWindow
    {
        public virtual void OnDisable()
        {
            EditorApplication.update -= UpdateWindow;
        }

        public virtual void OnEnable()
        {
            EditorApplication.update += UpdateWindow;
        }

        /// <summary>
        /// Called every frame while window is visible
        /// </summary>
        public virtual void UpdateWindow()
        {
            // For use in children
        }
    }
}
