using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.PopupSystem
{
    [CreateAssetMenu(fileName = "PopupService", menuName = "Match 3/Popup System/Popup Service")]
    public class PopupService : ScriptableObject
    {
        private Dictionary<Type, PopupBase> _popupRegistry = new();

        public void ShowPopup<T>() where T : PopupBase
        {
            if (!_popupRegistry.TryGetValue(typeof(T), out var popup))
            {
                Debug.LogError($"Popup type {typeof(T).Name} is not registered with the popup service!");
                return;
            }

            popup.Open();
        }

        public void RegisterPopup(PopupBase popup)
        {
            var type = popup.GetType();
            if (!_popupRegistry.TryAdd(type, popup))
            {
                Debug.LogError($"Popup type {type} already exist in the lookup dictionary!");
                return;
            }
        }

        public void UnregisterPopup(PopupBase popup)
        {
            var type = popup.GetType();
            _popupRegistry.Remove(type);
        }
    }
}