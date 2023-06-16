using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableItemList : ScriptableObject
{
    public List<Buyable> allItems = new List<Buyable>();
}
