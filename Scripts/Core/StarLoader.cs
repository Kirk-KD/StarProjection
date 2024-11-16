using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarLoader
{
    public static List<Star> LoadFromCSV(string path, float maxMagnitude)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"No such file: {path}");

        List<Star> stars = new();
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (!ValidateStarData(values)) continue;

            Star star = new Star
            {
                Catalog = "HP", // Catalog
                ID = values[1], // ID
                RAdeg = float.Parse(values[8]), // Right Ascension
                DEdeg = float.Parse(values[9]), // Declination
                Magnitude = float.Parse(values[5]) // Visual Magnitude
            };

            if (star.Magnitude <= maxMagnitude) stars.Add(star);
        }

        return stars;
    }

    public static void ProjectStars(List<Star> stars, float longitude, DateTime dateTime, float distanceFactor)
    {
        foreach (var star in stars) star.ToVector3(longitude, dateTime, distanceFactor);
    }

    private static bool ValidateStarData(string[] values)
    {
        return !values[8].Equals("") && !values[9].Equals("") && !values[5].Equals("");
    }
}
