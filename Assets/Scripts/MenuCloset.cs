// Allows the user to search for cards using MTG api (https://api.magicthegathering.io/v1/)

using System.Collections;
using System.Text; // For StringBuilder
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class GetCards : MonoBehaviour
{
    public UnityEngine.UI.InputField searchBar;
    public Button searchButton;
    public Text resultListText;
    public Image cardImage;
    public GameObject SearchMenu; // Menu principal de recherche
    public GameObject MainMenu; // Menu principal du closet
    public GameObject DeckMenu; // Menu principal du deck
    public Button openMainButton;
    public Button openDeckButton;
    public Button openSearchButton;
    public Button BackFromSearch;
    public Button BackFromCloset;
    public Button BackFromDeck;
    public Button BackFromMain;
    public GameObject Question;


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
        if (openMainButton != null)
        {
            openMainButton.onClick.AddListener(() => ToggleMainMenu()); // Pour fermer
        }
        if (BackFromMain != null)
        {
            BackFromMain.onClick.AddListener(() => ToggleMainMenu()); // Pour ouvrir
        }

    openMainButton.gameObject.SetActive(true); // Bouton ouvert au démarrage
    BackFromCloset.gameObject.SetActive(true); // Bouton fermé au démarrage

    openSearchButton.gameObject.SetActive(false); // Bouton fermé au démarrage
    openDeckButton.gameObject.SetActive(false); // Bouton fermé au démarrage
    Question.SetActive(false); // Bouton fermé au démarrage

    SearchMenu.SetActive(false); // Menu fermé au démarrage
    DeckMenu.SetActive(false); // Menu fermé au démarrage
    MainMenu.SetActive(false); // Menu fermé au démarrage
    }

    void ToggleMainMenu()
    {
        Debug.Log("ToggleMainMenu");
        openMainButton.gameObject.SetActive(!openMainButton.gameObject.activeSelf); // Inverse l'état actuel du bouton
        BackFromCloset.gameObject.SetActive(!BackFromCloset.gameObject.activeSelf); // Inverse l'état actuel du bouton
        MainMenu.SetActive(!MainMenu.activeSelf); // Inverse l'état actuel du menu de recherche
        Question.SetActive(!Question.activeSelf); // Inverse l'état actuel du menu de recherche
        openDeckButton.gameObject.SetActive(!openDeckButton.gameObject.activeSelf); // Inverse l'état actuel du bouton
        openSearchButton.gameObject.SetActive(!openSearchButton.gameObject.activeSelf); // Inverse l'état actuel du bouton
    }
    void ToggleSearchMenu()
    {
        Debug.Log("ToggleSearchMenu");
        openDeckButton.gameObject.SetActive(!openDeckButton.gameObject.activeSelf); // Inverse l'état actuel du bouton
        BackFromMain.gameObject.SetActive(!BackFromMain.gameObject.activeSelf); // Inverse l'état actuel du bouton
        Question.SetActive(!Question.activeSelf); // Inverse l'état actuel du menu de recherche
        openSearchButton.gameObject.SetActive(!openSearchButton.gameObject.activeSelf); // Inverse l'état actuel du bouton
        BackFromSearch.gameObject.SetActive(!BackFromCloset.gameObject.activeSelf); // Inverse l'état actuel du bouton
        SearchMenu.SetActive(!SearchMenu.activeSelf); // Inverse l'état actuel du menu de recherche
    }

    void ToggleDeckMenu()
    {
        Debug.Log("ToggleDeckMenu");
        openDeckButton.gameObject.SetActive(!openDeckButton.gameObject.activeSelf); // Inverse l'état actuel du bouton
        BackFromDeck.gameObject.SetActive(!BackFromDeck.gameObject.activeSelf); // Inverse l'état actuel du bouton
        DeckMenu.SetActive(!SearchMenu.activeSelf); // Inverse l'état actuel du menu de recherche
    }
}