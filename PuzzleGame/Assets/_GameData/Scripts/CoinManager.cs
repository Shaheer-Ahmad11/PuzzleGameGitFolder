using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinManager : MonoBehaviour
{
    [SerializeField] bool istest;
    public string testCoins;
    public static CoinManager instance;
    public Text CoinValueText;
    public int totalCoins;
    void Start()
    {
        if (instance == null)
        { instance = this; }
        if (!PlayerPrefs.HasKey("totalCoins"))
        {
            PlayerPrefs.SetInt("totalCoins", 100);
        }
        if (istest)
        {
            totalCoins = int.Parse(testCoins);
            PlayerPrefs.SetInt("totalCoins", totalCoins);
        }
        totalCoins = PlayerPrefs.GetInt("totalCoins");
        CoinValueText.text = totalCoins.ToString();

    }


    public void Add(int value)
    {
        totalCoins += value;
        PlayerPrefs.SetInt("totalCoins", totalCoins);
    }
    public void Remove(int value)
    {
        totalCoins -= value;
        PlayerPrefs.SetInt("totalCoins", totalCoins);
    }
}
