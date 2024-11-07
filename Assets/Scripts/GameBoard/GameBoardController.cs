using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Класс, отвечающий за создание игрового поля и элементов на нем
/// </summary>
public class GameBoardController : MonoBehaviour
{
    public float CellSize => _tilePrefab.transform.localScale.x;

    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private float _tweenDuration = 0.4f;

    private Item[] _itemPrefabs;
    private Item[,] _items;
    private MatchFinder _matchFinder;

    public void Initialize(Item[] itemsPrefabs, MatchFinder matchFinder)
    {
        _itemPrefabs = itemsPrefabs;
        _matchFinder = matchFinder;
    }

    public Item[,] CreateGameBoard()
    {
        _items = new Item[_width, _height];

        // Создаем игровое поле - сетку из префабов клеточек с элементами на ней
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                var tile = Instantiate(_tilePrefab, transform);
                var tilePosition = new Vector2(x, y);
                tile.transform.position = tilePosition;
            }
        }

        FillGameBoard();
        return _items;
    }

    /// <summary>
    /// Метод заполняет игровое поле элементами
    /// </summary>
    private void FillGameBoard()
    {
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                // Создаем элемент в клетке
                SpawnItem(_items, new Vector2(x, y));
            }
        }
    }

    private void SpawnItem(Item[,] items, Vector2 position)
    {
        // Выбираем случайный элемент
        var index = Random.Range(0, _itemPrefabs.Length);
        var item = _itemPrefabs[index];

        // Пока есть совпадения типов элементов - генерируем новый индекс
        // Для того, чтобы на сгенерированной сетке не было элементов по 3 в ряд
        while (_matchFinder.HasMatchesWithPreviousItems(items, position, item))
        {
            index = Random.Range(0, _itemPrefabs.Length);
            item = _itemPrefabs[index];
        }

        item = Instantiate(_itemPrefabs[index], transform);
        item.transform.position = position;

        // Добавляем созданный элемент в массив
        items[(int)position.x, (int)position.y] = item;

        item.Show(_tweenDuration);
    }

    
}