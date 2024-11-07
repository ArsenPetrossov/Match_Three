using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemMover : MonoBehaviour
{
    [SerializeField] private float _swapDuration = 0.5f;

    private Item[,] _items;
    private GameBoardIndexProvider _gameBoardIndexProvider;

    private Vector2Int _firstItemPosition;
    private Vector2Int _secondItemPosition;
    private MatchFinder _matchFinder;
    private bool _isSwapping = false;

    public void Initialize(Item[,] items, GameBoardIndexProvider gameBoardIndexProvider, MatchFinder matchFinder)
    {
        _items = items;
        _gameBoardIndexProvider = gameBoardIndexProvider;
        _matchFinder = matchFinder;
    }

    private void Update()
    {
        if (_isSwapping)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _firstItemPosition = GetItemPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _secondItemPosition = GetItemPosition();
            
            if (AreItemsAdjacent(_firstItemPosition, _secondItemPosition))
            {
                StartCoroutine(SwapAndCheck());
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

    private void SwapItems()
    {
        var firstItem = _items[_firstItemPosition.x, _firstItemPosition.y];
        var secondItem = _items[_secondItemPosition.x, _secondItemPosition.y];

        firstItem.Move(_secondItemPosition, _swapDuration);
        secondItem.Move(_firstItemPosition, _swapDuration);

        _items[_firstItemPosition.x, _firstItemPosition.y] = secondItem;
        _items[_secondItemPosition.x, _secondItemPosition.y] = firstItem;

        (_firstItemPosition, _secondItemPosition) = (_secondItemPosition, _firstItemPosition);
    }

    private IEnumerator SwapAndCheck()
    {
        _isSwapping = true;

        SwapItems();
        Debug.Log("Перемещаем обекты");
        yield return new WaitForSeconds(_swapDuration);

        bool matchFoundFirst = _matchFinder.HasMatchesAfterMove(_items, _firstItemPosition);
        bool matchFoundSecond = _matchFinder.HasMatchesAfterMove(_items, _secondItemPosition);

        if (!matchFoundFirst && !matchFoundSecond)
        {
            SwapItems();

            yield return new WaitForSeconds(_swapDuration);
            Debug.Log("Совпадений не найдено, элементы возвращены обратно.");
        }
        else
        {
            Debug.Log("Совпадения найдены, обрабатываем их.");
            _matchFinder.HandleMatches(_items);
        }

        _isSwapping = false;
    }
}