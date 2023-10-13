using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class dragger : MonoBehaviour
{
    public static dragger instance;
    private GameObject temptile, temptile1;
    private bool isdragging, shuffled, win;
    private Vector2 startposition, targetposition;
    private Vector3 _offset;
    [HideInInspector] public int currentlevel;
    [HideInInspector] public Sprite[] _alltiles;
    private Sprite currentimage;
    private List<Vector2> tilePositions = new List<Vector2>();
    [SerializeField] GameObject[] alltiles;
    [SerializeField] List<Transform> tiles;
    [Tooltip("Win Panel")]
    [SerializeField] public GameObject panel, border, previewPanel;
    [SerializeField] Image tagretImage;
    [SerializeField] int currentsolved;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        shuffled = false;
        currentlevel = PlayerPrefs.GetInt("puzzlelevel");

        _alltiles = Resources.LoadAll<Sprite>("Puzzle/puzzlecopy/" + currentlevel);
        currentimage = Resources.Load<Sprite>("Puzzle/" + currentlevel);

        for (int i = 0; i < alltiles.Length; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = _alltiles[i];
        }
        tagretImage.sprite = currentimage;
        previewPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = currentimage;
        previewPanel.SetActive(false);
        int length = tiles.Count;
        while (tilePositions.Count != length)
        {
            randomIndex = Random.Range(0, tiles.Count);
            tilePositions.Add(tiles[randomIndex].position);
            tiles.RemoveAt(randomIndex);
        }
        StartCoroutine(shuffle());

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && shuffled && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                if (hit.collider != null && hit.collider.gameObject.tag != "tile")
                {
                    temptile = hit.collider.gameObject;
                    temptile.GetComponent<BoxCollider2D>().enabled = false;
                    // Debug.Log(temptile.name);
                    startposition = temptile.transform.position;
                    _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - temptile.transform.position;
                    _offset.z = 0;
                    temptile.GetComponent<SpriteRenderer>().sortingOrder = 100;
                    temptile.transform.localScale = new Vector2(temptile.transform.localScale.x + .01f, temptile.transform.localScale.y + .01f);
                    isdragging = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && temptile != null)
        {
            isdragging = false;

            temptile.GetComponent<BoxCollider2D>().enabled = true;
            temptile.transform.localScale = new Vector2(temptile.transform.localScale.x - .01f, temptile.transform.localScale.y - .01f);

            if (temptile1 == null)
            {
                temptile.transform.position = startposition;
            }
            else
            {
                temptile.transform.position = targetposition;
                temptile1.transform.position = Vector2.Lerp(targetposition, startposition, 2f);
            }
            temptile.GetComponent<SpriteRenderer>().sortingOrder = 1;
            temptile = null;
            temptile1 = null;


        }
        if (isdragging)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            temptile.transform.position = mousePosition - _offset;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                temptile1 = hit.collider.gameObject;
                targetposition = hit.transform.position;
            }
        }



    }//update

    int randomIndex;
    IEnumerator shuffle()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < alltiles.Length; i++)
        {
            alltiles[i].transform.position = tilePositions[i];
        }
        shuffled = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(checksolved());
    }

    private IEnumerator checksolved()
    {
        yield return new WaitForEndOfFrame();
        currentsolved = 0;
        foreach (GameObject a in alltiles)
        {
            if (a != null)
            {
                if (a.GetComponent<TileColliderScript>().inRightPlace)
                {
                    currentsolved++;
                }
            }
        }
        if (currentsolved == alltiles.Length)
        {
            StartCoroutine(waiting());
        }
        else
        {
            StartCoroutine(checksolved());
        }
    }

    public void onClickPreview()
    {
        previewPanel.SetActive(true);
    }
    IEnumerator waiting()
    {
        // yield return new WaitForSeconds(1f);
        // animator.SetTrigger("new");

        Debug.Log("win");
        if (!win)
        {
            win = true;

            CoinManager.instance.AddCoins(30);
            CoinManager.instance.AddDiamonds(1);
            yield return new WaitForSeconds(2f);
            // gameObject.SetActive(false);
            foreach (GameObject t in alltiles)
            {
                t.SetActive(false);
            }
            border.SetActive(false);
            panel.SetActive(true);
            if (HomeManager.isSound)
            {
                SoundManager.instance.Play("Victory");
                SoundManager.instance.Stop("PuzzleBG");
            }
            currentlevel++;
            Debug.Log(currentlevel);

            if (currentlevel > HomeManager.instance.totalPuzzleLevels)
            {
                currentlevel = 1;
            }
            PlayerPrefs.SetInt("puzzlelevel", currentlevel);
            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (Random.Range(0, 3) == 1)
            {
                if (AdNetwork.instance.isInterstitialReady)
                { AdNetwork.instance.showInterstitialAd(); }
            }
            /*if (FacebookLoginExp.Instance.isLoggedin)
            {
                FacebookLoginExp.Instance.saveDatatoPlayFab();
            }*/
        }
    }
}
