using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyable
{
    float Cost
    {
        get;
        set;
    }

    void Buy();
}
