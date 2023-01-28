using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{

    public static PuzzleGameManager instance;
    [SerializeField] private GameObject hintImage;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

        hintImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Puzzle/" + PlayerPrefs.GetInt("puzzlelevel"));
        if (HomeManager.isSound)
        {
            foreach (var s in SoundManager.instance.sounds)
            {
                SoundManager.instance.Stop(s.name);
            }
            SoundManager.instance.Play("PuzzleBG");
        }
    }


    public void onBackButtonClick()
    {
        SceneManager.LoadScene("Home");
    }

    public void onHintClick()
    {

        StartCoroutine(showHint());
    }


    private IEnumerator showHint()
    {
        hintImage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        hintImage.SetActive(false);
    }


}
