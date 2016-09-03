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
        mainComponents.Tutorial.onClick.AddListener(showTutorial);

		tutorialScreen.SetActive (false);
		forExitTutorial.SetActive (false);
    }
	
    public void  showTutorial()
    {
		mainComponents.Tutorial.gameObject.SetActive(true);
		tutorialScreen.SetActive (true);
    }


}
