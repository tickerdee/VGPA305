using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class tutorialExitB : MonoBehaviour {

	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public GameObject forExitTutorial;

	void Start()
	{
		//mainComponents.ExitTutorial.gameObject.SetActive (true);
		mainComponents.ExitTutorial.onClick.AddListener (exitTutorial);
		//forExitTutorial.SetActive (true);
	}

	public void exitTutorial()
	{
		mainComponents.Tutorial.gameObject.SetActive(true);
		forExitTutorial.SetActive (false);
	}

}
