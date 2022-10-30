using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class matchingCardScripts : MonoBehaviour
{

    public Sprite[] hiddenImages;
    [SerializeField] int totalCards;
    [SerializeField] Transform CardsPlacing;
    public GameObject cardsPrefab;
    List<int> cardsNumbers = new List<int>();
    void Start()
    {
        for (int i = 0; i < totalCards; i++)
        {
            GameObject card = Instantiate(cardsPrefab, CardsPlacing);
            int cardnumber;
            if (i < totalCards / 2)
            {
                cardnumber = Random.RandomRange(0, hiddenImages.Length);
                cardsNumbers.Add(cardnumber);
            }
            else
            {

                int tempnum = Random.RandomRange(0, cardsNumbers.Count);

                cardnumber = cardsNumbers[tempnum];
                cardsNumbers.RemoveAt(tempnum);
            }

            card.name = cardnumber.ToString();
            card.GetComponent<Button>().onClick.AddListener(onCardClick);
        }

    }


    void Update()
    {

    }

    public void onCardClick()
    {
        GameObject currentCard = EventSystem.current.currentSelectedGameObject;
        int Cardindex = currentCard.transform.GetSiblingIndex();
        int cardname = int.Parse(currentCard.name);
        flipCard(Cardindex, cardname);
    }

    void flipCard(int currentCard, int cardname)
    {
        CardsPlacing.transform.GetChild(currentCard).GetComponent<Image>().sprite = hiddenImages[cardname];
    }
}
