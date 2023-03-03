using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorsRandomizer
{
    public static Color GetRandomColor()
    {
        Color[] possibleColors = new Color[] { Color.green, Color.cyan, Color.magenta };
        int colorIndex = Random.Range(0, possibleColors.Length);
        return possibleColors[colorIndex];
    }
}
