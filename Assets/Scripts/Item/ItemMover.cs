using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class ItemMover : MonoBehaviour
{

    public Action<Item[,]> ItemsSwapped;
    [SerializeField] private float _swapDuration = 0.5f;

    private Item[,] _items;
    private GameBoardIndexProvider _gameBoardIndexProvider;

    private Vector2Int _firstItemPosition;
    private Vector2Int _secondItemPosition;
    private MatchFinder _matchFinder;
    

    public void Initialize(Item[,] items, GameBoardIndexProvider gameBoardIndexProvider, MatchFinder matchFinder)
    {
        _items = items;
        _gameBoardIndexProvider = gameBoardIndexProvider;
        _matchFinder = matchFinder;
    }

    private void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            _firstItemPosition = GetItemPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _secondItemPosition = GetItemPosition();
            
            if (AreItemsAdjacent(_firstItemPosition, _secondItemPosition))
            {
                SwapItems();
            }
        }
    }

    private Vector2Int GetItemPosition()
    {
        Vector2Int itemPosition = Vector2Int.zero;
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            itemPosition = _gameBoardIndexProvider.GetIndex(mousePosition);
        }

        return itemPosition;
    }

    /// <summary>
    /// Метод проверяет, что переданные элементы располагаются в соседних клетках
    /// (исключая диагонали)
    /// </summary>
    private bool AreItemsAdjacent(Vector2Int firstItemPosition, Vector2Int secondItemPosition)
    {
        var xDifference = Mathf.Abs(firstItemPosition.x - secondItemPosition.x);
        var yDifference = Mathf.Abs(firstItemPosition.y - secondItemPosition.y);

        return xDifference + yDifference == 1;
    }

    public void ReSwapItems()
    {
        SwapItems(true);
    }
    private void SwapItems(bool isReSwap = false)
    {
        var firstItem = _items[_firstItemPosition.x, _firstItemPosition.y];
        var secondItem = _items[_secondItemPosition.x, _secondItemPosition.y];

        firstItem.Move(_secondItemPosition, _swapDuration);
        secondItem.Move(_firstItemPosition, _swapDuration);

        _items[_firstItemPosition.x, _firstItemPosition.y] = secondItem;
        _items[_secondItemPosition.x, _secondItemPosition.y] = firstItem;
        
        var movingSequence = DOTween.Sequence();
        movingSequence.Join(firstItem.Move(_secondItemPosition, _swapDuration));
        movingSequence.Join(secondItem.Move(_firstItemPosition, _swapDuration));
        movingSequence.OnComplete(() =>
        {
            if (!isReSwap)
            {
                ItemsSwapped?.Invoke(_items);
            }
        });

        (_firstItemPosition, _secondItemPosition) = (_secondItemPosition, _firstItemPosition);
    }

    

}