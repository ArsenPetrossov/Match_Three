using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectionController : MonoBehaviour
{
    [SerializeField] private LayerMask _selectableLayer;

    public Transform SelectedItem { get; private set; }
    public Transform MovableItem { get; private set; }

    private Vector2 _mouseStartPoint;
    private Vector2 _mouseEndPoint;
    private Transform _selectedItem;
    private Transform _targetItemToMove;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPoint = Input.mousePosition;

            SelectItem();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseEndPoint = Input.mousePosition;

            SelectItemInDirection(GetMovementDirection(_mouseStartPoint, _mouseEndPoint));
            SwapItem();
        }
    }

    public void SwapItem()
    {
        _selectedItem.GetComponent<Item>().MoveTo(_targetItemToMove.position,0.5f);
        _targetItemToMove.GetComponent<Item>().MoveTo(_selectedItem.position,0.5f);
    }
    public void SelectItem()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, _selectableLayer);

        if (hit.collider != null)
        {
            _selectedItem = hit.collider.transform;
            Debug.Log("Выбран объект " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("Ничего не выбрано");
        }

        SelectedItem = _selectedItem;
    }

    private Vector2 GetMovementDirection(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 direction = (endPoint - startPoint).normalized;
        Vector2 moveDirection;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                Debug.Log("Перетаскивание вправо");
                moveDirection = Vector2.right;
            }
            else
            {
                Debug.Log("Перетаскивание влево");
                moveDirection = Vector2.left;
            }
        }

        else
        {
            if (direction.y > 0)
            {
                Debug.Log("Перетаскивание вверх");
                moveDirection = Vector2.up;
            }
            else
            {
                Debug.Log("Перетаскивание вниз");
                moveDirection = Vector2.down;
            }
        }

        return moveDirection;
    }

    private void SelectItemInDirection(Vector2 direction)
    {
        Debug.DrawRay(_selectedItem.position, direction * 1f, Color.red, 1f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(_selectedItem.position, direction, 1f, _selectableLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.transform != _selectedItem)
            {
                Debug.Log("Выбран объект для перемещения: " + hit.collider.gameObject.name);
                _targetItemToMove = hit.collider.transform;
            }
        }

        MovableItem = _targetItemToMove;
    }
}