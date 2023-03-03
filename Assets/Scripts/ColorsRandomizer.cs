using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorsRandomizer
{
    public static Color GetRandomColor()
    {
        Color[] possibleColors = new Color[] { new(1.0f, 1.0f, 0.0f, 1.0f), Color.cyan, Color.green };
        int colorIndex = Random.Range(0, possibleColors.Length);
        return possibleColors[colorIndex];
    }
}
