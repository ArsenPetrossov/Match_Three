using DG.Tweening;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за игровой элемент
/// </summary>
public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemType Type { get; private set; }

    public void Show(float tweenDuration)
    {
        // Анимация увеличения размера элемента
        var itemScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(itemScale, tweenDuration);
    }
}