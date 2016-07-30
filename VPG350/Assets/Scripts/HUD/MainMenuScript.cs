using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public CustomButton TestActionButton;

	// Use this for initialization
	void Start () {
	    
		TestActionButton.OnClickCall = TextButtonOnClick;
	}

	void TextButtonOnClick(){
		
		return;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
