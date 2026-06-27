using UnityEngine;

namespace Match3.SaveSystem
{
    public class GenericSavedScriptableObject<T> : BaseSavedScriptableObject where T : class
    {
        [SerializeField] private SavedObject<T> defaultValues;

        private SavedObject<T> _savedObject;

        public override void Initialize()
        {
            _savedObject = new SavedObject<T>(name);
            if (_savedObject.IsLoaded) return;
            _savedObject.data = defaultValues.CopyData();
            _savedObject.SaveData();
        }

        public override void Save() => _savedObject?.SaveData();
        public override void Load() => _savedObject?.LoadData();

        public override void Reset()
        {
            _savedObject.Delete();
            _savedObject = new SavedObject<T>(name)
            {
                data = defaultValues.CopyData()
            };
            _savedObject.SaveData();
        }
    }
}