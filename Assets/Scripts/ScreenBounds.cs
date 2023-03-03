using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ScreenBounds : MonoBehaviour
{
    public UnityEvent<Collider2D> ExitTriggerFired;

    [SerializeField]
    private float _cornerOffset = 1;

    private Camera _mainCamera;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.localScale = Vector3.one;
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
    }

    private void Start()
    {
        transform.position = Vector3.zero;
        UpdateBoundsSize();
    }

    public void UpdateBoundsSize()
    {
        //orthographicSize = half the size of the height of the screen. That is why we * it by 2
        float ySize = _mainCamera.orthographicSize * 2;

        //width of the camera depends on the aspect ratio and the height
        Vector2 boxColliderSize = new(ySize * _mainCamera.aspect, ySize);
        _boxCollider.size = boxColliderSize;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTriggerFired?.Invoke(collision);
    }

    public bool AmIOutOfBounds(Vector3 worldPosition)
    {
        return
            Mathf.Abs(worldPosition.x) > Mathf.Abs(_boxCollider.bounds.min.x) ||
            Mathf.Abs(worldPosition.y) > Mathf.Abs(_boxCollider.bounds.min.y);
    }

    public Vector2 CalculateWrappedPosition(Vector2 worldPosition, float teleportOffset = 0.2f)
    {
        float boxWidth = _boxCollider.size.x;
        float boxHeight = _boxCollider.size.y;

        Vector2 resultPosition = new(worldPosition.x, worldPosition.y);
        if (worldPosition.x < (_boxCollider.bounds.min.x - teleportOffset))
        {
            resultPosition.x = worldPosition.x + boxWidth - teleportOffset;
        }
        else if (worldPosition.x > _boxCollider.bounds.max.x + teleportOffset)
        {
            resultPosition.x = worldPosition.x - boxWidth + teleportOffset;
        }

        if (worldPosition.y > _boxCollider.bounds.max.y + teleportOffset)
        {
            resultPosition.y = worldPosition.y - boxHeight + teleportOffset;
        }
        else if (worldPosition.y < _boxCollider.bounds.min.y - teleportOffset)
        {
            resultPosition.y = worldPosition.y + boxHeight - teleportOffset;
        }

        return resultPosition;
    }

}
