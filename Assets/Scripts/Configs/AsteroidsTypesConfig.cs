using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidsTypesConfig", menuName = "ScriptableObjects/AsteroidsTypesConfig", order = 1)]
public class AsteroidsTypesConfig : ScriptableObject
{
    public List<AsteroidPrefabByType> AsteroidsTypes;

    public Asteroid GetAsteroid(AsteroidType type)
    {
        foreach (AsteroidPrefabByType asteroidPrefabByType in AsteroidsTypes)
        {
            if (asteroidPrefabByType.Type == type)
            {
                return asteroidPrefabByType.Prefab;
            }
        }

        Debug.LogError($"The type {type} does not have a prefab configured!");
        return null;
    }
}

[Serializable]
public class AsteroidPrefabByType
{
    public AsteroidType Type;
    public Asteroid Prefab;
}