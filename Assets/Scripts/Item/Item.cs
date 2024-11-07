using DG.Tweening;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за игровой элемент
/// </summary>
public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemType Type { get; private set; }
    public bool IsMatched { get; set; }

    public void Show(float tweenDuration)
    {
        // Анимация увеличения размера элемента
        var itemScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(itemScale, tweenDuration);
    }

    public void Move(Vector2Int position,float tweenDuration)
    {
        transform.DOMove(new Vector3(position.x,position.y), tweenDuration);
    }

    public void Kill()
    {
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
    }
}