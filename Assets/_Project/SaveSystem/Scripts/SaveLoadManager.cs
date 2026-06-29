using UnityEngine;

namespace Match3.SaveSystem
{
    [DefaultExecutionOrder(-10)]
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] private bool autoSaveOnClose = true;
        [SerializeField] private BaseSavedScriptableObject[] savedObjects;

        private void InitializeAll()
        {
            foreach (var savedObject in savedObjects)
            {
                savedObject.Initialize();
            }
        }

        private void SaveAll()
        {
            foreach (var savedObject in savedObjects)
            {
                savedObject.Save();
            }
        }

        private void LoadAll()
        {
            foreach (var savedObject in savedObjects)
            {
                savedObject.Load();
            }
        }

        private void ResetAll()
        {
            foreach (var savedObject in savedObjects)
            {
                savedObject.Reset();
            }
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            InitializeAll();
        }

        private void OnDestroy()
        {
            if (!autoSaveOnClose) return;
            SaveAll();
        }

        #endregion
    }
}