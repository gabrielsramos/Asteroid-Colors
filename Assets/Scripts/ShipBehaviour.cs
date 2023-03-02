using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipBehaviour : MonoBehaviour
{
    private ShipInputAction _actionInput;

    private void Awake()
    {
        _actionInput = new ShipInputAction();
    }

    private void OnEnable()
    {
        _actionInput.Ship.Enable();
    }

    private void OnDisable()
    {
        _actionInput.Ship.Disable();
    }

    private void OnDrawGizmos()
    {
        Vector2 center = transform.position;

        if (_actionInput == null)
        {
            return;
        }

        Vector2 direction = _actionInput.Ship.Move.ReadValue<Vector2>();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(center, center + direction.normalized);
    }
}
