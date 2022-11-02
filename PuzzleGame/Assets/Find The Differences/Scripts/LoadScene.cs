using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public bool gotoNextLevel;

    public bool gotoPreviuseLevel;

    public int sceneIndex = -1;

    public string sceneName = string.Empty;

    public Sprite spTouched;

    public static LoadScene REF;

    private void Start()
    {
        if (REF == null)
        {
            REF = this;
        }
    }

    private void OnMouseUp()
    {
        Animator component = base.gameObject.GetComponent<Animator>();
        if ((bool)component)
        {
            base.gameObject.GetComponent<Animator>().enabled = false;
        }
        base.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        Invoke("GotoScene", 0.2f);
        Sound.REF.Play("sndClick");
        if ((bool)spTouched)
        {
            GetComponent<SpriteRenderer>().sprite = spTouched;
        }
    }

    private void GotoScene()
    {
        base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        Invoke("GotoSceneExecute", 0.2f);
    }

    public void GotoNextLevel()
    {
        GameManager.level++;
        SceneManager.LoadScene("lvlGame");
    }

    private void GotoPreviuseLevel()
    {
        GameManager.level--;
        SceneManager.LoadScene("lvlGame");
    }

    private void GotoSceneExecute()
    {
        if (gotoNextLevel)
        {
            GotoNextLevel();
        }
        else if (gotoPreviuseLevel)
        {
            GotoPreviuseLevel();
        }
        else if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else if (sceneName != string.Empty)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void GotoMain()
    {
        SceneManager.LoadScene(0);
    }
}
