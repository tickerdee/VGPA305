using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class onlineButton : MonoBehaviour {

	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public GameObject onlineScreen;
	public GameObject forExitOnline;

	// Use this for initialization
	void Start () 
	{
		mainComponents.Online.gameObject.SetActive (true);
		mainComponents.Online.onClick.AddListener (openOnlineMenu);
		onlineScreen.SetActive (false);
		forExitOnline.SetActive (false);
	}

	public void openOnlineMenu()
	{
		mainComponents.Online.gameObject.SetActive(true);
		onlineScreen.SetActive (true);
	}
}
