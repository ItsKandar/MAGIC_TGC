    // Allows the user to search for cards using MTG api (https://api.magicthegathering.io/v1/)

    using System;
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
        private CardDatabase database;

        void Start()
        {
            database = CardDatabase.LoadDatabase();
            if (database == null)
            {
                Debug.LogError("Failed to load the card database.");
                // Initialiser database ici ou gérer l'erreur comme nécessaire
            }
            if (searchButton != null)
            {
                searchButton.onClick.AddListener(() => OnSearchClicked()); // Pour lancer la recherche
            }
        }

        void OnSearchClicked()
        {
            if (searchBar == null || resultListText == null || database == null)
            {
                Debug.LogError("One or more required components are not assigned.");
                return;
            }

            string cardName = searchBar.text; // Get the text from the InputField
            CardInfo foundCard = database.FindCardByName(cardName);

            if (foundCard != null)
            {
                // La carte est trouvée dans la base de données locale
                DisplayLocalCard(foundCard);
            }
            else
            {
                // La carte n'est pas trouvée localement, faire une requête API
                StartCoroutine(FetchCardFromAPI(cardName));
            }
            // Debug.Log(database.GetAllCards());
        }

        // Si la carte existe deja dans la BDD
        void DisplayLocalCard(CardInfo card)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<b>{card.Name}</b>");
            sb.AppendLine($"- Couleurs: {string.Join(", ", card.Colors)}");
            sb.AppendLine($"- Coût: {card.ManaCost} ({card.Cmc})");
            if (!string.IsNullOrEmpty(card.Type))
                sb.AppendLine($"- Types: {card.Type}");
            sb.AppendLine($"{card.Text}\n");
            if (!string.IsNullOrEmpty(card.Flavor))
                sb.AppendLine($"<i>{card.Flavor}</i>\n");
            if (!string.IsNullOrEmpty(card.Power) && !string.IsNullOrEmpty(card.Toughness))
                sb.AppendLine($"- P/T: {card.Power}/{card.Toughness}");
            if (card.ColorIdentity != null && card.ColorIdentity.Length > 0)
                sb.AppendLine($"- Identité de couleur: {string.Join(", ", card.ColorIdentity)}");
            sb.AppendLine($"- Rareté: {card.Rarity}\n");
            sb.AppendLine($"<i>Version française de la carte</i>\n");

            resultListText.text = sb.ToString();
            DisplayCardImage(card.ImageUrl); // Assurez-vous que cette méthode gère correctement les images locales ou distantes
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
                    resultListText.text = "Erreur lors de la recherche : Verifiez votre connexion internet";
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
                        if (card.colors != "")
                        {
                            sb.AppendLine($"- Couleurs: {card.colors}");
                        }
                        sb.AppendLine($"- Coût: {card.manaCost} ({card.cmc})");
                        if (frenchCardInfo.type != null)
                        {
                            sb.AppendLine($"- Types: {frenchCardInfo.type}");
                        }
                        sb.AppendLine($"{frenchCardInfo.text}\n");
                        if (frenchCardInfo.flavor != "")
                        {
                            sb.AppendLine($"<i>{frenchCardInfo.flavor}</i>\n");
                        }
                        if (card.power != null && card.toughness != null)
                        {
                            sb.AppendLine($"- P/T: {card.power}/{card.toughness}");
                        }
                        if (card.colorIdentity != "")
                        {
                            sb.AppendLine($"- Identité de couleur: {card.colorIdentity}");
                        }
                        sb.AppendLine($"- Rareté: {card.rarity}\n");
                        sb.AppendLine($"<i>Version française de la carte</i>\n");
                        DisplayCardImage(frenchCardInfo.imageUrl);
                        CardInfo newCardInfo = new CardInfo(
                            frenchCardInfo.name, // Utilisez le nom français si disponible
                            card.colors?.Split(',') ?? new string[0], // Suppose que card.colors est une string; ajustez selon le vrai format
                            card.manaCost,
                            float.TryParse(card.cmc, out float cmcValue) ? cmcValue : 0, // Convertit la chaîne cmc en float
                            frenchCardInfo.type ?? card.type, // Utilisez le type français si disponible, sinon le type par défaut
                            frenchCardInfo.text ?? card.text, // Utilisez le texte français si disponible, sinon le texte par défaut
                            frenchCardInfo.flavor ?? card.flavor, // Utilisez le texte d'ambiance français si disponible
                            card.power,
                            card.toughness,
                            card.colorIdentity?.Split(',') ?? new string[0], // Suppose que colorIdentity est une string; ajustez selon le vrai format
                            card.rarity,
                            frenchCardInfo.imageUrl ?? card.imageUrl // Utilisez l'URL de l'image française si disponible
                        );
                        Debug.Log(newCardInfo);
                        // Récupérer la base de données existante ou en créer une nouvelle
                        CardDatabase database = CardDatabase.LoadDatabase();
                        
                        // Ajouter la carte à la base de données
                        database.AddCard(newCardInfo);
                        
                        // Sauvegarder la base de données mise à jour
                        database.SaveDatabase();
                        break;
                    }
                    else
                    {
                        // Si aucune version française n'est trouvée, affichez les informations de base de la carte
                        sb.AppendLine($"<b>{card.name}</b>");
                        if (card.colors != "")
                        {
                            sb.AppendLine($"- Couleurs: {card.colors}");
                        }
                        sb.AppendLine($"- Coût: {card.manaCost} ({card.cmc})");
                        if (card.type != null)
                        {
                            sb.AppendLine($"- Types : {card.type}");
                        }
                        sb.AppendLine($"{card.text}\n");
                        if (card.flavor != "")
                        {
                            sb.AppendLine($"<i>{card.flavor}</i>\n");
                        }
                        if (card.power != null && card.toughness != null)
                        {
                            sb.AppendLine($"- P/T: {card.power}/{card.toughness}");
                        }
                        if (card.colorIdentity != "")
                        {
                            sb.AppendLine($"- Identité de couleur: {card.colorIdentity}");
                        }
                        sb.AppendLine($"- Rareté: {card.rarity}\n");
                        sb.AppendLine($"<i>Version anglaise de la carte</i>\n");
                        DisplayCardImage(card.imageUrl);
                        break;
                    }
                }
                resultListText.text = sb.ToString();
            }
            else
            {
                resultListText.text = "Aucun résultat pour " + searchBar.text + ", assurez-vous que la carte existe et que le nom est correct et en Français.";
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

    [Serializable]
    public class CardInfo
    {
        public string Name { get; set; }
        public string[] Colors { get; set; }
        public string ManaCost { get; set; }
        public float Cmc { get; set; } // Coût de mana converti
        public string Type { get; set; }
        public string Text { get; set; }
        public string Flavor { get; set; } // Texte d'ambiance
        public string Power { get; set; } // Force
        public string Toughness { get; set; } // Endurance
        public string[] ColorIdentity { get; set; }
        public string Rarity { get; set; } // Rareté
        public string ImageUrl { get; set; }

        // Constructeur sans paramètres pour la sérialisation/désérialisation
        public CardInfo() { }

        // Constructeur pour initialiser facilement une instance
        public CardInfo(string name, string[] colors, string manaCost, float cmc, string type, 
                        string text, string flavor, string power, string toughness, 
                        string[] colorIdentity, string rarity, string imageUrl)
        {
            Name = name;
            Colors = colors;
            ManaCost = manaCost;
            Cmc = cmc;
            Type = type;
            Text = text;
            Flavor = flavor;
            Power = power;
            Toughness = toughness;
            ColorIdentity = colorIdentity;
            Rarity = rarity;
            ImageUrl = imageUrl;
        }

        // Méthode pour générer la description de la carte
        public string GetCardDescription()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"<b>{Name}</b>");
            sb.AppendLine($"- Couleurs: {string.Join(", ", Colors)}");
            sb.AppendLine($"- Coût: {ManaCost} ({Cmc})");
            if (!string.IsNullOrEmpty(Type))
                sb.AppendLine($"- Types: {Type}");
            if (!string.IsNullOrEmpty(Text))
                sb.AppendLine($"{Text}\n");
            if (!string.IsNullOrEmpty(Flavor))
                sb.AppendLine($"<i>{Flavor}</i>\n");
            if (!string.IsNullOrEmpty(Power) && !string.IsNullOrEmpty(Toughness))
                sb.AppendLine($"- P/T: {Power}/{Toughness}");
            if (ColorIdentity != null && ColorIdentity.Length > 0)
                sb.AppendLine($"- Identité de couleur: {string.Join(", ", ColorIdentity)}");
            sb.AppendLine($"- Rareté: {Rarity}\n");
            sb.AppendLine($"<i>Version française de la carte</i>\n");

            return sb.ToString();
        }
    }