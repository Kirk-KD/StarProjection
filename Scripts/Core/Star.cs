using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star
{
    public static readonly Dictionary<string, string> StarDictionary = new()
    {
        { "13847", "Acamar" }, { "7588", "Achernar" }, { "60718", "Acrux" }, { "33579", "Adhara" },
        { "95947", "Albireo" }, { "65477", "Alcor" }, { "17702", "Alcyone" },
        { "21421", "Aldebaran" }, { "105199", "Alderamin" }, { "1067", "Algenib" }, { "50583", "Algieba" },
        { "14576", "Algol" }, { "31681", "Alhena" }, { "62956", "Alioth" }, { "67301", "Alkaid" },
        { "9640", "Almaak" }, { "109268", "Alnair" }, { "25428", "Alnath" }, { "26311", "Alnilam" },
        { "26727", "Alnitak" }, { "46390", "Alphard" }, { "76267", "Alphekka" }, { "677", "Alpheratz" },
        { "98036", "Alshain" }, { "97649", "Altair" }, { "2081", "Ankaa" }, { "80763", "Antares" },
        { "69673", "Arcturus" }, { "25985", "Arneb" }, { "112247", "Babcock's star" }, { "87937", "Barnard's star" },
        { "25336", "Bellatrix" }, { "27989", "Betelgeuse" }, { "96295", "Campbell's star" }, { "30438", "Canopus" },
        { "24608", "Capella" }, { "746", "Caph" }, { "36850", "Castor" }, { "63125", "Cor Caroli" },
        { "98298", "Cyg X-1" }, { "102098", "Deneb" }, { "57632", "Denebola" }, { "3419", "Diphda" },
        { "54061", "Dubhe" }, { "107315", "Enif" }, { "87833", "Etamin" }, { "113368", "Fomalhaut" },
        { "57939", "Groombridge 1830" }, { "68702", "Hadar" }, { "9884", "Hamal" }, { "72105", "Izar" },
        { "24186", "Kapteyn's star" }, { "90185", "Kaus Australis" }, { "72607", "Kocab" }, { "110893", "Kruger 60" },
        { "36208", "Luyten's star" }, { "113963", "Markab" }, { "59774", "Megrez" }, { "14135", "Menkar" },
        { "53910", "Merak" }, { "25930", "Mintaka" }, { "10826", "Mira" }, { "5447", "Mirach" },
        { "15863", "Mirphak" }, { "65378", "Mizar" }, { "25606", "Nihal" }, { "92855", "Nunki" },
        { "58001", "Phad" }, { "17851", "Pleione" }, { "11767", "Polaris" }, { "37826", "Pollux" },
        { "37279", "Procyon" }, { "70890", "Proxima" }, { "84345", "Rasalgethi" }, { "86032", "Rasalhague" },
        { "30089", "Red Rectangle" }, { "49669", "Regulus" }, { "24436", "Rigel" }, { "71683", "Rigil Kent" },
        { "109074", "Sadalmelik" }, { "27366", "Saiph" }, { "113881", "Scheat" }, { "85927", "Shaula" },
        { "3179", "Shedir" }, { "92420", "Sheliak" }, { "32349", "Sirius" }, { "65474", "Spica" },
        { "97278", "Tarazed" }, { "68756", "Thuban" }, { "77070", "Unukalhai" }, { "3829", "Van Maanen 2" },
        { "91262", "Vega" }, { "63608", "Vindemiatrix" }, { "18543", "Zaurak" }, { "60936", "3C273" }
    };

    public string Catalog { get; set; } // Name of the database (always "HP")
    public string ID { get; set; } // ID of the star, basically a string of numbers
    public float RAdeg { get; set; } // Right ascension degree
    public float DEdeg { get; set; } // Declination degree
    public float Magnitude { get; set; } // Brightness, higher is dimmer, <= 6 is roughly the cutoff for naked eye visibility
    public float? BV { get; set; } // bV color value
    public Vector3 Position { get; private set; }

    public string Name {
        get {
            return StarDictionary.GetValueOrDefault(ID, Catalog + ID);
        }
    }
    public bool HasReadableName
    {
        get
        {
            return StarDictionary.ContainsKey(ID);
        }
    }

    public Color Color
    {
        get
        {
            if (BV == null) return new Color(1f, 1f, 1f, 1f);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            float bV = Mathf.Clamp((float)BV, -0.4f, 2.0f);

            float r, g, b;

            if (bV < 0.0f)
            {
                // Blue stars
                r = 0.61f + 0.11f * bV;
                g = 0.70f + 0.07f * bV;
                b = 1.00f;
            }
            else if (bV < 0.4f)
            {
                // White-blue stars
                r = 0.83f + 0.17f * bV;
                g = 0.87f + 0.11f * bV;
                b = 1.00f;
            }
            else if (bV < 1.5f)
            {
                // Yellow stars
                r = 1.00f;
                g = 0.98f - 0.16f * (bV - 0.4f);
                b = 0.87f - 0.31f * (bV - 0.4f);
            }
            else
            {
                // Red stars
                r = 1.00f;
                g = 0.82f - 0.5f * (bV - 1.5f);
                b = 0.57f - 0.44f * (bV - 1.5f);
            }

            return new Color(r, g, b, 1.0f);
#else
            float bV = Mathf.Clamp((float)BV, -0.4f, 2.0f);

            float r, g, b;

            if (bV < 0.0f)
            {
                // Blue stars (changed because apple sucks)
                r = 0.61f + 0.11f * bV - 0.2f;
                g = 0.70f + 0.07f * bV - 0.2f;
                b = 1.00f;
            }
            else if (bV < 0.4f)
            {
                // White-blue stars
                r = 0.83f + 0.17f * bV;
                g = 0.87f + 0.11f * bV;
                b = 1.00f;
            }
            else if (bV < 1.5f)
            {
                // Yellow stars (changed because apple sucks)
                r = 1.00f;
                g = 0.98f - 0.16f * (bV - 0.4f) - 0.2f;
                b = 0.87f - 0.31f * (bV - 0.4f) - 0.2f;
            }
            else
            {
                // Red stars
                r = 1.00f;
                g = 0.82f - 0.5f * (bV - 1.5f);
                b = 0.57f - 0.44f * (bV - 1.5f);
            }

            return new Color(r, g, b, 1.0f);
#endif
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
