using UnityEditor;
using UnityEngine;

namespace Match3.Core.Helpers.Editor
{
    public static class EditorUtilities
    {
        public static T[] GetInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            var result = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                var instance = AssetDatabase.GUIDToAssetPath(guids[i]);
                result[i] = AssetDatabase.LoadAssetAtPath<T>(instance);
            }

            return result;
        }
    }
}