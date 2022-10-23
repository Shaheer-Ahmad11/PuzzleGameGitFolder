using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace=null;
    private Camera _camera;
    [SerializeField] private tileScript[] tiles;
    private int emptySpaceIndex = 8;
    public GameObject panel;
    public Animator animator;
    void Start()
    {
        
        _camera=Camera.main;
        shuffle();
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){

            Ray ray=_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit= Physics2D.Raycast(ray.origin,ray.direction);

            if(hit){
                if(Vector2.Distance(a:emptySpace.position, b:hit.transform.position)<2){
                    Vector2 lastEmptySpacePos=emptySpace.position;
                    tileScript thisTile=hit.transform.GetComponent<tileScript>();
                    
                    emptySpace.position=thisTile.targetPosition;
                    thisTile.targetPosition=lastEmptySpacePos;

                    int tileIndex=findIndex(thisTile);
                    tiles[emptySpaceIndex]=tiles[tileIndex];
                    tiles[tileIndex]=null;
                    emptySpaceIndex=tileIndex;

                }
            }
        } 
        int correctTiles=0;
        foreach (var a in tiles)
        {
            if(a!=null){

                if(a.inRightPlace){
                    correctTiles++;
                }
            }
            
        }
        if(correctTiles==tiles.Length-1){
        //    var a= GetComponent<timerScript>();
        //    a.StopTImer();
            timerScript.instance.StopTImer();
            StartCoroutine(waiting());
           

        }
    }

    void shuffle(){
        if(emptySpaceIndex !=8){
            var tileOn8LastPos=tiles[8].targetPosition;
            tiles[15].targetPosition=emptySpace.position;
            emptySpace.position=tileOn8LastPos;
            tiles[emptySpaceIndex]=tiles[8];
            tiles[8]=null;
            emptySpaceIndex=8;



        }


        int inversion;
        do{

             for(int i=0;i<=7;i++){
                var lastPos= tiles[i].targetPosition;
                int randomIndex = Random.Range(0,7);
                tiles[i].targetPosition=tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition=lastPos;
                var tile=tiles[i];
                tiles[i]=tiles[randomIndex];
                tiles[randomIndex]=tile;   
            }
            inversion=GetInversions();
            Debug.Log("shuffled");

        }while(inversion%2 !=0);

    }
    public int findIndex(tileScript ts){
        for(int i=0;i<tiles.Length;i++){
            if(tiles[i]!=null){
                if(tiles[i]==ts){
                    return i;
                }
            }
        }    
     return-1;

    }
    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0;i < tiles.Length;i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }
    IEnumerator waiting(){
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("new");
         yield return new WaitForSeconds(5f);
         panel.SetActive(true);
    }

}
