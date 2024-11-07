using UnityEngine;

/// <summary>
/// Класс, отвечающий за поиск совпадений элементов
/// </summary>
public class MatchFinder
{
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
            if(items[position.x - 1, position.y].Type == itemToCheck.Type && 
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
    
    public bool HasMatchesAfterMove(Item[,] items, Vector2Int position)
    {
        ItemType itemType = items[position.x, position.y].Type;

        // Проверяем совпадения по горизонтали
        int horizontalMatches = 1;
        horizontalMatches += CountMatchesInDirection(items, position, itemType, Vector2Int.left);
        horizontalMatches += CountMatchesInDirection(items, position, itemType, Vector2Int.right);

        // Проверяем совпадения по вертикали
        int verticalMatches = 1;
        verticalMatches += CountMatchesInDirection(items, position, itemType, Vector2Int.up);
        verticalMatches += CountMatchesInDirection(items, position, itemType, Vector2Int.down);

        return horizontalMatches >= 3 || verticalMatches >= 3;
    }

    private int CountMatchesInDirection(Item[,] items, Vector2Int startPosition, ItemType itemType, Vector2Int direction)
    {
        int matchCount = 0;
        Vector2Int checkPosition = startPosition + direction;

        while (IsWithinBounds(items, checkPosition) && items[checkPosition.x, checkPosition.y].Type == itemType)
        {
            matchCount++;
            checkPosition += direction;
        }

        return matchCount;
    }

    private bool IsWithinBounds(Item[,] items, Vector2Int position)
    {
        return position.x >= 0 && position.x < items.GetLength(0) &&
               position.y >= 0 && position.y < items.GetLength(1);
    }

    public void HandleMatches(Item[,] items)
    {
        Debug.Log("Обработка совпадений: удаление и заполнение новых элементов.");
    }
}