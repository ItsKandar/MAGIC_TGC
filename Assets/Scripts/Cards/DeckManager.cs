using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // Liste représentant le deck de l'utilisateur avec les cartes qu'il contient.
    public List<CardInfo> userDeck = new List<CardInfo>();

    // Référence à CardDatabase
    public CardDatabase cardDatabase; // Assignez cette référence dans l'inspecteur Unity

    private void Awake()
    {
        // Charger le deck de l'utilisateur au démarrage du jeu
        LoadDeck();
    }

    // Ajoute une carte au deck de l'utilisateur
    public void AddCardToDeck(CardInfo cardToAdd)
    {
        // Ajoute la carte à la liste
        userDeck.Add(cardToAdd);
        // Sauvegarde le deck mis à jour
        SaveDeck();
    }

    // Retire une carte du deck de l'utilisateur
    public void RemoveCardFromDeck(CardInfo cardToRemove)
    {
        // Retire la carte spécifiée de la liste
        userDeck.Remove(cardToRemove);
        // Sauvegarde le deck après la suppression
        SaveDeck();
    }

    // Sauvegarde le deck de l'utilisateur dans PlayerPrefs
    public void SaveDeck()
    {
        // Convertit la liste de cartes en JSON
        string json = JsonUtility.ToJson(new SerializableDeck(userDeck));
        // Stocke le JSON dans PlayerPrefs sous la clé "UserDeck"
        PlayerPrefs.SetString("UserDeck", json);
        // Applique les changements à PlayerPrefs
        PlayerPrefs.Save();
    }

    // Charge le deck de l'utilisateur depuis PlayerPrefs
    public void LoadDeck()
    {
        // Vérifie si un deck est déjà sauvegardé
        if (PlayerPrefs.HasKey("UserDeck"))
        {
            // Récupère le JSON du deck sauvegardé
            string json = PlayerPrefs.GetString("UserDeck");
            // Convertit le JSON en objet SerializableDeck
            SerializableDeck loadedDeck = JsonUtility.FromJson<SerializableDeck>(json);
            // Convertit SerializableDeck en une liste de CardInfo et l'attribue à userDeck
            userDeck = loadedDeck.ToCardInfoList();
        }
        else
        {
            // Si aucun deck n'est sauvegardé, initialise userDeck avec une liste vide
            userDeck = new List<CardInfo>();
        }
    }

    // Méthode pour récupérer toutes les cartes de la base de données
    public void GetAllCardsFromDatabase()
    {
        if (cardDatabase != null)
        {
            // Récupère toutes les cartes de la base de données et les ajoute à userDeck
            userDeck = cardDatabase.GetAllCards();
            SaveDeck(); // Optionnel, si vous souhaitez sauvegarder immédiatement cette liste comme le deck de l'utilisateur
        }
        else
        {
            Debug.LogError("CardDatabase is not assigned in DeckManager.");
        }
    }

    [System.Serializable]
    private class SerializableDeck
    {
        // Liste interne des cartes pour la sérialisation
        public List<CardInfo> cards;

        // Constructeur pour initialiser avec une liste existante de cartes
        public SerializableDeck(List<CardInfo> cards)
        {
            this.cards = cards;
        }

        // Convertit cet objet SerializableDeck en une liste de CardInfo pour utilisation dans le jeu
        public List<CardInfo> ToCardInfoList()
        {
            return cards ?? new List<CardInfo>();
        }
    }
}