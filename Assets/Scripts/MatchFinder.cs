using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder
{
    public List<Item> Matches => _matches;
    private List<Item> _matches = new();

    public bool HasMatchesWithPreviousItems(Item[,] items, Vector2 positionToCheck, Item itemToCheck)
    {
        var position = Vector2Int.RoundToInt(positionToCheck);

        if (position.x > 1)
        {
            if (items[position.x - 1, position.y].Type == itemToCheck.Type &&
                items[position.x - 2, position.y].Type == itemToCheck.Type)
            {
                return true;
            }
        }

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

        List<Vector2Int> horizontalMatches = new List<Vector2Int>();
        List<Vector2Int> verticalMatches = new List<Vector2Int>();

        horizontalMatches.Add(position);
        horizontalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.left));
        horizontalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.right));

        if (horizontalMatches.Count >= 3)
        {
            matchedPositions.AddRange(horizontalMatches);
            AddItemsToMatchesList(items, horizontalMatches);
        }

        verticalMatches.Add(position);
        verticalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.up));
        verticalMatches.AddRange(FindMatchesInDirection(items, position, itemType, Vector2Int.down));

        if (verticalMatches.Count >= 3)
        {
            matchedPositions.AddRange(verticalMatches);
            AddItemsToMatchesList(items, verticalMatches);
        }

        matchedPositions = matchedPositions.Distinct().ToList();


        return matchedPositions;
    }

    private List<Vector2Int> FindMatchesInDirection(Item[,] items, Vector2Int startPosition, ItemType itemType,
        Vector2Int direction)
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

    public bool IsWithinBounds(Item[,] items, Vector2Int position)
    {
        return position.x >= 0 && position.x < items.GetLength(0) &&
               position.y >= 0 && position.y < items.GetLength(1);
    }


    public bool HasMatches(Item[,] items)
    {
        _matches.Clear();

        for (int x = 0; x < items.GetLength(0); x++)
        {
            for (int y = 0; y < items.GetLength(1); y++)
            {
                if (items[x, y] != null)
                {
                    FindMatches(items, new Vector2Int(x, y));
                }
            }
        }

        return _matches.Count > 0;
    }

    private void AddItemsToMatchesList(Item[,] items, List<Vector2Int> positions)
    {
        foreach (var pos in positions)
        {
            Item item = items[pos.x, pos.y];
            if (item != null && !_matches.Contains(item))
            {
                _matches.Add(item);
            }
        }
    }
}