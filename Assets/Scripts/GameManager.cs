using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс - менеджер, инициализирующий и связывающий остальные сущности
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private ItemsSetProvider _itemsSetProvider;
    [SerializeField] private GameBoardController _gameBoardController;
    [SerializeField] private ItemMover _itemsMover;

    private MatchFinder _matchFinder;
    private void Awake()
    {
        _matchFinder = new MatchFinder();
        var gameBoardIndexProvider = new GameBoardIndexProvider(_gameBoardController);
        var itemsSet = _itemsSetProvider.GetItemsSet();

        _gameBoardController.Initialize(itemsSet, _matchFinder);

        var items = _gameBoardController.CreateGameBoard();
        _itemsMover.Initialize(items, gameBoardIndexProvider, _matchFinder);

        _itemsMover.ItemsSwapped += OnItemSwapped;
        _gameBoardController.ItemFellsDown += OnItemsDown;
    }

    private void OnItemsDown(Item[,] items)
    {
        if (_matchFinder.HasMatches(items))
        {
            _gameBoardController.KillMatched(_matchFinder.Matches);
            
        }
    }

    private void OnItemSwapped(Item[,] items)
    {
        // Если на игровом поле совпадения есть - разрушаем совпавшие элементы
        if (_matchFinder.HasMatches(items))
        {
            _gameBoardController.KillMatched(_matchFinder.Matches);
            
        }
        else
        {
            // Если совпадений нет - возвращает элементы обратно на свои места
            _itemsMover.ReSwapItems();
        }
    }

    private void OnDestroy()
    {
        _itemsMover.ItemsSwapped -= OnItemSwapped;
        _gameBoardController.ItemFellsDown -= OnItemsDown;
    }
}