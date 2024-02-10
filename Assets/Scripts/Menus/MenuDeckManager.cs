using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDeckManager : MonoBehaviour
{
    public GameObject MainDeckMenu;
    public GameObject MyDeckMenu;
    public GameObject preconMenu;
    public GameObject buttons;
    public Button PreconButton;
    public Button MyDeckButton;
    public Button BackFromPrecon;
    public Button BackFromMyDeck;

    void SwitchMenu(GameObject menuToActivate)
    {
        Debug.Log("Switching to menu: " + menuToActivate);

        // Main Deck Menu
        MainDeckMenu.SetActive(false);

        // Precon Menu
        preconMenu.SetActive(false);
        if (menuToActivate == preconMenu)
        {
            preconMenu.SetActive(true);
        }

        // My Deck Menu
        MyDeckMenu.SetActive(false);
        if (menuToActivate == MyDeckMenu)
        {
            MyDeckMenu.SetActive(true);
        }

        // Main Deck Menu
        MainDeckMenu.SetActive(false);
        if (menuToActivate == MainDeckMenu)
        {
            MainDeckMenu.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Disable the precon and my deck menus
        preconMenu.SetActive(false);
        MyDeckMenu.SetActive(false);

        // Enable the buttons
        buttons.SetActive(true);
        MyDeckButton.gameObject.SetActive(true);
        PreconButton.gameObject.SetActive(true);
        BackFromPrecon.gameObject.SetActive(true);

        // Add listeners to the buttons
        PreconButton.onClick.AddListener(() => SwitchMenu(preconMenu));
        MyDeckButton.onClick.AddListener(() => SwitchMenu(MyDeckMenu));
        BackFromPrecon.onClick.AddListener(() => SwitchMenu(MainDeckMenu));
        BackFromMyDeck.onClick.AddListener(() => SwitchMenu(MainDeckMenu));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}