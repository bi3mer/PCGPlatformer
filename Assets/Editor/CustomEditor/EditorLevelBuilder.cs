#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace CustomInspector
{
    [CustomEditor(typeof(LevelBuilder))]
    public class EditorLevelBuilder : Editor
    {
        private LevelBuilder levelBuilder = null;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            levelBuilder = (LevelBuilder) target;

            GUILayout.Space(10);

            if (GUILayout.Button("Save"))
            {
                levelBuilder.Save();
            }

            if (GUILayout.Button("Load"))
            {
                levelBuilder.Load();
            }
        }
    }
}
#endif