using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LongitudeRuler : MonoBehaviour
{
    public GameObject permissionPromptUI;  

    public static float Longitude { get; private set; }

    void Awake()
    {
        
    }

    public IEnumerator GetLongitude()
    {

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User did not approve the use of location");
            permissionPromptUI.SetActive(true); 
            RequestLocationAccess(); 
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
            Debug.Log("Location service initialization timed out");
            Longitude = 0;
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            Longitude = 0;
            yield break;
        }
        else
        {
            Longitude = Input.location.lastData.longitude;
            Debug.Log("Longitude: " + Longitude); 
        }
    }
    public float getLongitude()
    {
        return Longitude;
    }
    public void RequestLocationAccess()
    {
        if (!Input.location.isEnabledByUser)
        {
            Application.OpenURL("app-settings:"); 
        }
    }

    void OnDestroy()
    {
        Input.location.Stop();  
    }
}