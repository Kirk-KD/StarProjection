using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarLoader
{
    public static IEnumerator LoadFromCSV(string fileName, float maxMagnitude, Action<List<Star>> onComplete)
    {
        string path = GetStreamingAssetsPath(fileName);
        if (File.Exists(path))
        {
            Debug.Log($"Loading stars from {path}");

            using StreamReader sr = new(path);

            List<string> lines = new();
            while (!sr.EndOfStream) lines.Add(sr.ReadLine());
            onComplete?.Invoke(ParseStarsFromCSV(lines.ToArray(), maxMagnitude));
        } else
        {
            Debug.LogError($"File not found: {path}");
            onComplete?.Invoke(null);
        }

        yield return null;
    }

    private static string GetStreamingAssetsPath(string fileName)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Special case for iOS
            return Path.Combine(Application.dataPath, "Raw", fileName);
        }
        else
        {
            // Other NORMAL platforms made by intelligent people
            return Path.Combine(Application.streamingAssetsPath, fileName);
        }
    }

    private static List<Star> ParseStarsFromCSV(string[] lines, float maxMagnitude)
    {
        List<Star> stars = new();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (!ValidateStarData(values)) continue;

            Star star = new()
            {
                Catalog = "HP", // Catalog
                ID = values[1], // ID
                RAdeg = float.Parse(values[8]), // Right Ascension
                DEdeg = float.Parse(values[9]), // Declination
                Magnitude = float.Parse(values[5]) // Visual Magnitude
            };

            if (star.Magnitude <= maxMagnitude) stars.Add(star);
            //if (star.HasReadableName) Debug.Log($"Named star: {star.Name}");
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
