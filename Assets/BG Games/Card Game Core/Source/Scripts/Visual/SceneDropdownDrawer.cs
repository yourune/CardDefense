using UnityEditor;
using UnityEngine;

namespace Template.H_lib.Utils
{
    public class SceneDropdownAttribute : PropertyAttribute
    {
    }
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
    public class SceneDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] sceneNames = GetSceneNames();
            int currentIndex = property.intValue;
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneNames);
            
            if (selectedIndex != currentIndex)
            {
                property.intValue = selectedIndex;
            }
        }

        private string[] GetSceneNames()
        {
            int sceneCount = UnityEditor.EditorBuildSettings.scenes.Length;
            string[] sceneNames = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorBuildSettings.scenes[i].path);
            }

            return sceneNames;
        }
    }
    #endif
}