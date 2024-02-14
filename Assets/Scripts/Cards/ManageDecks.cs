using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Text; // For StringBuilder
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class ManageDeck : MonoBehaviour
{
    private DeckManager database; // Référence à la base de données des decks
    public Button CreateDeckButton; // Bouton pour créer un nouveau deck
    public Dropdown DeckDropdown; // Liste déroulante pour les decks
    public Transform deckListContainer; // Conteneur pour les éléments de liste de decks
    public GameObject deckListItemPrefab; // Préfabriqué pour les éléments de liste de decks
    public InputField newDeckNameInputField; // Champ de texte pour le nom du nouveau deck
    public Button ListDeckButton; // Bouton pour lister les decks

    // Start is called before the first frame update
    void Start()
    {
        database = DeckManager.LoadDatabase();
        if (database == null)
        {
            Debug.LogError("Failed to load the card database.");
        }

        // Ajoute des listeneres
        CreateDeckButton.onClick.AddListener(CreateNewDeck);
        ListDeckButton.onClick.AddListener(ListDecks);
    }
    
    void ListDecks()
    {
        Debug.Log("Listing decks.");

        // Effacer la liste actuelle
        // foreach (Transform child in deckListContainer)
        // {
        //     child.gameObject.SetActive(false);
        //     if (child != null){
        //         Destroy(child.gameObject);
        //     }
        // }

        // Créer un élément de liste pour chaque deck
        foreach (var deck in database.decks)
        {
            GameObject newItem = Instantiate(deckListItemPrefab, deckListContainer);
            Text deckName = newItem.GetComponentInChildren<Text>();
            deckName.text = deck.deckName;
            Button deckButton = newItem.GetComponentInChildren<Button>();
            deckButton.onClick.AddListener(() => ShowDeck(deck.deckName));
        }

        // Mettre à jour la liste déroulante
        DeckDropdown.ClearOptions();
        DeckDropdown.AddOptions(database.decks.Select(deck => deck.deckName).ToList());

        // Afficher le premier deck
        if (database.decks.Count > 0)
        {
            ShowDeck(database.decks[0].deckName);
        }
    }

    void ShowDeck(string deckName)
    {
        
        Debug.Log("Showing deck " + deckName);
        var deck = database.decks.Find(deck => deck.deckName == deckName);
        if (deck != null)
        {
            Debug.Log("Deck found: " + deck.deckName);
            foreach (var card in deck.cards)
            {
                Debug.Log("Card in deck: " + card.Name);
            }
        }
        else
        {
            Debug.LogError("Deck not found.");
        }
    }

    void CreateNewDeck()
    {
        Debug.Log("Creating new deck.");
        string newDeckName = newDeckNameInputField.text;
        if (!string.IsNullOrWhiteSpace(newDeckName))
        {
            database.AddDeck(newDeckName);
            newDeckNameInputField.text = ""; // Réinitialiser le champ de texte
            ShowDeck(newDeckName); // Actualiser l'affichage des decks
        }
        else
        {
            database.AddDeck("Unnamed Deck");
            ShowDeck(newDeckName); // Actualiser l'affichage des decks
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
