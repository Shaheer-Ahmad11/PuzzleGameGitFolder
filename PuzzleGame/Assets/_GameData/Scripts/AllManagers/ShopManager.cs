using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class wallpapers
    {
        public Sprite wallpaper;
        public int value;

    }
    [SerializeField] private List<wallpapers> _wallpapers = new List<wallpapers>();
    [SerializeField] private Transform allWallpapers;
    [SerializeField] private Image[] wallpaperShowcase;
    int coins, diamonds;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("appliedWallpaper"))
        {

        }
        else
        {
            applyWallpaper(PlayerPrefs.GetInt("applyWallpaper", 0));
        }
        coins = PlayerPrefs.GetInt("totalCoins");
        diamonds = PlayerPrefs.GetInt("totaldiamonds");
        for (int i = 0; i < allWallpapers.childCount; i++)
        {
            Transform currentWallpaper = allWallpapers.GetChild(i);
            currentWallpaper.GetChild(0).GetComponent<Image>().sprite = _wallpapers[i].wallpaper;
            currentWallpaper.GetChild(1).GetChild(1).GetComponent<Text>().text = _wallpapers[i].value.ToString();
            currentWallpaper.GetChild(1).GetComponent<Button>().onClick.AddListener(buyWallpaper);
            // currentWallpaper.GetComponent<Button>().onClick.AddListener(() => applyWallpaper(i));
            currentWallpaper.GetComponent<Button>().AddEventListener(i, applyWallpaper);
        }

        updateWallpapers();
    }
    public void buyCoin(int price)
    {
        coins = PlayerPrefs.GetInt("totalCoins");
        diamonds = PlayerPrefs.GetInt("totaldiamonds");
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

    public void buyWallpaper()
    {
        coins = PlayerPrefs.GetInt("totalCoins");
        diamonds = PlayerPrefs.GetInt("totaldiamonds");
        int currentWallpaperindex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        if (coins >= _wallpapers[currentWallpaperindex].value)
        {
            PlayerPrefs.SetString("w_ispurchased" + currentWallpaperindex, "true");

            CoinManager.instance.RemoveCoins(_wallpapers[currentWallpaperindex].value);
            CoinManager.instance.UpdateCoins();
        }
        updateWallpapers();

    }


    private void updateWallpapers()
    {
        for (int i = 0; i < allWallpapers.childCount; i++)
        {
            Transform currentWallpaper = allWallpapers.GetChild(i);
            if (PlayerPrefs.GetString("w_ispurchased" + i, "false") == "true")
            {
                currentWallpaper.GetComponent<Button>().interactable = true;
                currentWallpaper.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                currentWallpaper.GetComponent<Button>().interactable = false;
                currentWallpaper.GetChild(1).gameObject.SetActive(true);
            }

        }
    }

    private void applyWallpaper(int index)
    {
        foreach (Image i in wallpaperShowcase)
        {
            i.sprite = _wallpapers[index].wallpaper;
        }
        PlayerPrefs.SetInt("appliedWallpaper", index);
    }

}
