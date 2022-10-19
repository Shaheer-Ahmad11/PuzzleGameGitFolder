using UnityEngine;

public class BtnZoom : MonoBehaviour
{
    public GameObject magnifer;

    private bool enable;

    private GameObject btnBack;

    private void Start()
    {
        btnBack = GameObject.Find("btnBack");
        HideMagnifier();
    }

    private void ResetMagnifer()
    {
        Vector3 moveTo = enable ? new Vector3(0f, 0f, 0f) : new Vector3(0f, -11f, 0f);
        magnifer.GetComponent<Piece>().SetMoveTo(moveTo);
    }

    private void OnMouseUp()
    {
        base.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        Sound.REF.Play("sndClick");
        if (enable)
        {
            Invoke("HideMagnifier", 0.2f);
        }
        else
        {
            Invoke("ShowMagnifier", 0.2f);
        }
    }

    public void ShowMagnifier()
    {
        Zoom.show = true;
        base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        enable = true;
        base.transform.GetChild(0).gameObject.SetActive(enable);
        ResetMagnifer();
        // btnBack.SetActive(value: false);
    }

    public void HideMagnifier()
    {
        Zoom.show = false;
        base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        enable = false;
        base.transform.GetChild(0).gameObject.SetActive(enable);
        ResetMagnifer();
        // btnBack.SetActive(value: true);
    }
}
