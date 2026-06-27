using System;
using System.IO;
using UnityEngine;

namespace Match3.SaveSystem
{
    [Serializable]
    public class SavedObject<T> where T : class
    {
        #region Constructor

        public SavedObject(string fileName)
        {
            _filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.akd");
            if (File.Exists(_filePath)) LoadData();
        }

        #endregion

        public T data;

        private string _filePath;

        public bool IsLoaded { get; private set; }

        public void LoadData()
        {
            if (!File.Exists(_filePath)) return;
            var encrypted = File.ReadAllText(_filePath);
            var json = Cryptography.Decrypt(encrypted);
            data = JsonUtility.FromJson<T>(json);
            IsLoaded = true;
        }

        public void SaveData()
        {
            var json = JsonUtility.ToJson(data);
            var encrypted = Cryptography.Encrypt(json);
            File.WriteAllText(_filePath, encrypted);
        }

        public void Delete()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
        }

        public T CopyData()
        {
            var json = JsonUtility.ToJson(data);
            return JsonUtility.FromJson<T>(json);
        }
    }
}