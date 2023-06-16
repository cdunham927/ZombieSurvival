using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemList : MonoBehaviour
{
    public GameObject shopParent;
    //public List<Items> weaponList;
    WeaponController weapons;
    PlayerController player;

    //New weapon list
    public ScriptableItemList iList;
    public GameObject shopListParent;
    public GameObject shopItemPrefab;

    ShopItem shop;

    //Do we want the shop to pick like 5 random guns to fill the shop each wave??
    public int numShopItems;
    public ScriptableItemList equipmentList;
    public List<ShopItem> curShopList = new List<ShopItem>();
    public int numEquipment;
    public float timeBetweenShopRotations;
    float remainingShopTime;

    //Temp list so we get no duplicate weapons
    //These probably don't need to be public
    public List<Buyable> dupeIList = new List<Buyable>();
    public List<Buyable> dupeEList = new List<Buyable>();

    public Text nextShopText;

    EventSystem ev;
    public Button exitButton;

    //We can probably set it up later so the shop is
    //sorted based on how expensive the items are
    //Weapons go at the top of the shop
    //While Equipment goes at the bottom

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        weapons = FindObjectOfType<WeaponController>();

        ev = FindObjectOfType<EventSystem>();

        SetupShopInitial();
    }

    void CopyList(List<Buyable> fromList, List<Buyable> toList)
    {
        toList.Clear();

        for (int i = 0; i < fromList.Count; i++)
        {
            toList.Add(fromList[i]);
        }
    }

    //Spawn item icon, set up description, and add functionality to button
    public void SetupShopInitial()
    {
        //Make new temporary lists for weapons and equipment
        //so we don't have any duplicates in the shop
        CopyList(iList.allItems, dupeIList);
        CopyList(equipmentList.allItems, dupeEList);

        //Put weapons in shop randomly
        for (int i = 0; i < numShopItems; i++)
        {
            Buyable newItem = dupeIList[Random.Range(0, dupeIList.Count)];
            SetupWeapon(newItem);
            dupeIList.Remove(newItem);
        }

        //Put equipment in the shop too
        for (int e = 0; e < numEquipment; e++)
        {
            Buyable newEquipment = dupeEList[Random.Range(0, dupeEList.Count)];
            SetupWeapon(newEquipment);
            dupeEList.Remove(newEquipment);
        }

        //Set timer
        remainingShopTime = timeBetweenShopRotations;

        //Set exit button to be at the bottom of the store list
        //Set up button functionality
        exitButton.onClick.AddListener(delegate { ExitStore(); });
        exitButton.transform.SetParent(null);
        exitButton.transform.SetParent(shopListParent.transform);
    }

    public void ExitStore()
    {
        //UI stuff(for controller support)
        ev.SetSelectedGameObject(null);
        //Close store UI
        shopParent.SetActive(false);
        //Player can't move or shoot whilst in a menu
        player.ExitMenu();
        weapons.ExitMenu();
    }

    public void PopulateShop()
    {
        //Old shop setup
        //for (int i = 0; i < iList.allItems.Count; i++)
        //{
        //    SetupWeapon(iList.allItems[i]);
        //}


        //Make new temporary lists for weapons and equipment
        //so we don't have any duplicates in the shop
        CopyList(iList.allItems, dupeIList);
        CopyList(equipmentList.allItems, dupeEList);

        //Need to replace the current shops weapons and equipment
        for (int i = 0; i < curShopList.Count; i++)
        {
            if (i < numShopItems)
            {
                Buyable newItem = dupeIList[Random.Range(0, dupeIList.Count)];
                ReplaceItem(curShopList[i], newItem);
                dupeIList.Remove(newItem);
            }
            else
            {
                //Put equipment in the shop too
                Buyable newEquipment = dupeEList[Random.Range(0, dupeEList.Count)];
                ReplaceItem(curShopList[i], newEquipment);
                dupeEList.Remove(newEquipment);
            }
        }

        //Set timer
        remainingShopTime = timeBetweenShopRotations;

        //Set exit button to be at the bottom of the store list
        exitButton.transform.SetParent(null);
        exitButton.transform.SetParent(shopListParent.transform);
    }

    void ReplaceItem(ShopItem sh, Buyable nI)
    {
        //Change sprite, set size
        sh.itemImage.sprite = nI.sprite;
        sh.layEl.preferredWidth = nI.spriteWidth;
        sh.layEl.preferredHeight = nI.spriteHeight;

        //Set up description and price text
        sh.itemName.text = nI.itemName;
        sh.itemDescription.text = nI.itemDescription;
        sh.itemCost.text = "Buy - $" + nI.cost.ToString();

        //Delete old functionality
        shop.buyButton.onClick.RemoveAllListeners();
        //Set up new button functionality
        sh.buyButton.onClick.AddListener(delegate { BuyItem(nI); });

        //Set its parent so it shows up in the correct spot
        //sh.transform.SetParent(shopListParent.transform);
    }

    //Put description of item, icon, and price
    void SetupWeapon(Buyable i)
    {
        shop = Instantiate(shopItemPrefab).GetComponent<ShopItem>();
        //Change sprite, set size
        shop.itemImage.sprite = i.sprite;
        shop.layEl.preferredWidth = i.spriteWidth;
        shop.layEl.preferredHeight = i.spriteHeight;

        //Set up description and price text
        shop.itemName.text = i.itemName;
        shop.itemDescription.text = i.itemDescription;
        shop.itemCost.text = "Buy - $" + i.cost.ToString();

        //Set up button functionality
        shop.buyButton.onClick.AddListener(delegate { BuyItem(i); } );

        //Set its parent so it shows up in the correct spot
        shop.transform.SetParent(shopListParent.transform);

        curShopList.Add(shop);
    }

    private void Update()
    {
        if (remainingShopTime > 0)
        {
            remainingShopTime -= Time.deltaTime;
        }

        if (remainingShopTime <= 0)
        {
            PopulateShop();
        }

        //Need this to look like an actual countdown
        //float roundedTime = Mathf.Round(remainingShopTime * 20f) / 20f;
        float roundedTime = Mathf.Round(remainingShopTime);
        nextShopText.text = ("Shop refreshes in. . . " + roundedTime + "s");
        nextShopText.color = (remainingShopTime < 10f) ? Color.red : Color.yellow;
    }

    public void BuyItem(Buyable item)
    {
        Items it = item.GetComponent<Items>();
        Equipment eq = item.GetComponent<Equipment>();

        //If we're buying a weapon, check inventory stuff and add it that way
        if (player.money >= item.cost && weapons.hasRoom()) {
            if (it != null)
            {
                player.money -= item.cost;
                weapons.AddItem(it);

                //Set up button functionality
                if (shop != null) shop.buyButton.onClick.RemoveAllListeners();
            }
            else
            {
                player.money -= item.cost;
                weapons.SwitchEquipment(eq);

                //Set up button functionality
                //if (shop != null) shop.buyButton.onClick.RemoveAllListeners();
            }
        }
    }
}