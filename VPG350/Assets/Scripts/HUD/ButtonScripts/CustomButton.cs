using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CustomButton : Button {

	public Action OnClickCall;
    BasicFirstPersonController person;

    // Use this for initialization
    protected override void Start () {
        onClick.AddListener(ONCLICK);
        person = FindObjectOfType<BasicFirstPersonController>();
    }

	void ONCLICK(){
		OnClickCall ();

	}

	// Update is called once per frame
	void Update () {
	
        if(GameObject.Find("UI").GetComponent<pauseIn>().isPaused)
        {
            GameObject.Find("UI").GetComponent<pauseIn>().isPaused = false;
        }
	}
}
