using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarProjectionManager : MonoBehaviour
{
    private static readonly string csvPath = Application.dataPath + "/StarProjection/Data/stars.csv";

    public List<Star> Stars { get; private set; }

    [Tooltip("The Magnitude of a star starts from zero and grows larger as the visibility grows dimmer.\nUsually stars with Magnitudes lower than 6 is visible to the naked eye. Increase this value to include more stars.")]
    [Header("Magnitude Cutoff")]
    [Range(0f, 15f)]
    [SerializeField]
    public float maxMagnitude = 6;

    [Tooltip("The resulting Vector3 locations are amplified by this factor. When the factor is 1, all stars are on the unit sphere. Set it to one if you WANT the stars to be on a unit sphere to be further processed.")]
    [Header("Distance Factor")]
    [Range(1f, 200f)]
    [SerializeField]
    private float distanceFactor = 30;

    private void Awake()
    {
        Stars = StarLoader.LoadFromCSV(csvPath, maxMagnitude);
        Debug.Log($"{Stars.Count} stars loaded from {csvPath} (Magnitude <= {maxMagnitude})");

        // Currently hard coded longitude
        StarLoader.ProjectStars(Stars, -122.4194f, DateTime.Now, distanceFactor);
    }
}
