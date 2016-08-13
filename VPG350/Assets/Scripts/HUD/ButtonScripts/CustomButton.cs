using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CustomButton : Button {

	public Action OnClickCall;

	// Use this for initialization
	protected override void Start () {
		onClick.AddListener (ONCLICK);
	}

	void ONCLICK(){
		OnClickCall ();

        if(GameObject.Find("UI").GetComponent<pauseIn>().isPaused)
        {
            GameObject.Find("UI").GetComponent<pauseIn>().isPaused = false;
        }
	}

	// Update is called once per frame
	void Update () {
	
	}
}
