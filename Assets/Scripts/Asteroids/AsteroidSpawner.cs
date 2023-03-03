using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private List<Vector2> _spawnDirections;
    [SerializeField] private ScreenBounds _screenBounds;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector2 center = transform.position;

        foreach (Vector2 direction in _spawnDirections)
        {
            Gizmos.DrawLine(center, center + direction);
        }
    }

    public Asteroid SpawnAsteroid(Asteroid asteroidPrefab, int piecesAmount, int livesAmount)
    {
        int directionIndex = Random.Range(0, _spawnDirections.Count);
        var asteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        Color randomColor = ColorsRandomizer.GetRandomColor();
        asteroid.Init(_screenBounds, _spawnDirections[directionIndex], randomColor, piecesAmount, livesAmount);
        return asteroid;
    }
}
