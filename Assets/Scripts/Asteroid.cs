using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 500f;
    [SerializeField] private float _teleportOffset = 1f;
    [SerializeField] private float _damageFlickDelay = 0.1f;
    [SerializeField] private Asteroid _minorAsteroidPrefab;
    [SerializeField] private int _piecesAmount = 2;


    private Rigidbody2D _rigidBody;
    private SpriteRenderer _sprite;
    private ScreenBounds _screenBounds;
    private Vector2 _direction;
    private int _colorIndex;
    private Coroutine _flickCoroutine;
    private int _lives;

    private readonly Color[] _possibleColors = new Color[] { Color.green, Color.cyan, Color.magenta };

    public void Init(ScreenBounds screenBounds, Vector2 direction, int lives)
    {
        _screenBounds = screenBounds;
        _direction = direction;
        _lives = lives;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();

        _colorIndex = 1; Random.Range(0, _possibleColors.Length);
        _sprite.color = _possibleColors[_colorIndex];
    }

    private void Update()
    {
        var tempPosition =_rigidBody.position + _speed * Time.deltaTime * _direction.normalized;

        //check if need to teleport asteroid in screen
        if (_screenBounds.AmIOutOfBounds(tempPosition))
        {
            var newPosition = _screenBounds.CalculateWrappedPosition(tempPosition);
            tempPosition = newPosition;
        }

        _rigidBody.MovePosition(tempPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            CollidedWithProjectile(collision);
        }
    }

    private void CollidedWithProjectile(Collider2D collision)
    {
        var projectile = collision.gameObject.GetComponent<Projectile>();

        if (projectile.GetColor() == _possibleColors[_colorIndex])
        {
            if (_flickCoroutine != null)
            {
                StopCoroutine(_flickCoroutine);
            }
            _flickCoroutine = StartCoroutine(FlickAsteroid());

            _lives--;
            if (_lives <= 0)
            {
                DestroyAsteroid();
            }
        }
        Destroy(collision.gameObject);
    }

    private void DestroyAsteroid()
    {
        if (_minorAsteroidPrefab)
        {
            SpawnMinorAsteroids();
        }
        Destroy(gameObject);
    }

    private void SpawnMinorAsteroids()
    {
        for (int i = 0; i < _piecesAmount; i++)
        {
            float t = i / (float)_piecesAmount;
            float angleRad = t * 2 * Mathf.PI;
            Vector2 direction = new(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            var asteroid = Instantiate(_minorAsteroidPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            asteroid.Init(_screenBounds, direction, 3);
        }
    }

    private IEnumerator FlickAsteroid()
    {
        _sprite.color = Color.red;
        yield return new WaitForSeconds(_damageFlickDelay);
        _sprite.color = _possibleColors[_colorIndex];
    }
}
