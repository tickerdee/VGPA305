using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class onlineExitB : MonoBehaviour {
	
	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public GameObject forExitOnline;

	// Use this for initialization
	void Start () {
		mainComponents.ExitOnline.onClick.AddListener(exitOnline);
	
	}

	public void exitOnline()
	{
		mainComponents.Online.gameObject.SetActive (true);
		forExitOnline.SetActive(false);
	}

}
