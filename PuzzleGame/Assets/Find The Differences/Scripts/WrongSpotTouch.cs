// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class WrongSpotTouch : MonoBehaviour
// {
//     private void OnMouseUp()
//     {
//         if (GameManager.REF.heartCount > 0)
//         {
//             GameManager.REF.heartParent.transform.GetChild(GameManager.REF.heartCount - 1).gameObject.GetComponent<Image>().enabled = false;
//             GameManager.REF.heartCount--;
//         }
//         if (GameManager.REF.heartCount <= 0)
//         {
//             Debug.Log("You Fail");
//         }
//         Debug.Log("Wrong Touch");
//     }
// }
