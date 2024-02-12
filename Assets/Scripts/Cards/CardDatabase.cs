using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CardDatabase : ScriptableObject
{
    public List<CardInfo> cards;

    void OnEnable()
    {
        if (cards == null)
        {
            cards = new List<CardInfo>();
        }
    }
    public void AddCard(CardInfo newCard)
    {
        // Eviter les doublons en vérifiant si la carte est déjà présent
        if (!cards.Exists(card => card.Name == newCard.Name))
        {
            cards.Add(newCard);
            Debug.Log("Card added to database : " + newCard.Name);
        }
    }

    // Méthode pour sauvegarder la base de données dans PlayerPrefs ou un fichier
    public void SaveDatabase()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("CardDatabase", json);
        PlayerPrefs.Save();
        Debug.Log("Database saved");
    }

    // Dans CardDatabase.cs
    public CardInfo FindCardByName(string name)
    {
        // S'assure que le nom fourni n'est pas null ou vide
        if (string.IsNullOrEmpty(name)) return null;
        Debug.Log("Searching for card : " + name);
        // Retourne le premier CardInfo qui correspond au nom, en s'assurant que card et card.Name ne sont pas nulls
        var DBRequest = cards.FirstOrDefault(card => card != null && !string.IsNullOrEmpty(card.Name) && card.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        Debug.Log("Card found : " + DBRequest);
        return DBRequest;
        }

    // Méthode pour charger la base de données
    public static CardDatabase LoadDatabase()
    {
        // Vérifie si une instance de CardDatabase existe déjà dans les assets.
        // Vous devez déterminer comment vous souhaitez stocker ou localiser cet asset.
        // Cela pourrait être via des références directes, des chemins d'accès, etc.

        // Si vous ne trouvez pas d'instance existante, créez-en une nouvelle :
        CardDatabase db = ScriptableObject.CreateInstance<CardDatabase>();

        // Ici, vous pouvez charger des données dans db si nécessaire, par exemple, à partir de PlayerPrefs
        if (PlayerPrefs.HasKey("CardDatabase"))
        {
            string json = PlayerPrefs.GetString("CardDatabase");
            JsonUtility.FromJsonOverwrite(json, db);
        }

        return db;
    }

    public List<CardInfo> GetAllCards()
    {
        return cards;
    }
}