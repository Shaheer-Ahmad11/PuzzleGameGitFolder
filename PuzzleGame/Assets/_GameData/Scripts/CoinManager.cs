using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinManager : MonoBehaviour
{
    [SerializeField] bool istest;
    public string testCoins, testDiamonds;
    public static CoinManager instance;
    public Text CoinValueText, DiamondValueText, coinvaluetextinPanel, diamondvaluetextinPanel;
    public int totalCoins, totalDiamonds;
    void Start()
    {
        if (instance == null)
        { instance = this; }
        if (istest)
        {
            totalCoins = int.Parse(testCoins);
            PlayerPrefs.SetInt("totalCoins", totalCoins);
            totalDiamonds = int.Parse(testDiamonds);
            PlayerPrefs.SetInt("totaldiamonds", totalDiamonds);
        }
        totalCoins = PlayerPrefs.GetInt("totalCoins");
        totalDiamonds = PlayerPrefs.GetInt("totaldiamonds");
        UpdateCoins();
        UpdateDiamonds();
    }

    public void AddCoins(int value)
    {
        totalCoins += value;
        PlayerPrefs.SetInt("totalCoins", totalCoins);
    }
    public void RemoveCoins(int value)
    {
        totalCoins -= value;
        PlayerPrefs.SetInt("totalCoins", totalCoins);
    }
    public void UpdateCoins()
    {
        CoinValueText.text = totalCoins.ToString();
        coinvaluetextinPanel.text = totalCoins.ToString();
    }

    //diamonds
    public void AddDiamonds(int value)
    {
        totalDiamonds += value;
        PlayerPrefs.SetInt("totaldiamonds", totalDiamonds);
    }
    public void RemoveDiamonds(int value)
    {
        totalDiamonds -= value;
        PlayerPrefs.SetInt("totaldiamonds", totalDiamonds);
    }
    public void UpdateDiamonds()
    {
        DiamondValueText.text = totalDiamonds.ToString();
        diamondvaluetextinPanel.text = totalDiamonds.ToString();
    }
}
