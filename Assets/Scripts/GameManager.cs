using UnityEngine;

/// <summary>
/// Класс - менеджер, инициализирующий и связывающий остальные сущности
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private ItemsSetProvider _itemsSetProvider;
    [SerializeField] private GameBoardController _gameBoardController;
    [SerializeField] private ItemMover _itemsMover;

    private void Awake()
    {
        var matchFinder = new MatchFinder();
        var gameBoardIndexProvider = new GameBoardIndexProvider(_gameBoardController);
        var itemsSet = _itemsSetProvider.GetItemsSet();

        _gameBoardController.Initialize(itemsSet, matchFinder);

        var items = _gameBoardController.CreateGameBoard();
        _itemsMover.Initialize(items, gameBoardIndexProvider, matchFinder);
    }
}