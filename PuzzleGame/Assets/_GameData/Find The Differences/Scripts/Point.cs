using UnityEngine;
using UnityEngine.EventSystems;

public class Point : MonoBehaviour
{
    public Sprite spPointCheck;
    // public static bool isoverobject;
    public bool enable = true;
    // public static bool heart=false;

    private void Start()
    {
    }

    private void Update()
    {
    }
    public bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }
    private void OnMouseUp()
    {
        if (enable)
        {
            if (HomeManager.isSound)
            { SoundManager.instance.Play("differencespoted"); }
            GameManager.REF.isoverobject = true;
            enable = false;
            base.gameObject.GetComponent<SpriteRenderer>().sprite = spPointCheck;
            base.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            string text = base.gameObject.name;
            if (text.Contains("A"))
            {
                text = text.Replace('A', 'B');
            }
            else if (text.Contains("B"))
            {
                text = text.Replace('B', 'A');
            }
            GameObject gameObject = GameObject.Find(text);
            gameObject.GetComponent<SpriteRenderer>().sprite = spPointCheck;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            gameObject.GetComponent<Point>().enable = false;
            GameManager.REF.PointCheck();
            Sound.REF.Play("sndPointCheck");
        }
    }
}
