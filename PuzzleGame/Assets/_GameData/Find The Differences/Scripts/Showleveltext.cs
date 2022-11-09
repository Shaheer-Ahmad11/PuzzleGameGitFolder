using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showleveltext : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshPro>().text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
