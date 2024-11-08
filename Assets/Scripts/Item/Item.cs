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

    public Tween Move(Vector2Int position,float tweenDuration)
    {
        var tween = transform.DOMove(new Vector3(position.x,position.y), tweenDuration);
        return tween;
    }

    public Tween Kill(float tweenDuration)
    {
        var tween = transform.DOScale(0, tweenDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // В конце анимации уничтожаем элемент
                Destroy(gameObject);
            });

        return tween;
    }
}