using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerScript : MonoBehaviour
{
    public int seconds, minutes;
    [SerializeField] Text timeText;
    public static timerScript instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        addToSeconds();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void addToSeconds()
    {
        seconds++;
        if (seconds > 59)
        {
            minutes++;
            seconds = 0;

        }
        timeText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        Invoke(nameof(addToSeconds), time: 1);
    }
    public void StopTImer()
    {
        CancelInvoke(nameof(addToSeconds));
    }
}
