using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpotDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnMouseDown()
    {
        Debug.Log("again touched");
        // gameObject.GetComponent<SpriteRenderer>().enabled = false;
        EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
