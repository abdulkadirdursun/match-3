using System.Reflection;
using Match3.Core.Helpers.Editor;
using UnityEditor;
using UnityEngine;

namespace Match3.SaveSystem.Editor
{
    [CustomEditor(typeof(SaveLoadManager))]
    public class SaveLoadManagerEditor : UnityEditor.Editor
    {
        private const string SaveAllFunctionName = "SaveAll";
        private const string LoadAllFunctionName = "LoadAll";
        private const string ResetAllFunctionName = "ResetAll";
        private const string SavedScriptableObjectListVariableName = "savedObjects";
        private MethodInfo _saveAllFunction;
        private MethodInfo _loadAllFunction;
        private MethodInfo _resetAllFunction;
        private FieldInfo _savedObjectsField;
        private SaveLoadManager _saveLoadManager;

        private void Awake()
        {
            _saveLoadManager = (SaveLoadManager)target;
            var type = typeof(SaveLoadManager);
            _saveAllFunction = type.GetMethod(SaveAllFunctionName, BindingFlags.Instance | BindingFlags.NonPublic);
            _loadAllFunction = type.GetMethod(LoadAllFunctionName, BindingFlags.Instance | BindingFlags.NonPublic);
            _resetAllFunction = type.GetMethod(ResetAllFunctionName, BindingFlags.Instance | BindingFlags.NonPublic);
            _savedObjectsField = type.GetField(SavedScriptableObjectListVariableName, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("SaveAll"))
                SaveAll();
            if (GUILayout.Button("LoadAll"))
                LoadAll();
            if (GUILayout.Button("ResetAll"))
                ResetAll();
            if (GUILayout.Button("RefreshList"))
                RefreshSavedScriptableObjectList();
        }

        private void LoadAll()
        {
            if (_loadAllFunction == null)
            {
                Debug.LogError($"{LoadAllFunctionName} function is not available in {nameof(SaveLoadManager)}");
                return;
            }

            _loadAllFunction.Invoke(_saveLoadManager, null);
        }

        private void SaveAll()
        {
            if (_saveAllFunction == null)
            {
                Debug.LogError($"{SaveAllFunctionName} function is not available in {nameof(SaveLoadManager)}");
                return;
            }

            _saveAllFunction.Invoke(_saveLoadManager, null);
        }

        private void ResetAll()
        {
            if (_resetAllFunction == null)
            {
                Debug.LogError($"{ResetAllFunctionName} function is not available in {nameof(SaveLoadManager)}");
                return;
            }

            _resetAllFunction.Invoke(_saveLoadManager, null);
        }

        private void RefreshSavedScriptableObjectList()
        {
            if (_savedObjectsField == null)
            {
                Debug.LogError($"{SavedScriptableObjectListVariableName} field is not available in {nameof(SaveLoadManager)}");
                return;
            }

            var savedObjects = EditorUtilities.GetInstances<BaseSavedScriptableObject>();

            _savedObjectsField.SetValue(_saveLoadManager, savedObjects);
            EditorUtility.SetDirty(_saveLoadManager);
        }
    }
}