using System;
using System.Collections;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Action OnLivesLost;
    [HideInInspector]
    public Action OnShipDestroyed;

    [SerializeField] private int _flickerooRate;
    [SerializeField] private float _invulDelay;
    [SerializeField] private int _lives; //TODO move this to config
    [SerializeField] private float _shootingDelay = 0.3f;
    [SerializeField] private float _shipRotationSpeed = 100.0f;
    [SerializeField] private float _shipPositionSpeed = 5.0f;

    [SerializeField] private ScreenBounds _screenBounds;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _positionToShootFrom;
    [SerializeField] private SpriteRenderer _leftPropulsorSprite;
    [SerializeField] private SpriteRenderer _rightPropulsorSprite;

    private ShipInputAction _actionInput;
    private Rigidbody2D _rigidBody;
    private Color _shootingColor;
    private bool _isInvulnerable;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _actionInput = new ShipInputAction();
        _rigidBody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
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

    private void FixedUpdate()
    {
        MoveShip();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") && !_isInvulnerable)
        {
            _lives--;
            if (_lives <= 0)
            {
                OnShipDestroyed?.Invoke();
                DestroyShip();
            }
            else
            {
                OnLivesLost?.Invoke();
                StartCoroutine(StartFlickeroo());
            }
        }
    }

    private void DestroyShip()
    {
        Destroy(gameObject);
    }

    private IEnumerator StartFlickeroo()
    {
        _isInvulnerable = true;
        var transparentColor = new Color(1, 1, 1, 0);
        float remainingTime = _invulDelay;
        float interval = _invulDelay / (float)_flickerooRate;

        while (remainingTime > 0.0f)
        {
            _sprite.color = transparentColor;
            _leftPropulsorSprite.color = transparentColor;
            _rightPropulsorSprite.color = transparentColor;
            remainingTime -= interval;
            yield return new WaitForSeconds(interval);

            _sprite.color = Color.white;
            _leftPropulsorSprite.color = Color.white;
            _rightPropulsorSprite.color = Color.white;
            remainingTime -= interval;
            yield return new WaitForSeconds(interval);
        }
        _isInvulnerable = false;
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
        //TODO: remove this magic number
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
        bullet.Init(_shootingColor, _screenBounds);
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

    public void SetShootingColor(Color color)
    {
        _shootingColor = color;
    }
}
