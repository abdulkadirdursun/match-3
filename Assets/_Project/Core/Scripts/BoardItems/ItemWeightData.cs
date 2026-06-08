using System;
using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "ItemWeightData", menuName = "Match 3/Core/Item Weight Data")]
    public class ItemWeightData : ScriptableObject
    {
        #region Structs

        [Serializable]
        public struct AdditionalWeightData
        {
            [SerializeField, Range(1, 8)] private int sameTypeNeighborCount;
            [SerializeField, Range(-10f, 10f)] private float additionalWeight;

            public int SameTypeNeighborCount
            {
                get => sameTypeNeighborCount;
                set => sameTypeNeighborCount = value;
            }
            public float AdditionalWeight
            {
                get => additionalWeight;
                set => additionalWeight = value;
            }
        }

        #endregion

        [SerializeField, Range(0.1f, 20f)] private float baseWeight = 1f;
        [SerializeField] private AdditionalWeightData[] additionalWeightData;

        public float BaseWeight => baseWeight;

        public float GetAdditionalWeight(int sameTypeNeighborCount)
        {
            AdditionalWeightData selectedWeightData = new();
            foreach (ref readonly var weightData in additionalWeightData.AsSpan())
            {
                if (weightData.SameTypeNeighborCount == sameTypeNeighborCount)
                {
                    return weightData.AdditionalWeight;
                }

                if (weightData.SameTypeNeighborCount > selectedWeightData.SameTypeNeighborCount
                    && weightData.SameTypeNeighborCount < sameTypeNeighborCount)
                {
                    selectedWeightData.SameTypeNeighborCount = weightData.SameTypeNeighborCount;
                    selectedWeightData.AdditionalWeight = weightData.AdditionalWeight;
                }
            }

            return selectedWeightData.AdditionalWeight;
        }
    }
}