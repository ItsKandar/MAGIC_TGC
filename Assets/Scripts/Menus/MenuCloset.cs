// Allows the user to search for cards using MTG api (https://api.magicthegathering.io/v1/)

using System.Collections;
using System.Text; // For StringBuilder
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class MenuCloset : MonoBehaviour
{
    public GameObject SearchMenu; // Menu principal de recherche
    public GameObject Menus; // Menu principal du closet
    public GameObject DeckMenu; // Menu principal du deck
    public Button openMainButton;
    public Button openDeckButton;
    public Button openSearchButton;
    public Button BackFromSearch;
    public Button BackFromCloset;
    public Button BackFromDeck;
    public Button BackFromMain;
    public GameObject MainButtons;


    void Start()
    {
        if (BackFromSearch != null)
        {
            BackFromSearch.onClick.AddListener(() => ToggleSearchMenu()); // Pour fermer
        }
        if (openSearchButton != null)
        {
            openSearchButton.onClick.AddListener(() => ToggleSearchMenu()); // Pour ouvrir
        }
        if (BackFromDeck != null)
        {
            BackFromDeck.onClick.AddListener(() => ToggleDeckMenu()); // Pour fermer
        }
        if (openDeckButton != null)
        {
            openDeckButton.onClick.AddListener(() => ToggleDeckMenu()); // Pour ouvrir
        }
        if (openMainButton != null)
        {
            openMainButton.onClick.AddListener(() => ToggleMenus()); // Pour fermer
        }
        if (BackFromMain != null)
        {
            BackFromMain.onClick.AddListener(() => ToggleMenus()); // Pour ouvrir
        }

    openMainButton.gameObject.SetActive(true); // Bouton ouvert au démarrage
    BackFromCloset.gameObject.SetActive(true); // Bouton fermé au démarrage
    BackFromMain.gameObject.SetActive(true); // Bouton fermé au démarrage
    BackFromSearch.gameObject.SetActive(false); // Bouton fermé au démarrage
    BackFromDeck.gameObject.SetActive(false); // Bouton fermé au démarrage
    MainButtons.SetActive(false); // Bouton fermé au démarrage

    SearchMenu.SetActive(false); // Menu fermé au démarrage
    DeckMenu.SetActive(false); // Menu fermé au démarrage
    Menus.SetActive(false); // Menu fermé au démarrage
    }

    void ToggleMenus()
    {
        Debug.Log("ToggleMainMenu");
        BackFromCloset.gameObject.SetActive(!BackFromCloset.gameObject.activeSelf); // Inverse l'état actuel du bouton
        Menus.SetActive(!Menus.activeSelf); // Inverse l'état actuel du menu de recherche
        MainButtons.SetActive(!MainButtons.activeSelf); // Inverse l'état actuel du menu de recherche
    }
    void ToggleSearchMenu()
    {
        Debug.Log("ToggleSearchMenu");
        MainButtons.SetActive(!MainButtons.activeSelf); // Inverse l'état actuel du menu de recherche
        BackFromSearch.gameObject.SetActive(!BackFromCloset.gameObject.activeSelf); // Inverse l'état actuel du bouton
        SearchMenu.SetActive(!SearchMenu.activeSelf); // Inverse l'état actuel du menu de recherche
    }

    void ToggleDeckMenu()
    {
        Debug.Log("ToggleDeckMenu");
        BackFromDeck.gameObject.SetActive(!BackFromDeck.gameObject.activeSelf); // Inverse l'état actuel du bouton
        MainButtons.SetActive(!MainButtons.activeSelf); // Inverse l'état actuel du menu de recherche
        DeckMenu.SetActive(!DeckMenu.activeSelf); // Inverse l'état actuel du menu de recherche
    }
}