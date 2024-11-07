using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за поиск совпадений элементов
/// </summary>
public class MatchFinder
{
    private List<Item> _matchedItems = new List<Item>();

    /// <summary>
    /// Метод проверяет, есть ли совпадения по типам элементов с двумя предыдущими элементами
    /// Используется при создании элементов игрового поля
    /// </summary>
    public bool HasMatchesWithPreviousItems(Item[,] items, Vector2 positionToCheck, Item itemToCheck)
    {
        var position = Vector2Int.RoundToInt(positionToCheck);

        // Проверка по горизонтали
        if (position.x > 1)
        {
            if (items[position.x - 1, position.y].Type == itemToCheck.Type &&
                items[position.x - 2, position.y].Type == itemToCheck.Type)
            {
                return true;
            }
        }

        // Проверка по вертикали
        if (positionToCheck.y > 1)
        {
            if (items[position.x, position.y - 1].Type == itemToCheck.Type &&
                items[position.x, position.y - 2].Type == itemToCheck.Type)
            {
                return true;
            }
        }

        return false;
    }

    public List<Vector2Int> FindMatches(Item[,] items, Vector2Int position)
    {
        List<Vector2Int> matchedPositions = new List<Vector2Int>();
        ItemType itemType = items[position.x, position.y].Type;

        // Список для временного хранения совпадений
        List<Vector2Int> horizontalMatches = new List<Vector2Int>();
        List<Vector2Int> verticalMatches = new List<Vector2Int>();

        // Проверяем горизонтальные совпадения
        horizontalMatches.Add(position);
        horizontalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.left));
        horizontalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.right));

        if (horizontalMatches.Count >= 3)
        {
            matchedPositions.AddRange(horizontalMatches);
        }

        // Проверяем вертикальные совпадения
        verticalMatches.Add(position);
        verticalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.up));
        verticalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.down));

        if (verticalMatches.Count >= 3)
        {
            matchedPositions.AddRange(verticalMatches);
        }

        // Убираем дублирующиеся позиции
        matchedPositions = matchedPositions.Distinct().ToList();

        return matchedPositions;
    }

    private List<Vector2Int> FindMatchesInDirection(Item[,] items, Vector2Int startPosition, ItemType itemType, Vector2Int direction)
    {
        List<Vector2Int> matches = new List<Vector2Int>();
        Vector2Int checkPosition = startPosition + direction;

        while (IsWithinBounds(items, checkPosition))
        {
            Item currentItem = items[checkPosition.x, checkPosition.y];

            if (currentItem != null && currentItem.Type == itemType)
            {
                matches.Add(checkPosition);
                checkPosition += direction;
            }
            else
            {
                break;
            }
        }

        return matches;
    }

    private bool IsWithinBounds(Item[,] items, Vector2Int position)
    {
        return position.x >= 0 && position.x < items.GetLength(0) &&
               position.y >= 0 && position.y < items.GetLength(1);
    }

    public void HandleMatches(Item[,] items, List<Vector2Int> matchedPositions)
    {
        foreach (var position in matchedPositions)
        {
            Item matchedItem = items[position.x, position.y];

            matchedItem.Kill();

            items[position.x, position.y] = null;
        }
    }
}