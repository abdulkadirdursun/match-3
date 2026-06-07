using System.Reflection;
using Match3.Core.Helpers.Editor;
using UnityEditor;
using UnityEngine;

namespace Match3.LevelSystem.Editor
{
    [CustomEditor(typeof(LevelDatabase))]
    public class LevelDatabaseEditor : UnityEditor.Editor
    {
        private const string LevelDataArrayFieldName = "levelDataArray";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("RefreshDatabase"))
                RefreshDatabase();
        }

        private void RefreshDatabase()
        {
            var field = typeof(LevelDatabase).GetField(LevelDataArrayFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                Debug.LogError($"There is no field named {LevelDataArrayFieldName} in the type {nameof(LevelDatabase)}");
                return;
            }

            var database = (LevelDatabase)target;
            var levelDataArray = EditorUtilities.GetInstances<LevelData>();
            field.SetValue(database, levelDataArray);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssetIfDirty(database);
        }
    }
}