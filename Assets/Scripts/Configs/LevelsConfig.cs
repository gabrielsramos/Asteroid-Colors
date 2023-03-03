using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "ScriptableObjects/LevelsConfig", order = 1)]
public class LevelsConfig : ScriptableObject
{
    public float AsteroidsDelay;
    public List<AsteroidsConfigs> LevelsAsteroids;

    public AsteroidsConfigs GetLevelConfig(int level)
    {
        int levelsAmount = LevelsAsteroids.Count;

        if (level >= LevelsAsteroids.Count)
        {
            return LevelsAsteroids[levelsAmount - 1];
        }

        return LevelsAsteroids[level - 1];
    }
}