using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDeckManager : MonoBehaviour
{
    public DeckManager deckManager; // Assignez cette référence dans l'inspecteur Unity
    public InputField newDeckNameInputField; // Assignez ce champ dans l'éditeur Unity
    public GameObject MainDeckMenu;
    public GameObject MyDeckMenu;
    public GameObject preconMenu;
    public GameObject buttons;
    public Button PreconButton;
    public Button MyDeckButton;
    public Button BackFromPrecon;
    public Button BackFromMyDeck;
    public Button CreateDeckButton; // Bouton pour créer un nouveau deck
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
    [SerializeField]
    private CardDatabase cardDatabase; // Assignez cette référence dans l'inspecteur Unity

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
        SwitchMenu(MyDeckMenu);

        foreach (Transform child in deckListContainer)
        {
            Destroy(child.gameObject);
        }

        // Assurez-vous que cette méthode existe et retourne une List<DeckInfo>
        foreach (var deck in deckManager.GetDecks())
        {
            GameObject listItem = Instantiate(deckListItemPrefab, deckListContainer);
            var textComponent = listItem.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = deck.deckName; // Utilisez `deckName` pour le nom du deck
            }
            else
            {
                Debug.LogError("Text component not found in deck list item prefab.");
            }
            // Vous pouvez étendre ici pour inclure d'autres informations sur le deck
        }
    }

    void CreateNewDeck()
    {
        string newDeckName = newDeckNameInputField.text;
        if (!string.IsNullOrWhiteSpace(newDeckName))
        {
            deckManager.AddDeck(newDeckName);
            newDeckNameInputField.text = ""; // Réinitialiser le champ de texte
            ShowMyDecks(); // Actualiser l'affichage des decks
        }
        else
        {
            Debug.LogError("Deck name cannot be empty.");
        }
    }


    // Update est laissé vide si non utilisé
    void Update()
    {
        
    }
}