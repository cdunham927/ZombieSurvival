using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopProximityController : MonoBehaviour
{
    PlayerController player;
    WeaponController weapon;

    //public VMItem[] itemList;

    //public VMUI vendingMachineItemParent;
    public GameObject shopCanvas;

    EventSystem ev;
    public Button exitButton;

    public bool inRange;

    public Button firstSelected;

    public GameObject sBParent;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        weapon = FindObjectOfType<WeaponController>();

        ev = FindObjectOfType<EventSystem>();

        //Set up button functionality
        exitButton.onClick.AddListener(delegate { ExitStore(); });
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && inRange)
        {
            OpenVendingMachine();
        }
    }

    public void OpenVendingMachine()
    {
        //Set exit button at bottom of shop
        exitButton.transform.SetParent(null);
        exitButton.transform.SetParent(sBParent.transform);
        //Set first selected button in shop(helps for controller support)
        firstSelected = sBParent.transform.GetChild(0).GetComponentInChildren<Button>();

        //Player can't move or shoot whilst in a menu
        player.InMenu();
        weapon.InMenu();
        //Open store UI
        shopCanvas.SetActive(true);
        //UI stuff(for controller support)
        ev.SetSelectedGameObject(null);
        ev.SetSelectedGameObject(firstSelected.gameObject);
    }

    public void ExitStore()
    {
        //UI stuff(for controller support)
        ev.SetSelectedGameObject(null);
        //Close store UI
        shopCanvas.SetActive(false);
        //Player can't move or shoot whilst in a menu
        player.ExitMenu();
        weapon.ExitMenu();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
