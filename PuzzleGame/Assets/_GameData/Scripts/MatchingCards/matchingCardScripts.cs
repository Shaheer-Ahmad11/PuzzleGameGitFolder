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
    public GameObject cardsPrefab, questiomarkprefab, hintbtn;
    public Sprite startImage, tikimage;
    List<int> cardsNumbers = new List<int>();
    private int clickedIndex, currentClickedNumber, totalMoves, TotalScored, currentcardslevel;
    private GameObject firstCard, secondCard;
    public GameObject winpanel;
    public Text MovesText, ScoreText;
    bool win;
    [SerializeField] List<int> currentselectedimagefromlist = new List<int>();
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
                PlayerPrefs.SetInt("totalcard", 2);
            }

            totalCards = PlayerPrefs.GetInt("totalcard", totalCards);

            // if (totalCards < 2 && currentcardslevel < 2)
            // {
            //     totalCards = 2;
            //     PlayerPrefs.SetInt("totalcard", totalCards);
            // }
            // else if (currentcardslevel % 2 == 0 && currentcardslevel != PlayerPrefs.GetInt("prevlevel"))
            // {
            //     totalCards += 2;
            //     PlayerPrefs.SetInt("totalcard", totalCards);
            // }
            // else
            {
                // totalCards = totalCards;
                totalCards = 8;
            }
            PlayerPrefs.SetInt("prevlevel", currentcardslevel);
            if (totalCards >= 8)
            {
                totalCards = 8;
            }
            //Changing Shape of grid view
            if (totalCards <= 10)
            {
                CardsPlacing.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                CardsPlacing.GetComponent<GridLayoutGroup>().constraintCount = 2;
            }
            else
            {

                CardsPlacing.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                CardsPlacing.GetComponent<GridLayoutGroup>().constraintCount = 3;
                // CardsPlacing.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;

                // if (totalCards <= 36)
                // { CardsPlacing.GetComponent<GridLayoutGroup>().constraintCount = 6; }
                // else if (totalCards > 36 && totalCards % 6 == 0)
                // {
                //     CardsPlacing.GetComponent<GridLayoutGroup>().constraintCount = totalCards / 6;
                // }
            }
        }
        for (int i = 0; i < totalCards / 2; i++)
        {
            Instantiate(questiomarkprefab, topscore);
        }
        int looptempindex = 0, n = 0;
        while (looptempindex < totalCards)
        {
            GameObject card = Instantiate(cardsPrefab, CardsPlacing);
            int cardnumber;
            if (looptempindex < totalCards / 2)
            {
                cardnumber = Random.Range(0, hiddenImages.Length);
                while (currentselectedimagefromlist.Contains(cardnumber))
                {
                    cardnumber = Random.Range(0, hiddenImages.Length);
                }
                if (!currentselectedimagefromlist.Contains(cardnumber))
                {
                    currentselectedimagefromlist.Add(cardnumber);
                    cardsNumbers.Add(cardnumber);
                }

            }
            else
            {
                int tempnum = Random.Range(0, cardsNumbers.Count);
                cardnumber = cardsNumbers[tempnum];
                cardsNumbers.RemoveAt(tempnum);
            }
            // changetilecolor(looptempindex);
            card.name = cardnumber.ToString();
            card.GetComponent<Button>().onClick.AddListener(onCardClick);
            card.GetComponent<Button>().interactable = false;
            looptempindex++;
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
                StartCoroutine(enableClick(false));
                TotalScored++;
                if (HomeManager.isSound)
                { SoundManager.instance.Play("cardsmatch"); }
                topscore.GetChild(TotalScored - 1).GetComponent<Image>().sprite = tikimage;
                ScoreText.text = "Score = " + TotalScored;
                StartCoroutine(offMatchedCards());
                clickedIndex = 0;

            }
            else
            {
                StartCoroutine(flipback());
                clickedIndex = 0;
            }

        }
        if (TotalScored >= totalCards / 2 && !win)
        {
            win = true;
            Debug.Log("levelWin");
            StartCoroutine(nextLevel());
        }

        if (hintindex >= 1)
        {
            if (AdNetwork.instance.isRewardedVideoAvailable)
            {
                hintbtn.transform.GetChild(0).gameObject.SetActive(true);
                hintbtn.GetComponent<Button>().interactable = true;
            }

            else
            {
                hintbtn.transform.GetChild(0).gameObject.SetActive(false);
                hintbtn.GetComponent<Button>().interactable = false;
            }
        }

        if (AdNetwork.instance.givereward && AdNetwork.instance.adclosed)
        {
            StartCoroutine(enableClick(false));
            flipAll();
            AdNetwork.instance.givereward = false;
            AdNetwork.instance.adclosed = false;
        }
    }
    private IEnumerator nextLevel()
    {
        CoinManager.instance.AddCoins(30);
        CoinManager.instance.AddDiamonds(1);
        if (HomeManager.isSound)
        { SoundManager.instance.Play("Victory"); }
        currentcardslevel++;
        Debug.Log("current level =" + currentcardslevel);
        PlayerPrefs.SetInt("cardslevel", currentcardslevel);
        yield return new WaitForSeconds(1f);
        topscore.gameObject.SetActive(false);
        winpanel.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void onCardClick()
    {
        //perform action when press on card
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
        if (clickedIndex < 2)
        {
            firstCard = EventSystem.current.currentSelectedGameObject;
            firstCard.GetComponent<Button>().interactable = false;
        }
        else if (clickedIndex > 1)
        {
            StartCoroutine(enableClick(false));
            secondCard = EventSystem.current.currentSelectedGameObject;
            secondCard.GetComponent<Button>().interactable = false;

        }
    }
    void flipCard(int currentCard, int cardname)
    { //flip only one card(Clicked Card)
        CardsPlacing.transform.GetChild(currentCard).GetChild(0).gameObject.SetActive(true);
        CardsPlacing.transform.GetChild(currentCard).GetChild(1).gameObject.SetActive(true);
        CardsPlacing.transform.GetChild(currentCard).GetChild(1).GetComponent<Image>().sprite = hiddenImages[cardname];
        // CardsPlacing.transform.GetChild(currentCard).GetComponent<Button>().interactable = false;
    }
    private IEnumerator flipback()
    {//flip back 2 clicked cards
        yield return new WaitForSeconds(1f);
        // firstCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
        firstCard.transform.GetChild(1).gameObject.SetActive(false);
        firstCard.transform.GetChild(0).gameObject.SetActive(true);
        // secondCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
        secondCard.transform.GetChild(1).gameObject.SetActive(false);
        secondCard.transform.GetChild(0).gameObject.SetActive(true);
        // firstCard = null;
        // secondCard = null;
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
        // firstCard = null;
        // secondCard = null;
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
            tempcurrentCard.transform.GetChild(0).gameObject.SetActive(false);
            tempcurrentCard.transform.GetChild(1).gameObject.SetActive(true);
            tempcurrentCard.transform.GetChild(1).GetComponent<Image>().sprite = hiddenImages[tempIndex];
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
            tempcurrentCard.transform.GetChild(1).gameObject.SetActive(false);
            tempcurrentCard.transform.GetChild(0).gameObject.SetActive(true);
            // tempcurrentCard.transform.GetChild(0).GetComponent<Image>().sprite = startImage;
            // tempcurrentCard.GetComponent<Button>().interactable = true;
        }
        StartCoroutine(enableClick(true));
    }
    int hintindex = 0;
    public void onHintClick()
    {

        Debug.Log(hintindex);
        if (hintindex >= 1)
        {
            if (AdNetwork.instance.isRewardedVideoAvailable)
            {
                AdNetwork.instance.showRewardedVideoAd();
            }
        }
        else
        {
            StartCoroutine(enableClick(false));
            flipAll();
        }
        hintindex++;
    }
    public void onBackBtnClick()
    {
        SceneManager.LoadScene("Home");
    }
}
