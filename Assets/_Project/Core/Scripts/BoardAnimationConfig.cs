using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "BoardAnimationConfig", menuName = "Match 3/Core/Board Animation Config")]
    public class BoardAnimationConfig : ScriptableObject
    {
        [Header("Fall Animation")]
        [SerializeField] private float fallDuration = 0.2f;
        [SerializeField] private Ease fallEase = Ease.InOutCubic;
        [SerializeField] private float fallDelay;
        [Header("Swap Animation")]
        [SerializeField] private float swapDuration = 0.15f;
        [SerializeField] private Ease swapEase = Ease.Linear;
        [Header("Failed Swap animation")]
        [SerializeField] private float bounceDuration = 0.3f;
        [SerializeField] private Ease bounceEase = Ease.InOutCubic;

        //Fall
        public float FallDuration => fallDuration;
        public Ease FallEase => fallEase;
        public float FallDelay => fallDelay;
        //Swap
        public float SwapDuration => swapDuration;
        public Ease SwapEase => swapEase;
        //Bounce
        public float BounceDuration => bounceDuration;
        public Ease BounceEase => bounceEase;
    }
}