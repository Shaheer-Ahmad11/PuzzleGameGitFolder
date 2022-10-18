using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                GameObject temp = hit.collider.gameObject;
                Debug.Log(temp.name);
            }
        }
    }
}