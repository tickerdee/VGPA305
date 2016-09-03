using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tutorialButton : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

    public GameObject tutorialScreen;
	public GameObject forExitTutorial;

    // Use this for initialization
    void Start () {
		mainComponents.Tutorial.gameObject.SetActive(true);
		//tutorialScreen.SetActive (true);
		//mainComponents.ExitTutorial.gameObject.SetActive (false);
        mainComponents.Tutorial.onClick.AddListener(showTutorial);
		//mainComponents.ExitTutorial.onClick.AddListener (forExitTutorial2);
		tutorialScreen.SetActive (false);
		forExitTutorial.SetActive (false);
    }
	
    public void  showTutorial()
    {
		mainComponents.Tutorial.gameObject.SetActive(true);
		//mainComponents.forExitTutorial.gameObject.SetActive (true);
		tutorialScreen.SetActive (true);
    }

	public void  forExitTutorial2()
	{
	}
}
