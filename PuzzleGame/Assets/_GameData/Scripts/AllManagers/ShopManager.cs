using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{


    int coins, diamonds;
    private void Start()
    {
        coins = PlayerPrefs.GetInt("totalCoins");
        diamonds = PlayerPrefs.GetInt("totaldiamonds");
    }
    public void buyCoin(int price)
    {
        if (price <= diamonds)
        {
            switch (price)
            {
                case 5:
                    {
                        CoinManager.instance.AddCoins(100);
                        break;
                    }
                case 10:
                    {
                        CoinManager.instance.AddCoins(250);
                        break;
                    }
                case 15:
                    {
                        CoinManager.instance.AddCoins(500);
                        break;
                    }
                case 20:
                    {
                        CoinManager.instance.AddCoins(800);
                        break;
                    }
                case 50:
                    {
                        CoinManager.instance.AddCoins(2000);
                        break;
                    }
                case 0:
                    {
                        CoinManager.instance.AddCoins(20);
                        break;
                    }
            }

            CoinManager.instance.RemoveDiamonds(price);
            CoinManager.instance.UpdateDiamonds();
            CoinManager.instance.UpdateCoins();
            diamonds = PlayerPrefs.GetInt("totaldiamonds");
        }

    }
}
