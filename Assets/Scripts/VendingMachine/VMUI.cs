using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VMUI : MonoBehaviour
{
    public Button but;
    public Image im;
    public Text nameText;
    //public Text descriptionText;
    public Text buyText;

    public float cost;

    PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void SetUp(VMItem it)
    {
        //Set up text
        nameText.text = it.vItemName + " - " + it.vItemDescription;
        buyText.text = "Buy - $" + it.price.ToString();
        //descriptionText.text = it.vItemDescription;
        //Set up icon
        im.sprite = it.vItemIcon;
        //Set up button functionality
        but.onClick.AddListener(delegate { it.Effect(); });
        //Set up cost
        cost = it.price;
    }

    private void Update()
    {
        but.interactable = (player.money > cost);
    }
}
