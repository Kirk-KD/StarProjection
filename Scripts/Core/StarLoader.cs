using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class StarLoader
{
    public static IEnumerator LoadFromCSV(string fileName, float maxMagnitude, Action<List<Star>> onComplete)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log($"Loading CSV from {path}");

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading file: {uwr.error}");
                onComplete?.Invoke(null);
                yield break;
            }

            string[] lines = uwr.downloadHandler.text.Split('\n');
            List<Star> stars = ParseStarsFromCSV(lines, maxMagnitude);
            onComplete?.Invoke(stars);
        } else
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"File not found: {path}");
                onComplete?.Invoke(null);
                yield break;
            }

            string[] lines = File.ReadAllLines(path);
            List<Star> stars = ParseStarsFromCSV(lines, maxMagnitude);
            onComplete?.Invoke(stars);
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
