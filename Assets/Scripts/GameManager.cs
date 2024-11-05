using UnityEngine;

/// <summary>
/// Класс - менеджер, инициализирующий и связывающий остальные сущности
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private ItemsSetProvider _itemsSetProvider;
    [SerializeField] 
    private GameBoardController _gameBoardController;

    private void Awake()
    {
        var matchFinder = new MatchFinder();
        var itemsSet = _itemsSetProvider.GetItemsSet();
        
        _gameBoardController.Initialize(itemsSet, matchFinder);
        _gameBoardController.CreateGameBoard();
    }
}