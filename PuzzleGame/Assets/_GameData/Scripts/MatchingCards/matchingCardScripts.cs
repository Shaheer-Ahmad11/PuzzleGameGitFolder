using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class matchingCardScripts : MonoBehaviour
{
    public Color[] tilescolor;
    public Sprite[] hiddenImages;
    public Transform topscore;
    [SerializeField] int totalCards;
    [SerializeField] Transform CardsPlacing;
    public GameObject cardsPrefab, questiomarkprefab;
    public Sprite startImage, tikimage;
    List<int> cardsNumbers = new List<int>();
    private int clickedIndex, currentClickedNumber, totalMoves, TotalScored, currentcardslevel;
    private GameObject firstCard, secondCard;
    public GameObject winpanel;
    public Text MovesText, ScoreText;
    bool win;
    void Start()
    {
        {
            if (!PlayerPrefs.HasKey("cardslevel"))
            {
                PlayerPrefs.SetInt("cardslevel", 1);
            }
            else
            {
                currentcardslevel = PlayerPrefs.GetInt("cardslevel");
            }
            if (!PlayerPrefs.HasKey("totalcard"))
            {
                PlayerPrefs.SetInt("totalcard", 12);
            }

            totalCards = PlayerPrefs.GetInt("totalcard", totalCards);
            if (totalCards < 12 && currentcardslevel < 12)
            {
                totalCards = 12;
                PlayerPrefs.SetInt("totalcard", totalCards);
            }
            else if (currentcardslevel % 4 == 0)
            {
                totalCards = currentcardslevel;
                PlayerPrefs.SetInt("totalcard", totalCards);
            }
            else
            {
                totalCards = totalCards;
            }
        }
        for (int i = 0; i < totalCards / 2; i++)
        {
            Instantiate(questiomarkprefab, topscore);
        }
        for (int i = 0; i < totalCards; i++)
        {
            GameObject card = Instantiate(cardsPrefab, CardsPlacing);




            int cardnumber;
            if (i < totalCards / 2)
            {
                cardnumber = Random.Range(0, hiddenImages.Length);
                cardsNumbers.Add(cardnumber);
            }
            else
            {
                int tempnum = Random.Range(0, cardsNumbers.Count);
                cardnumber = cardsNumbers[tempnum];
                cardsNumbers.RemoveAt(tempnum);
            }
            changetilecolor(i);
            card.name = cardnumber.ToString();
            card.GetComponent<Button>().onClick.AddListener(onCardClick);
            card.GetComponent<Button>().interactable = false;

        }
        Invoke("flipAll", 1f);
        Invoke("flipBackAll", 3f);
    }
    int tileindex = 0;
    void changetilecolor(int index)
    {


        if (tileindex <= 3)
        {
            CardsPlacing.GetChild(index).gameObject.GetComponent<Image>().color = tilescolor[0];
            Debug.Log((tileindex));
            tileindex++;

        }
        else if (tileindex <= 8)
        {
            CardsPlacing.GetChild(index).gameObject.GetComponent<Image>().color = tilescolor[1];
            tileindex++;
        }
        if (tileindex >= 8)
        {
            tileindex = 0;
        }
    }
    void Update()
    {
        if (clickedIndex >= 2)
        {
            if (firstCard.name == secondCard.name)
            {
                TotalScored++;
                topscore.GetChild(TotalScored - 1).GetComponent<Image>().sprite = tikimage;
                ScoreText.text = "Score = " + TotalScored;
                StartCoroutine(offMatchedCards());
            }
            else
            {
                StartCoroutine(flipback());
            }
            clickedIndex = 0;
        }
        if (TotalScored >= totalCards / 2 && !win)
        {
            win = true;
            Debug.Log("levelWin");
            StartCoroutine(nextLevel());
        }
    }
    private IEnumerator nextLevel()
    {
        currentcardslevel++;
        PlayerPrefs.SetInt("cardslevl", currentcardslevel);
        yield return new WaitForSeconds(1f);
        topscore.gameObject.SetActive(false);
        winpanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void onCardClick()
    {  //perform action when press on card
        if (CardsPlacing.GetComponent<GridLayoutGroup>().enabled == true)
        {
            CardsPlacing.GetComponent<GridLayoutGroup>().enabled = false;
        }
        clickedIndex++;
        currentClickedNumber++;
        totalMoves = currentClickedNumber / 2;
        MovesText.text = "Total Moves = " + totalMoves;
        GameObject currentCard = EventSystem.current.currentSelectedGameObject;
        int Cardindex = currentCard.transform.GetSiblingIndex();
        int cardname = int.Parse(currentCard.name);
        flipCard(Cardindex, cardname);
        if (clickedIndex == 1)
        {
            firstCard = EventSystem.current.currentSelectedGameObject;
            firstCard.GetComponent<Button>().interactable = false;
        }
        else if (clickedIndex >= 2)
        {
            secondCard = EventSystem.current.currentSelectedGameObject;
            secondCard.GetComponent<Button>().interactable = false;
            enableClick(false);
            // for (int i = 0; i < CardsPlacing.childCount; i++)
            // {
            //     CardsPlacing.GetChild(i).GetComponent<Button>().interactable = false;
            // }
        }
    }
    void flipCard(int currentCard, int cardname)
    { //flip only one card(Clicked Card)
        CardsPlacing.transform.GetChild(currentCard).GetChild(0).GetComponent<Image>().sprite = hiddenImages[cardname];
    }
    private IEnumerator flipback()
    {//flip back 2 clicked cards
        yield return new WaitForSeconds(1f);
        firstCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
        secondCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
        enableClick(true);

    }
    private IEnumerator offMatchedCards()
    {
        //runs when tow cards matches

        yield return new WaitForSeconds(1f);
        firstCard.GetComponent<Button>().enabled = false;
        secondCard.GetComponent<Button>().enabled = false;
        enableClick(true);

    }
    void enableClick(bool isInteractable)
    {//enable or disable click on card when needed
        for (int i = 0; i < CardsPlacing.childCount; i++)
        {
            CardsPlacing.GetChild(i).GetComponent<Button>().interactable = isInteractable;
        }
    }
    public void flipAll()
    {//flip all cards 
        for (int i = 0; i < CardsPlacing.childCount; i++)
        {
            GameObject tempcurrentCard = CardsPlacing.GetChild(i).gameObject;
            int tempIndex = int.Parse(tempcurrentCard.name);
            tempcurrentCard.transform.GetChild(0).GetComponent<Image>().sprite = hiddenImages[tempIndex];
            // tempcurrentCard.GetComponent<Button>().interactable = false;
        }
    }
    public void flipBackAll()
    { //flip back all cards
        for (int i = 0; i < CardsPlacing.childCount; i++)
        {
            GameObject tempcurrentCard = CardsPlacing.GetChild(i).gameObject;
            int tempIndex = int.Parse(tempcurrentCard.name);
            tempcurrentCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
            tempcurrentCard.GetComponent<Button>().interactable = true;
        }
    }
    public void onBackBtnClick()
    {
        SceneManager.LoadScene("Home");
    }
}
