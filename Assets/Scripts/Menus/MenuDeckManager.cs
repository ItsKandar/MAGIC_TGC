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
    public Button MyDeckButton;
    public Button PreconButton;
    public Button BackFromPrecon;
    public Button BackFromMyDeck;

    void SwitchMenu(GameObject menuToActivate)
    {
        Debug.Log("Switching to menu: " + menuToActivate);

        // Désactive tous les menus principaux
        buttons.SetActive(false);
        MyDeckMenu.SetActive(false);
        preconMenu.SetActive(false);

        if (menuToActivate == MainDeckMenu)
        {
            buttons.SetActive(true);
        }
        // Active le menu spécifié
        menuToActivate.SetActive(true);
    }

    void Start()
    {
        // Initialisation du menu
        preconMenu.SetActive(false);
        MyDeckMenu.SetActive(false);
        buttons.SetActive(true);
        PreconButton.gameObject.SetActive(true);
        BackFromPrecon.gameObject.SetActive(true);
        BackFromMyDeck.gameObject.SetActive(true);

        // Ajout des écouteurs aux boutons
        PreconButton.onClick.AddListener(() => SwitchMenu(preconMenu));
        MyDeckButton.onClick.AddListener(() => SwitchMenu(MyDeckMenu));
        BackFromPrecon.onClick.AddListener(() => SwitchMenu(MainDeckMenu));
        BackFromMyDeck.onClick.AddListener(() => SwitchMenu(MainDeckMenu));
    }


    // Update est laissé vide si non utilisé
    void Update()
    {
        
    }
}