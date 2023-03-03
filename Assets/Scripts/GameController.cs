using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelsConfig _levelsConfig;
    [SerializeField] private AsteroidsTypesConfig _asteroidsTypesConfig;
    [SerializeField] private List<AsteroidSpawner> _spawners;

    private List<Asteroid> _instantiatedAsteroids;
    private int _currentLevel;
    private AsteroidsConfigs _currentConfig;

    private void Start()
    {
        _instantiatedAsteroids = new List<Asteroid>();
        _currentLevel = 1; //TODO: implement levels logic
        _currentConfig = _levelsConfig.GetLevelConfig(_currentLevel);

        StartCoroutine(StartSpawning(_levelsConfig.AsteroidsDelay));
    }

    private IEnumerator StartSpawning(float delay)
    {
        int maxSmall = _currentConfig.SmallAsteroidsAmount;
        int maxMedium = _currentConfig.MediumAsteroidsAmount;
        int maxBig = _currentConfig.BigAsteroidsAmount;

        while (maxSmall > 0)
        {
            yield return new WaitForSeconds(delay);
            SpawnAsteroid(AsteroidType.SmallAsteroid);
            maxSmall--;
        }

        while (maxMedium > 0)
        {
            yield return new WaitForSeconds(delay);
            SpawnAsteroid(AsteroidType.MediumAsteroid);
            maxMedium--;
        }

        while (maxBig > 0)
        {
            yield return new WaitForSeconds(delay);
            SpawnAsteroid(AsteroidType.BigAsteroid);
            maxBig--;
        }
    }

    private void SpawnAsteroid(AsteroidType type)
    {
        var spawner = GetRandomSpawner();
        var asteroidConfig = _asteroidsTypesConfig.GetAsteroid(type);
        var spawnedAsteroid = spawner.SpawnAsteroid(asteroidConfig.Prefab, asteroidConfig.PiecesAmount);
        _instantiatedAsteroids.Add(spawnedAsteroid);
    }

    private AsteroidSpawner GetRandomSpawner()
    {
        int randomIndex = Random.Range(0, _spawners.Count);
        return _spawners[randomIndex];
    }
}
