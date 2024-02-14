using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DeckInfo : ScriptableObject
{
    public string deckName;
    public List<CardInfo> cards;

    public DeckInfo(string name)
    {
        deckName = name;
        cards = new List<CardInfo>();
    }
}

[CreateAssetMenu(fileName = "DeckManager", menuName = "CardGame/DeckManager")]
public class DeckManager : ScriptableObject
{
    public List<DeckInfo> decks;

    private void OnEnable()
    {
        if (decks == null)
        {
            decks = new List<DeckInfo>();
        }
    }

    public void AddDeck(string deckName)
    {
        Debug.Log("Adding new deck to DB.");
        if (!decks.Any(deck => deck.deckName == deckName))
        {
            decks.Add(new DeckInfo(deckName));
            Debug.Log("Deck added to database : " + deckName);
        }
        else
        {
            Debug.LogError("A deck with the same name already exists.");
        }
    }

    public void AddCardToDeck(string deckName, CardInfo cardToAdd)
    {
        var deck = decks.Find(deck => deck.deckName == deckName);
        if (deck != null)
        {
            if (!deck.cards.Contains(cardToAdd))
            {
                deck.cards.Add(cardToAdd);
            }
            else
            {
                Debug.LogError("This card is already in the deck.");
            }
        }
        else
        {
            Debug.LogError("Deck not found.");
        }
    }

    public List<DeckInfo> GetDecks()
    {
        return decks;
    }

    public DeckInfo GetUserDeck(string deckName)
    {
        return decks.FirstOrDefault(deck => deck.deckName == deckName);
    }

    // Load Database
    public static DeckManager LoadDatabase()
    {
        DeckManager db = ScriptableObject.CreateInstance<DeckManager>();
        if (PlayerPrefs.HasKey("DeckManager"))
        {
            string json = PlayerPrefs.GetString("DeckManager");
            JsonUtility.FromJsonOverwrite(json, db);
        }
        return db;
    }
}