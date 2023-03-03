using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game Configs")]
    [SerializeField] private LevelsConfig _levelsConfig;
    [SerializeField] private AsteroidsTypesConfig _asteroidsTypesConfig;

    [Header("Scene References")]
    [SerializeField] private List<AsteroidSpawner> _spawners;
    [SerializeField] private ShipBehaviour _ship;
    [SerializeField] private UIController _uiController;

    private List<Asteroid> _instantiatedAsteroids;
    private int _currentLevel;
    private AsteroidsConfigs _currentConfig;

    private void Start()
    {
        _instantiatedAsteroids = new List<Asteroid>();
        _currentLevel = 1; //TODO: implement levels logic
        _currentConfig = _levelsConfig.GetLevelConfig(_currentLevel);

        ShootingColorChanged(Color.cyan);
        StartCoroutine(StartSpawning(_levelsConfig.AsteroidsDelay));
    }

    private void OnEnable()
    {
        _uiController.OnColorSelected += ShootingColorChanged;
        _ship.OnLivesLost += LiveLost;
        _ship.OnShipDestroyed += ShipWasDestroyed;
    }

    private void OnDisable()
    {
        _uiController.OnColorSelected -= ShootingColorChanged;
        _ship.OnLivesLost += LiveLost;
        _ship.OnShipDestroyed += ShipWasDestroyed;
    }

    private void LiveLost()
    {
        _uiController.LoseLive();
    }

    private void ShipWasDestroyed()
    {
        _uiController.LoseLive();
        //TODO end phase with game over
    }

    private void ShootingColorChanged(Color color)
    {
        _ship.SetShootingColor(color);
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
        var spawnedAsteroid = spawner.SpawnAsteroid(asteroidConfig.Prefab, asteroidConfig.PiecesAmount, asteroidConfig.LivesAmount);
        _instantiatedAsteroids.Add(spawnedAsteroid);
    }

    private AsteroidSpawner GetRandomSpawner()
    {
        int randomIndex = Random.Range(0, _spawners.Count);
        return _spawners[randomIndex];
    }
}
