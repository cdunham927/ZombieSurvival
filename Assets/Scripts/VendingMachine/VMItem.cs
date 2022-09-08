using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMItem : MonoBehaviour
{
    public string vItemName;
    public string vItemDescription;
    public Sprite vItemIcon;

    public float price;

    public virtual void Effect() { }
}
