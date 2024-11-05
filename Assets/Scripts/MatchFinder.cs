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
}