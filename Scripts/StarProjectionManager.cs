using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StarPlacement))]
[RequireComponent(typeof(LongitudeRuler))]
public class StarProjectionManager : MonoBehaviour
{
    private static readonly string csvFileName = "stars.csv";

    public List<Star> Stars { get; private set; }

    private StarPlacement starPlacement;
    private LongitudeRuler longitudeRuler;

    [Tooltip("The Magnitude of a star starts from zero and grows larger as the visibility grows dimmer.\nUsually stars with Magnitudes lower than 6 is visible to the naked eye. Increase this value to include more stars.")]
    [Header("Magnitude Cutoff")]
    [Range(0f, 15f)]
    public float maxMagnitude = 6;

    [Tooltip("The resulting Vector3 locations are amplified by this factor. When the factor is 1, all stars are on the unit sphere. Set it to one if you WANT the stars to be on a unit sphere to be further processed.")]
    [Header("Distance Factor")]
    [Range(1f, 200f)]
    [SerializeField]
    private float distanceFactor = 30;


    private void Awake()
    {
        starPlacement = GetComponent<StarPlacement>();
        longitudeRuler = GetComponent<LongitudeRuler>();
    }


    private void Start()
    {
        StartCoroutine(InitializeStars());
    }

    private IEnumerator InitializeStars()
    {
        Coroutine getLongitude = StartCoroutine(longitudeRuler.GetLongitude());
        Coroutine loadStars = StartCoroutine(StarLoader.LoadFromCSV(csvFileName, maxMagnitude, OnFinishLoadingStars));
        yield return getLongitude;
        yield return loadStars;
    }

    private void OnFinishLoadingStars(List<Star> stars)
    {
        if (stars == null)
        {
            Debug.LogError("Stars did not load!");
            return;
        }

        Debug.Log($"Loaded {stars.Count} stars.");

        Stars = stars;
        StarLoader.ProjectStars(Stars, LongitudeRuler.Longitude, DateTime.Now, distanceFactor);

        Debug.Log($"Projected at longitude {LongitudeRuler.Longitude} @ {DateTime.Now}");
        
        starPlacement.PlaceStars(Stars);
    }
}
