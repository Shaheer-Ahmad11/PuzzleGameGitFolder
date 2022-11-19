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
    [SerializeField] Transform CardsPlacing, Solved;
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

                Debug.Log("current level =" + currentcardslevel);
            }
            if (!PlayerPrefs.HasKey("totalcard"))
            {
                PlayerPrefs.SetInt("totalcard", 12);
            }

            totalCards = PlayerPrefs.GetInt("totalcard", totalCards);

            if (totalCards < 12 && currentcardslevel < 4)
            {
                totalCards = 12;
                PlayerPrefs.SetInt("totalcard", totalCards);
            }
            else if (currentcardslevel % 5 == 0 && currentcardslevel != PlayerPrefs.GetInt("prevlevel"))
            {
                totalCards += 6;
                PlayerPrefs.SetInt("totalcard", totalCards);
            }
            else
            {
                totalCards = totalCards;
            }
            PlayerPrefs.SetInt("prevlevel", currentcardslevel);
            if (totalCards >= 72)
            {
                totalCards = 72;
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
        StartCoroutine(enableClick(false));
        Invoke("flipAll", 1f);

    }
    int tileindex = 0;
    void changetilecolor(int index)
    {


        if (tileindex <= 5)
        {
            CardsPlacing.GetChild(index).gameObject.GetComponent<Image>().color = tilescolor[0];

            tileindex++;

        }
        else if (tileindex <= 12)
        {
            CardsPlacing.GetChild(index).gameObject.GetComponent<Image>().color = tilescolor[1];
            tileindex++;
        }
        if (tileindex >= 12)
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
                if (HomeManager.isSound)
                { SoundManager.instance.Play("cardsmatch"); }
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
        CoinManager.instance.Add(50);
        if (HomeManager.isSound)
        { SoundManager.instance.Play("Victory"); }
        currentcardslevel++;
        Debug.Log("current level =" + currentcardslevel);
        PlayerPrefs.SetInt("cardslevel", currentcardslevel);
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
        if (HomeManager.isSound)
        { SoundManager.instance.Play("Slide"); }
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
            StartCoroutine(enableClick(false));
            // for (int i = 0; i < CardsPlacing.childCount; i++)
            // {
            //     CardsPlacing.GetChild(i).GetComponent<Button>().interactable = false;
            // }
        }
    }
    void flipCard(int currentCard, int cardname)
    { //flip only one card(Clicked Card)
        CardsPlacing.transform.GetChild(currentCard).GetChild(0).GetComponent<Image>().sprite = hiddenImages[cardname];
        // CardsPlacing.transform.GetChild(currentCard).GetComponent<Button>().interactable = false;
    }
    private IEnumerator flipback()
    {//flip back 2 clicked cards
        yield return new WaitForSeconds(1f);
        firstCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
        secondCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
        StartCoroutine(enableClick(true));

    }
    private IEnumerator offMatchedCards()
    {
        //runs when tow cards matches

        yield return new WaitForSeconds(1f);
        firstCard.GetComponent<Button>().enabled = false;
        secondCard.GetComponent<Button>().enabled = false;
        // firstCard.transform.parent = null;
        firstCard.transform.parent = Solved;
        secondCard.transform.parent = Solved;
        StartCoroutine(enableClick(true));

    }
    private IEnumerator enableClick(bool isInteractable)
    {//enable or disable click on card when needed
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < CardsPlacing.childCount; i++)
        {
            CardsPlacing.GetChild(i).GetComponent<Button>().interactable = isInteractable;
        }
    }
    public void flipAll()
    {//flip all cards 
        StartCoroutine(enableClick(false));
        for (int i = 0; i < CardsPlacing.childCount; i++)
        {
            GameObject tempcurrentCard = CardsPlacing.GetChild(i).gameObject;
            int tempIndex = int.Parse(tempcurrentCard.name);
            tempcurrentCard.transform.GetChild(0).GetComponent<Image>().sprite = hiddenImages[tempIndex];
            Invoke("flipBackAll", 2f);
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
            // tempcurrentCard.GetComponent<Button>().interactable = true;
        }
        StartCoroutine(enableClick(true));
    }
    int hintindex;
    public void onHintClick()
    {
        if (hintindex < 1)
        {
            StartCoroutine(enableClick(false));
            flipAll();
        }
        else
        {
            StartCoroutine(enableClick(false));
            flipAll();
            // hintbtn.transform.GetChild(0).SetActive(true);
        }
        hintindex++;

    }
    public void onBackBtnClick()
    {
        SceneManager.LoadScene("Home");
    }
}
