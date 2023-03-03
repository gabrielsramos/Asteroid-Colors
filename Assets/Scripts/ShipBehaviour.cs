using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipBehaviour : MonoBehaviour
{
    [SerializeField] private float _shootingDelay = 0.3f;
    [SerializeField] private float _shipRotationSpeed = 100.0f;
    [SerializeField] private float _shipPositionSpeed = 5.0f;

    [SerializeField] private ScreenBounds _screenBounds;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _positionToShootFrom;

    private ShipInputAction _actionInput;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _actionInput = new ShipInputAction();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(KeepShooting());
    }

    private void OnEnable()
    {
        _actionInput.Ship.Enable();
    }

    private void OnDisable()
    {
        _actionInput.Ship.Disable();
    }

    private void Update()
    {
        MoveShip();
    }

    private void MoveShip()
    {
        
        Vector2 direction = _actionInput.Ship.Move.ReadValue<Vector2>();
        Vector2 pointingAt = transform.up;
        float force = direction.magnitude;

        if (force == 0)
        {
            return;
        }

        float angleBetween = Vector2.SignedAngle(pointingAt, direction);

        //Rotating torwards direction
        if (Mathf.Abs(angleBetween) > 3f)
        {
            int factor = angleBetween > 0 ? 1 : -1;
            var rotation = factor * _shipRotationSpeed * force * Time.deltaTime;
            _rigidBody.MoveRotation(rotation + _rigidBody.rotation);
        }

        //move towards direction
        Vector2 velocity = direction * _shipPositionSpeed;

        var tempPosition = _rigidBody.position + velocity * Time.deltaTime;

        //check if need to teleport ship in screen
        if (_screenBounds.AmIOutOfBounds(tempPosition))
        {
            Vector2 newPosition = _screenBounds.CalculateWrappedPosition(tempPosition);
            tempPosition = newPosition;
        }

        _rigidBody.MovePosition(tempPosition);  
    }

    private void Shoot()
    {
        var rotation = Quaternion.Euler(new Vector3(0, 0, _rigidBody.rotation));
        var bullet = Instantiate(_projectilePrefab, _positionToShootFrom.position, rotation, transform);
        bullet.Init(Color.cyan, _screenBounds);
    }

    private IEnumerator KeepShooting()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(_shootingDelay);
        }
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
