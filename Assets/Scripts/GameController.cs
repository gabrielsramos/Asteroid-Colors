using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelsConfig _levelsConfig;
    [SerializeField] private AsteroidsTypesConfig _asteroidsTypesConfig;
    [SerializeField] private List<AsteroidSpawner> _spawners;
}
