using UnityEngine;

namespace Match3.SaveSystem
{
    public abstract class BaseSavedScriptableObject : ScriptableObject
    {
        public abstract void Initialize();
        public abstract void Save();
        public abstract void Load();
        public abstract void Reset();
    }
    
    public class BaseSavedScriptableObject<T> : BaseSavedScriptableObject where T : class
    {
        [SerializeField] private SavedObject<T> defaultValues;

        protected SavedObject<T> SavedObject { get; private set; }

        public override void Initialize()
        {
            SavedObject = new SavedObject<T>(name);
            if (SavedObject.IsLoaded) return;
            SavedObject.data = defaultValues.CopyData();
            SavedObject.SaveData();
        }

        public override void Save() => SavedObject?.SaveData();
        public override void Load() => SavedObject?.LoadData();

        public override void Reset()
        {
            SavedObject?.Delete();
            SavedObject = new SavedObject<T>(name)
            {
                data = defaultValues.CopyData()
            };
            SavedObject.SaveData();
        }
    }
}