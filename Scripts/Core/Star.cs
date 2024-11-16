using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star
{
    public string Catalog { get; set; } // Name of the database (always "HP")
    public string ID { get; set; } // ID of the star, basically a string of numbers
    public float RAdeg { get; set; } // Right ascension degree
    public float DEdeg { get; set; } // Declination degree
    public float Magnitude { get; set; } // Brightness, higher is dimmer, <= 6 is roughly the cutoff for naked eye visibility
    public Vector3 Position { get; private set; }

    public string Name {
        get {
            return Catalog + ID;
        }
    }

    public void ToVector3(float longitude, DateTime dateTime, float distanceFactor)
    {
        float RA = Mathf.Deg2Rad * RAdeg;
        float DEC = Mathf.Deg2Rad * DEdeg;

        float x = Mathf.Cos(DEC) * Mathf.Cos(RA);
        float y = Mathf.Cos(DEC) * Mathf.Sin(RA);
        float z = Mathf.Sin(DEC);

        double jd = JulianDate(dateTime);
        double LST = SiderealTime(jd, longitude);

        float rotationAngle = Mathf.Deg2Rad * (float)LST * 15f;

        float rotatedX = x * Mathf.Cos(rotationAngle) - y * Mathf.Sin(rotationAngle);
        float rotatedY = x * Mathf.Sin(rotationAngle) + y * Mathf.Cos(rotationAngle);
        float rotatedZ = z;

        Position = new Vector3(rotatedX, rotatedY, rotatedZ) * distanceFactor;
    }

    private double JulianDate(DateTime dateTime)
    {
        DateTime epoch = new(2000, 1, 1, 12, 0, 0);
        double delta = (dateTime - epoch).TotalDays;
        return 2451545.0 + delta;
    }

    private double SiderealTime(double jd, float longitude)  // got the weird numbers from https://github.com/jamesstill/stcalc
    {
        double T = (jd - 2451545.0) / 36525.0;  // Julian Century
        double GMST = 280.46061837 + 360.98564736629 * (jd - 2451545.0) + T * T * 0.000387933 - T * T * T / 38710000.0;
        GMST %= 360;  // Normalize
        double LST = GMST + longitude;  // Local Sidereal Time
        return LST % 360;  // Normalize
    }
}
