using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [HideInInspector]
    public Action<Asteroid> OnMoreAsteroidsCreated;
    [HideInInspector]
    public Action<Asteroid> OnAsteroidDestroyed;

    [SerializeField] private float _speed = 500f;
    [SerializeField] private float _teleportOffset = 1f;
    [SerializeField] private float _damageFlickDelay = 0.1f;
    [SerializeField] private Asteroid _minorAsteroidPrefab;

    [Header("Audio clips")]
    [SerializeField] private AudioClip _asteroidHitedClip;
    [SerializeField] private AudioClip _asteroidDestroyedClip;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _sprite;
    private ScreenBounds _screenBounds;
    private Vector2 _direction;
    private Coroutine _flickCoroutine;
    private int _lives;
    private Color _color;
    private int _pieces;
    private AudioSource _audioSource;

    public void Init(ScreenBounds screenBounds, Vector2 direction, Color color, int pieces, int lives)
    {
        _screenBounds = screenBounds;
        _direction = direction;
        _lives = lives;
        _color = color;
        _pieces = pieces;

        _sprite.color = _color;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        var tempPosition =_rigidBody.position + _speed * Time.deltaTime * _direction.normalized;

        //check if need to teleport asteroid in screen
        if (_screenBounds.AmIOutOfBounds(tempPosition))
        {
            var newPosition = _screenBounds.CalculateWrappedPosition(tempPosition, _teleportOffset);
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
        _audioSource.PlayOneShot(_asteroidHitedClip);

        if (projectile.GetColor() == _color)
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
        OnAsteroidDestroyed?.Invoke(this);
        _audioSource.PlayOneShot(_asteroidDestroyedClip);
        Destroy(gameObject);
    }

    private void SpawnMinorAsteroids()
    {
        for (int i = 0; i < _pieces; i++)
        {
            float angleRad = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            Vector2 direction = new(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            var asteroid = Instantiate(_minorAsteroidPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            asteroid.Init(_screenBounds, direction, _color, _pieces, _lives);
            OnMoreAsteroidsCreated?.Invoke(asteroid);
        }
    }

    private IEnumerator FlickAsteroid()
    {
        _sprite.color = Color.red;
        yield return new WaitForSeconds(_damageFlickDelay);
        _sprite.color = _color;
    }
}
