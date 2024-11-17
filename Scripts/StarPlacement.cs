using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(StarProjectionManager))]
public class StarPlacement : MonoBehaviour
{
    public GameObject starPrefab;

    private StarProjectionManager projectionManager;

    private void Awake()
    {
        projectionManager = GetComponent<StarProjectionManager>();
    }

    public void PlaceStars(List<Star> stars)
    {
        foreach (Star star in stars)
        {
            GameObject starObj = Instantiate(starPrefab, star.Position, Quaternion.identity);
            float factor = GetMagnitudeFactor(star);

            ModifyStarObject(starObj, star, factor);
        }
    }

    private float GetMagnitudeFactor(Star star)
    {
        return Mathf.Clamp01((projectionManager.maxMagnitude + 1f - star.Magnitude) / (projectionManager.maxMagnitude + 1f));
    }

    private void ModifyStarObject(GameObject starObj, Star star, float factor)
    {
        MeshRenderer renderer = starObj.GetComponent<MeshRenderer>();

        MaterialPropertyBlock block = new();
        Color color = new(star.Color.r, star.Color.g, star.Color.b, factor);
        block.SetColor("_EmissionColor", color);
        renderer.SetPropertyBlock(block);

        starObj.transform.localScale *= factor;

        if (star.HasReadableName)
        {
            SphereCollider collider = starObj.AddComponent<SphereCollider>();
            collider.radius = 10;

            NamedStar namedStar = starObj.AddComponent<NamedStar>();
            namedStar.SetStar(star);
        }
    }
}
