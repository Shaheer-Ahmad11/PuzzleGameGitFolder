using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    public Vector3 targetPosition;
    private Vector3 correctPosition;
    private SpriteRenderer _sprite;
    public int number;



    void Awake()
    {
        targetPosition=transform.position;
        correctPosition=transform.position;
        _sprite=GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        transform.position=Vector3.Lerp(a: transform.position, b: targetPosition, t:0.05f);
        if(targetPosition==correctPosition){
            _sprite.color=Color.green;
        }
        else{
            _sprite.color=Color.white;

        }
        
    }
}
