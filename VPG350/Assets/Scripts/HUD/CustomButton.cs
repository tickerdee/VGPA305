using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CustomButton : Button {

	public Action OnClickCall;

	// Use this for initialization
	void Start () {
		onClick.AddListener (ONCLICK);
	}

	void ONCLICK(){
		OnClickCall ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
