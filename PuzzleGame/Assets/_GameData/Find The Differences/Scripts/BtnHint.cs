using UnityEngine;
using UnityEngine.UI;

public class BtnHint : MonoBehaviour
{
    private GameObject hintSpot;
    public static BtnHint instance;
    public bool ishint;
    private bool enable = true;

    private int power;

    public Sprite spNormalMod;

    public Sprite spPowerMod;

    private static float hintTime;
    private int hintCount = 0;

    private void Start()
    {
        hintSpot = GameObject.Find("HintSpot");
        hintTime = 6f;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (hintCount >= 1)
        {
            if (AdNetwork.instance.isRewardedVideoAvailable)
            {
                GetComponent<Button>().interactable = true;
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                GetComponent<Button>().interactable = false;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if (AdNetwork.instance.givereward && AdNetwork.instance.adclosed)
        {
            callshowhint();
            AdNetwork.instance.givereward = false;
            AdNetwork.instance.adclosed = false;
        }
    }

    public void Onhintbutttonclick()
    {
        if (hintCount >= 1)
        {
            if (AdNetwork.instance.isRewardedVideoAvailable)
            {
                AdNetwork.instance.showRewardedVideoAd();

                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                // callshowhint();
                ishint = true;
            }
        }
        else
        {
            callshowhint();
        }
        GameManager.REF.isoverobject = true;
    }
    public void callshowhint()
    {
        power = UnityEngine.Random.Range(0, 4);
        base.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        Sound.REF.Play("sndClick");
        Invoke("ShowHint", 0.3f);
        Sound.REF.Play("sndHint");
        enable = false;
    }
    private void ShowHint()
    {
        hintCount++;

        hintSpot.GetComponent<Animator>().Play("animFadeIn");
        // base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        // base.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
        GameObject[] array = GameObject.FindGameObjectsWithTag("PointSpot");
        int num = 0;
        int num2 = UnityEngine.Random.Range(0, array.Length);
        int num3 = 200;
        while (!array[num2].GetComponent<Point>().enable && num3-- > 0)
        {
            num2 = UnityEngine.Random.Range(0, array.Length);
        }
        GameObject[] array2 = array;
        foreach (GameObject gameObject in array2)
        {
            if (num == num2)
            {
                float y;
                if (power == 3)
                {
                    y = 0f;
                    hintSpot.GetComponent<SpriteRenderer>().sprite = spPowerMod;
                    Sound.REF.Play("sndHintPower");
                }
                else
                {
                    // y = UnityEngine.Random.Range(-0.8f, 0.8f);
                    y = 0f;
                    hintSpot.GetComponent<SpriteRenderer>().sprite = spNormalMod;
                }
                hintSpot.transform.position = gameObject.transform.position - new Vector3(0f, y, 0f);
            }
            num++;
        }
        Invoke("EnableHint", hintTime);
        // hintTime += 5f;
    }

    private void EnableHint()
    {
        enable = true;
        base.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

    }
}
