using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VendingMachineController : MonoBehaviour
{
    PlayerController player;
    WeaponController weapon;

    public VMItem[] itemList;

    public VMUI vendingMachineItemParent;
    public GameObject vendingMachineCanvas;

    EventSystem ev;
    public Button exitButton;

    public bool inRange;

    public Button firstSelected;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        weapon = FindObjectOfType<WeaponController>();

        ev = FindObjectOfType<EventSystem>();
        SetupVendingMachine();

        //Set up button functionality
        exitButton.onClick.AddListener(delegate { ExitStore(); });
    }

    void SetupVendingMachine()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            //Instantiate object
            VMUI v = Instantiate(vendingMachineItemParent);
            //UI stuff(for controller support)
            if (i == 0)
            {
                firstSelected = v.but;
            }
            //Set parent
            v.transform.SetParent(vendingMachineCanvas.transform);
            //Setup object
            v.SetUp(itemList[i]);
        }

        //Set exit button to be at the bottom of the store list
        exitButton.transform.SetParent(null);
        exitButton.transform.SetParent(vendingMachineCanvas.transform);
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
        //Player can't move or shoot whilst in a menu
        player.InMenu();
        weapon.InMenu();
        //Open store UI
        vendingMachineCanvas.SetActive(true);
        //UI stuff(for controller support)
        ev.SetSelectedGameObject(null);
        ev.SetSelectedGameObject(firstSelected.gameObject);
    }

    public void ExitStore()
    {
        //UI stuff(for controller support)
        ev.SetSelectedGameObject(null);
        //Close store UI
        vendingMachineCanvas.SetActive(false);
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
