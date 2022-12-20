using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColliderScript : MonoBehaviour
{
    public Vector2 startposition;
    public Vector2 targetposition;
    public bool inRightPlace = false;
    private void Start()
    {
        startposition = transform.position;
    }
    private void Update()
    {
        targetposition = transform.position;
        if (targetposition == startposition)
        {
            // GetComponent<SpriteRenderer>().color = Color.green;
            inRightPlace = true;
        }
        else
        {
            // GetComponent<SpriteRenderer>().color = Color.white;
            inRightPlace = false;
        }
    }


}
