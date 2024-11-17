using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedStar : MonoBehaviour
{
    public Star Data { get; private set; }

    public void SetStar(Star star)
    {
        Data = star;
    }
}
