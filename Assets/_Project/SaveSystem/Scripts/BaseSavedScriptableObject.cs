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
}