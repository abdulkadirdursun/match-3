using System.Collections.Generic;
using DG.Tweening;
using Match3.ObjectPooling;
using UnityEngine;

namespace Match3.Core
{
    public class BoardItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private BoardConfig boardConfig;
        [SerializeField] private BoardAnimationConfig boardAnimationConfig;
        [SerializeField] private BoardItemData[] boardItemDataArray;
        [SerializeField] private MatchScanner matchScanner;
        [Header("Pool")]
        [SerializeField] private BoardItem boardItemPrefab;
        [SerializeField] private int startItemPoolSize = 16;
        [SerializeField] private int maxItemPoolSize = 144;

        private ObjectPool<BoardItem> _boardItemPool;

        public void SpawnBoardItems()
        {
            _boardItemPool ??= new ObjectPool<BoardItem>(boardItemPrefab, transform, startItemPoolSize, maxItemPoolSize, OnCreate, onRelease: OnRelease);
            var boardSize = gameplayData.BoardSize;
            var excludedItems = new HashSet<BoardItemData>();
            var iteration = 10;

            do
            {
                _boardItemPool.ReleaseAll();
                for (int y = 0; y < boardSize.y; y++)
                {
                    for (int x = 0; x < boardSize.x; x++)
                    {
                        excludedItems.Clear();
                        var coord = new Vector2Int(x, y);
                        if (!gameBoardData.TryGetBoardCell(coord, out var cell)) continue;
                        if (x >= 2
                            && gameBoardData.TryGetBoardCell(coord + Vector2Int.left, out var leftNeighborCell) && leftNeighborCell.HasBoardItem
                            && gameBoardData.TryGetBoardCell(coord + (2 * Vector2Int.left), out var farLeftCell) && farLeftCell.HasBoardItem
                            && leftNeighborCell.BoardItem.BoardItemData == farLeftCell.BoardItem.BoardItemData)
                        {
                            excludedItems.Add(leftNeighborCell.BoardItem.BoardItemData);
                        }

                        if (y >= 2
                            && gameBoardData.TryGetBoardCell(coord + Vector2Int.down, out var bottomNeighborCell) && bottomNeighborCell.HasBoardItem
                            && gameBoardData.TryGetBoardCell(coord + Vector2Int.down, out var farBottomCell) && farBottomCell.HasBoardItem
                            && bottomNeighborCell.BoardItem.BoardItemData == farBottomCell.BoardItem.BoardItemData)
                        {
                            excludedItems.Add(bottomNeighborCell.BoardItem.BoardItemData);
                        }

                        var newBoardItem = _boardItemPool.Get();
                        newBoardItem.Initialize(GetBoardItemData(excludedItems));
                        newBoardItem.PlaceAt(cell.WorldPos);
                        cell.SetItem(newBoardItem);
                    }
                }

                iteration--;
            } while (!matchScanner.HasAnyPossibleMatch() && iteration > 0);
        }

        public List<Tween> RefillEmptyCells()
        {
            var tweens = new List<Tween>();
            var boardSize = gameplayData.BoardSize;
            var spawnHeight = ((boardSize.y * 0.5f) + 1) * boardConfig.CellSize;

            for (int x = 0; x < boardSize.x; x++)
            {
                var emptyCells = 0;
                for (int y = 0; y < boardSize.y; y++)
                {
                    if (!gameBoardData.TryGetBoardCell(new Vector2Int(x, y), out var cell)
                        || cell.HasBoardItem) continue;

                    var item = _boardItemPool.Get();
                    item.Initialize(GetBoardItemData());
                    var spawnPos = cell.WorldPos;
                    spawnPos.y = spawnHeight;
                    item.PlaceAt(spawnPos);
                    cell.SetItem(item);

                    var moveDelay = emptyCells * boardAnimationConfig.FallDelay;

                    var tween = item.FallToPos(cell.WorldPos, moveDelay);
                    tweens.Add(tween);
                    emptyCells++;
                }
            }

            return tweens;
        }

        private BoardItemData GetBoardItemData(HashSet<BoardItemData> excludedItems = null)
        {
            var randomNumber = Random.Range(0, boardItemDataArray.Length);
            var selectedItem = boardItemDataArray[randomNumber];
            if (excludedItems is { Count: > 0 })
            {
                while (excludedItems.Contains(selectedItem))
                {
                    randomNumber = (randomNumber + 1) % boardItemDataArray.Length;
                    selectedItem = boardItemDataArray[randomNumber];
                }
            }

            return selectedItem;
        }

        #region Pool Methods

        private void OnCreate(BoardItem boardItem, ObjectPool<BoardItem> pool)
        {
            boardItem.ObjectHid += pool.ReleaseRequest;
        }

        private void OnRelease(BoardItem boardItem)
        {
            boardItem.Reset();
        }

        #endregion
    }
}