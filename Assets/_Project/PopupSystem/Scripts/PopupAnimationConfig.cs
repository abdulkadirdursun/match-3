using DG.Tweening;
using UnityEngine;

namespace Match3.PopupSystem
{
    [CreateAssetMenu(fileName = "PopupAnimationConfig", menuName = "Match 3/Popup System/Popup Animation Config")]
    public class PopupAnimationConfig : ScriptableObject
    {
        [Header("Open Animation")]
        [SerializeField] private float openTime = 0.3f;
        [SerializeField] private Ease openEase = Ease.OutBack;
        [Header("Close Animation")]
        [SerializeField] private float closeTime = 0.2f;
        [SerializeField] private Ease closeEase = Ease.InCubic;
        [Header("Scale")]
        [SerializeField] private float startScaleAtOpen = .9f;
        [SerializeField] private float targetScaleAtClose = .85f;

        public float OpenTime => openTime;
        public Ease OpenEase => openEase;
        public float CloseTime => closeTime;
        public Ease CloseEase => closeEase;
        public float StartScaleAtOpen => startScaleAtOpen;
        public float TargetScaleAtClose => targetScaleAtClose;
    }
}