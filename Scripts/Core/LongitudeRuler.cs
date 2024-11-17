using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongitudeRuler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLocationService());
    }
    public float Longitude { get; private set; }
    private IEnumerator StartLocationService()
    {

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("La usere did not approve the use of location");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("Monsieur Neuvilette, le locating timed out");
            yield break;
        }
        else
        {
            Longitude = Input.location.lastData.longitude;
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Input.location.Stop();
    }
}
