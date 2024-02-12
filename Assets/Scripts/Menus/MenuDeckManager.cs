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
    public Button CreateDeckButton; // Bouton pour créer un nouveau deck

    public DeckManager deckManager; // Référence au DeckManager

    public Transform deckListContainer; // Conteneur pour lister les decks
    public GameObject deckListItemPrefab; // Prefab pour un élément de liste de deck

    void SwitchMenu(GameObject menuToActivate)
    {
        Debug.Log("Switching to menu: " + menuToActivate);

        // Désactive tous les menus principaux
        MainDeckMenu.SetActive(false);
        preconMenu.SetActive(false);
        MyDeckMenu.SetActive(false);

        // Active le menu spécifié
        menuToActivate.SetActive(true);
    }
    
    void Start()
    {
        // Initialisation du menu
        preconMenu.SetActive(false);
        MyDeckMenu.SetActive(false);
        buttons.SetActive(true);
        MyDeckButton.gameObject.SetActive(true);
        PreconButton.gameObject.SetActive(true);
        BackFromPrecon.gameObject.SetActive(true);
        BackFromMyDeck.gameObject.SetActive(true);
        CreateDeckButton.gameObject.SetActive(true); // Assurez-vous que ce bouton est correctement configuré dans l'éditeur Unity

        // Ajout des écouteurs aux boutons
        PreconButton.onClick.AddListener(() => SwitchMenu(preconMenu));
        MyDeckButton.onClick.AddListener(() => ShowMyDecks());
        BackFromPrecon.onClick.AddListener(() => SwitchMenu(MainDeckMenu));
        BackFromMyDeck.onClick.AddListener(() => SwitchMenu(MainDeckMenu));
        CreateDeckButton.onClick.AddListener(CreateNewDeck);
    }

    void ShowMyDecks()
    {
        // Affiche le menu MyDeck et liste tous les decks existants
        SwitchMenu(MyDeckMenu);

        // Supprime les anciens éléments de la liste (pour actualiser)
        foreach (Transform child in deckListContainer)
        {
            Destroy(child.gameObject);
        }

        // Pour chaque deck dans DeckManager, crée un nouvel élément dans l'interface utilisateur
        foreach (var deck in deckManager.userDeck)
        {
            GameObject listItem = Instantiate(deckListItemPrefab, deckListContainer);
            listItem.GetComponent<Text>().text = deck.Name; // Assurez-vous que votre prefab a un composant Text pour afficher le nom
            // Ajoutez ici plus de logique pour afficher les détails du deck ou pour interagir avec lui
        }
    }

    void CreateNewDeck()
    {
        // Logique pour créer un nouveau deck
        Debug.Log("Création d'un nouveau deck...");

        // Ici, vous pourriez ouvrir une nouvelle interface permettant à l'utilisateur de choisir des cartes pour le nouveau deck
        // et ensuite l'ajouter à deckManager.userDeck
    }

    // Update est laissé vide si non utilisé
    void Update()
    {
        
    }
}