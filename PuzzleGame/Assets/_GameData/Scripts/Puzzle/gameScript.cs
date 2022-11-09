using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    private Camera _camera;
    [SerializeField] private tileScript[] tiles;
    private int emptySpaceIndex = 8, currentlevel;
    public Sprite[] alltiles;
    private Sprite currentimage;
    public Image tagretImage;
    public GameObject panel;
    public Animator animator;
    bool win;
    void Start()
    {
        currentlevel = PlayerPrefs.GetInt("puzzlelevel");

        _camera = Camera.main;
        shuffle();
        alltiles = Resources.LoadAll<Sprite>("Puzzle/puzzlecopy/" + currentlevel);
        currentimage = Resources.Load<Sprite>("Puzzle/" + currentlevel);
        for (int i = 0; i < alltiles.Length - 1; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = alltiles[i];
        }
        tagretImage.sprite = currentimage;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit)
            {
                if (Vector2.Distance(a: emptySpace.position, b: hit.transform.position) < 2)
                {
                    Vector2 lastEmptySpacePos = emptySpace.position;
                    tileScript thisTile = hit.transform.GetComponent<tileScript>();

                    emptySpace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpacePos;

                    int tileIndex = findIndex(thisTile);
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;

                }
            }
        }
        int correctTiles = 0;
        foreach (var a in tiles)
        {
            if (a != null)
            {

                if (a.inRightPlace)
                {
                    correctTiles++;
                }
            }

        }
        if (correctTiles == tiles.Length - 1)
        {
            //    var a= GetComponent<timerScript>();
            //    a.StopTImer();
            timerScript.instance.StopTImer();

            StartCoroutine(waiting());


        }
    }

    void shuffle()
    {
        if (emptySpaceIndex != 8)
        {
            var tileOn8LastPos = tiles[8].targetPosition;
            tiles[15].targetPosition = emptySpace.position;
            emptySpace.position = tileOn8LastPos;
            tiles[emptySpaceIndex] = tiles[8];
            tiles[8] = null;
            emptySpaceIndex = 8;



        }


        int inversion;
        do
        {

            for (int i = 0; i <= 7; i++)
            {
                var lastPos = tiles[i].targetPosition;
                int randomIndex = Random.Range(0, 7);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;
            }
            inversion = GetInversions();
            Debug.Log("shuffled");

        } while (inversion % 2 != 0);

    }
    public int findIndex(tileScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;

    }
    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }
    IEnumerator waiting()
    {
        // yield return new WaitForSeconds(1f);
        // animator.SetTrigger("new");
        if (!win)
        {
            win = true;
            CoinManager.instance.Add(50);
            yield return new WaitForSeconds(1f);
            panel.SetActive(true);
            currentlevel++;
            Debug.Log(currentlevel);

            if (currentlevel > HomeManager.instance.totalPuzzleLevels)
            {
                currentlevel = 1;
            }
            PlayerPrefs.SetInt("puzzlelevel", currentlevel);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (Random.Range(0, 3) == 1)
            {
                if (AdNetwork.instance.isInterstitialReady)
                { AdNetwork.instance.showInterstitialAd(); }
            }
            if (FacebookLoginExp.Instance.isLoggedin)
            {
                FacebookLoginExp.Instance.saveDatatoPlayFab();
            }
        }
    }

}