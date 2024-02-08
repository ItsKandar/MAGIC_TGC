// Allows the user to search for cards using MTG api (https://api.magicthegathering.io/v1/)

using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class GetCards : MonoBehaviour
{
    public UnityEngine.UI.InputField searchBar; // Barre de recherche
    public Button searchButton; // Bouton pour lancer la recherche
    public Text resultListText; // Texte pour afficher les résultats
    public Image cardImage; // Image pour afficher la carte
    public Dropdown SetSelector; // Dropdown pour sélectionner le set

    void Start()
    {
        if (searchButton != null)
        {
            searchButton.onClick.AddListener(OnSearchClicked);
        }
    }

    void OnSearchClicked()
    {
        string cardName = searchBar.text; // Get the text from the InputField
        StartCoroutine(FetchCardFromAPI(cardName));
    }

    IEnumerator FetchCardFromAPI(string cardName)
    {
        string apiUrl = $"https://api.magicthegathering.io/v1/cards?language=french&name={UnityWebRequest.EscapeURL(cardName)}";
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Erreur : {request.error}");
                resultListText.text = "Impossible de trouver la carte : Verifiez votre connexion internet ou le nom de la carte.";
            }
            else
            {
                // Success - parse the response
                MTGCardResponse response = JsonUtility.FromJson<MTGCardResponse>(request.downloadHandler.text);
                DisplayCardList(response);
            }
        }
    }

    void DisplayCardList(MTGCardResponse response)
    {
        SetSelector.ClearOptions();
        SetSelector.onValueChanged.RemoveAllListeners();

        if (response.cards != null && response.cards.Length > 0)
        {
            List<string> options = new List<string>();
            cardVersionsBySet.Clear();

            foreach (var card in response.cards)
            {
                foreach (var foreignName in card.foreignNames.Where(fn => fn.language == "French"))
                {
                    string option = $"{foreignName.setName} - {foreignName.name}";
                    if (!cardVersionsBySet.ContainsKey(card.name))
                    {
                        cardVersionsBySet[card.name] = new Dictionary<string, MTGCard.ForeignName>();
                    }
                    if (!cardVersionsBySet[card.name].ContainsKey(foreignName.setName))
                    {
                        cardVersionsBySet[card.name][foreignName.setName] = foreignName;
                        options.Add(option);
                    }
                }
            }

            SetSelector.AddOptions(options);
            SetSelector.onValueChanged.AddListener(delegate { OnSetSelected(); });
        }
        else
        {
            resultListText.text = "Aucune carte trouvée, assurez-vous que la carte existe et que le nom est correct et en Français.";
        }
    }
    void OnSetSelected()
    {
        if (SetSelector.options.Count > 0 && SetSelector.value < SetSelector.options.Count)
        {
            string selectedOption = SetSelector.options[SetSelector.value].text;
            string setName = selectedOption.Split('-')[0].Trim();
            string cardName = selectedOption.Split('-')[1].Trim();

            foreach (var cardEntry in cardVersionsBySet)
            {
                if (cardEntry.Key == cardName && cardEntry.Value.ContainsKey(setName))
                {
                    DisplayCardDetails(cardEntry.Value[setName]);
                    break;
                }
            }
        }
    }   
    void DisplayCardDetails(ForeignName selectedCard)
    {
        // Affichez les détails de la version sélectionnée
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"<b>{selectedCard.name}</b>");
        sb.AppendLine($"- Couleurs: {selectedCard.colors}");
        sb.AppendLine($"- Coût: {selectedCard.manaCost} ({card.cmc})");
        sb.AppendLine($"- Types: {selectedCard.type}");
        sb.AppendLine($"{selectedCard.text}\n");
        sb.AppendLine($"<i>{selectedCard.flavor ?? "N/A"}</i>\n");
        sb.AppendLine($"{selectedCard.text}\n");
        sb.AppendLine($"<i>{selectedCard.flavor}</i>\n");
        sb.AppendLine($"- P/T: {selectedCard.power}/{selectedCard.toughness}");
        sb.AppendLine($"- Identité de couleur: {selectedCard.colorIdentity}");
        sb.AppendLine($"- Rareté: {selectedCard.rarity}\n");
        sb.AppendLine($"- Set: {selectedCard.setName}\n");
        sb.AppendLine($"<i>Version française de la carte</i>\n");
        resultListText.text = sb.ToString();
        DisplayCardImage(selectedCard.imageUrl);
    }
    void DisplayCardImage(string imageUrl)
    {
        StartCoroutine(FetchImage(imageUrl));
    }   
    IEnumerator FetchImage(string imageUrl)
    {
         // Convertir l'URL en HTTPS si nécessaire
        if (imageUrl.StartsWith("http://"))
        {
            imageUrl = imageUrl.Replace("http://", "https://");
        }
        
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            //image.sprite = null;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Erreur : {request.error}");
            }
            else
            {
                // Success - set the image texture
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                cardImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
        }
    }
    [System.Serializable]
    private class ForeignName
    {
        public string name;
        public string text;
        public string type;
        public string flavor;
        public string imageUrl;
        public string language;
        public string multiverseid;
    }

    [System.Serializable]
    private class MTGCardResponse
    {
        public MTGCard[] cards;
    }

    [System.Serializable]
    private class MTGCard
    {
        public ForeignName[] foreignNames;
        public string name;
        public string manaCost;
        public string cmc;
        public string colors;
        public string colorIdentity;
        public string type;
        public string supertypes;
        public string types;
        public string subtypes;
        public string rarity;
        public string set;
        public string setName;
        public string text;
        public string flavor;
        public string artist;
        public string number;
        public string power;
        public string toughness;
        public string layout;
        public string multiverseid;
        public string imageUrl;
        public string rulings;
    }
}