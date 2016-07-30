using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timerMin : MonoBehaviour {

    public Text number_text;
    public float timerOnSec, timerOnMin;

    void Start ()
    {
        //InvokeRepeating("timerOnMin", 1.0f, 60.0f);
    }


    void Update()
    {
            timerOnMin = (int)(Time.time / 60f);
            timerOnSec = (int)(Time.time % 60f);
            number_text.text = timerOnMin.ToString("00") + ":" + timerOnSec.ToString("00");
    }
}