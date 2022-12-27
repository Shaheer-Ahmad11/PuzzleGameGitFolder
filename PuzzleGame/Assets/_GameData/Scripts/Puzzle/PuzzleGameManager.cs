using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{

    [SerializeField] private GameObject hintImage;
    private void Start()
    {

        hintImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Puzzle/" + PlayerPrefs.GetInt("puzzlelevel"));
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
