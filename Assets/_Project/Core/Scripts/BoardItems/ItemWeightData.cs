using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "ItemWeightData", menuName = "Match 3/Core/Item Weight Data")]
    public class ItemWeightData : ScriptableObject
    {
        [SerializeField, Range(0.1f, 20f)] private float baseWeight = 1f;
        [Header("Additional Weight By Same Type Neighbor")]
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtOneNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtTwoNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtThreeNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtFourNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtFiveNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtSixNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtSevenNeighbor;
        [SerializeField, Range(-10f, 10f)] private float additionalWeightAtEightNeighbor;

        public float BaseWeight => baseWeight;

        public float GetAdditionalWeight(int sameTypeNeighborCount)
        {
            sameTypeNeighborCount = Mathf.Clamp(sameTypeNeighborCount, 0, 8);
            return sameTypeNeighborCount switch
            {
                1 => additionalWeightAtOneNeighbor,
                2 => additionalWeightAtTwoNeighbor,
                3 => additionalWeightAtThreeNeighbor,
                4 => additionalWeightAtFourNeighbor,
                5 => additionalWeightAtFiveNeighbor,
                6 => additionalWeightAtSixNeighbor,
                7 => additionalWeightAtSevenNeighbor,
                8 => additionalWeightAtEightNeighbor,
                _ => 0f
            };
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            additionalWeightAtOneNeighbor = Mathf.Max(-baseWeight, additionalWeightAtOneNeighbor);
            additionalWeightAtTwoNeighbor = Mathf.Max(-baseWeight, additionalWeightAtTwoNeighbor);
            additionalWeightAtThreeNeighbor = Mathf.Max(-baseWeight, additionalWeightAtThreeNeighbor);
            additionalWeightAtFiveNeighbor = Mathf.Max(-baseWeight, additionalWeightAtFiveNeighbor);
            additionalWeightAtOneNeighbor = Mathf.Max(-baseWeight, additionalWeightAtOneNeighbor);
            additionalWeightAtSixNeighbor = Mathf.Max(-baseWeight, additionalWeightAtSixNeighbor);
            additionalWeightAtSevenNeighbor = Mathf.Max(-baseWeight, additionalWeightAtSevenNeighbor);
            additionalWeightAtEightNeighbor = Mathf.Max(-baseWeight, additionalWeightAtEightNeighbor);
        }
#endif
    }
}