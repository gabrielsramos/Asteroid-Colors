using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidBody;
    private ScreenBounds _screenBounds;
    private SpriteRenderer _sprite;
    private Color _color;

    public void Init(Color color, ScreenBounds screenBounds)
    {
        _color = color;
        _screenBounds = screenBounds;

        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = color;
    }

    public Color GetColor()
    {
        return _color;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidBody.velocity = _speed * Time.deltaTime * transform.up;

        var globalPosition = transform.TransformPoint(transform.position);

        if (_screenBounds.AmIOutOfBounds(globalPosition))
        {
            Destroy(gameObject);
        }
    }

}
