using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyable : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    //Buy cost
    public float cost;
    public Sprite sprite;
    public int spriteWidth;
    public int spriteHeight;
    public virtual void Buy() { }
}
