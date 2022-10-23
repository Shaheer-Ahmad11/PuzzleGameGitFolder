using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    public Vector3 targetPosition;
    private Vector3 correctPosition;
    private SpriteRenderer _sprite;
    public int number;
    public bool  inRightPlace=false;
    

    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        transform.position = Vector3.Lerp(a: transform.position, b: targetPosition, t: 0.3f);
        if (targetPosition == correctPosition)
        {
            _sprite.color = Color.green;
            inRightPlace=true;
            
        }
       
        else
        {
            _sprite.color = Color.white;
            inRightPlace=false;

        }


    }
}
