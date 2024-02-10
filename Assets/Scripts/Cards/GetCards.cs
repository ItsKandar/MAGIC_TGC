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

    void Start()
    {
        if (searchButton != null)
        {
            searchButton.onClick.AddListener(() => OnSearchClicked()); // Pour lancer la recherche
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
        if (response.cards != null && response.cards.Length > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var card in response.cards)
            {
                // Recherche des informations de la carte en français
                var frenchCardInfo = card.foreignNames?.FirstOrDefault(fn => fn.language == "French");
                if (frenchCardInfo != null)
                {
                    sb.Clear();
                    // Utilisez frenchCardInfo pour afficher les détails de la carte en français
                    sb.AppendLine($"<b>{frenchCardInfo.name}</b>");
                    sb.AppendLine($"- Couleurs: {card.colors}");
                    sb.AppendLine($"- Coût: {card.manaCost} ({card.cmc})");
                    if (frenchCardInfo.type != null)
                    {
                        sb.AppendLine($"- Types: {frenchCardInfo.type}");
                    }
                    sb.AppendLine($"{frenchCardInfo.text}\n");
                    if (frenchCardInfo.flavor != null)
                    {
                        sb.AppendLine($"<i>{frenchCardInfo.flavor}</i>\n");
                    }
                    if (card.power != null && card.toughness != null)
                    {
                        sb.AppendLine($"- P/T: {card.power}/{card.toughness}");
                    }
                    if (card.colorIdentity != null)
                    {
                        sb.AppendLine($"- Identité de couleur: {card.colorIdentity}");
                    }
                    sb.AppendLine($"- Rareté: {card.rarity}\n");
                    sb.AppendLine($"<i>Version française de la carte</i>\n");
                    DisplayCardImage(frenchCardInfo.imageUrl);
                }
                else
                {
                    // Si aucune version française n'est trouvée, affichez les informations de base de la carte
                    sb.AppendLine($"<b>{card.name}</b>");
                    sb.AppendLine($"- Couleurs: {card.colors}");
                    sb.AppendLine($"- Coût: {card.manaCost} ({card.cmc})");
                    if (card.type != null)
                    {
                        sb.AppendLine($"- Types : {card.type}");
                    }
                    sb.AppendLine($"{card.text}\n");
                    if (card.flavor != null)
                    {
                        sb.AppendLine($"<i>{card.flavor}</i>\n");
                    }
                    if (card.power != null && card.toughness != null)
                    {
                        sb.AppendLine($"- P/T: {card.power}/{card.toughness}");
                    }
                    if (card.colorIdentity != null)
                    {
                        sb.AppendLine($"- Identité de couleur: {card.colorIdentity}");
                    }
                    sb.AppendLine($"- Rareté: {card.rarity}\n");
                    sb.AppendLine($"<i>Version anglaise de la carte</i>\n");
                    DisplayCardImage(card.imageUrl);
                }
            }
            resultListText.text = sb.ToString();
        }
        else
        {
            resultListText.text = "Aucune carte trouvée, assurez-vous que la carte existe et que le nom est correct et en Français.";
        }
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