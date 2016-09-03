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
<<<<<<< HEAD
		mainComponents.Tutorial.gameObject.SetActive(true);
		//tutorialScreen.SetActive (true);
		//mainComponents.ExitTutorial.gameObject.SetActive (false);
        mainComponents.Tutorial.onClick.AddListener(showTutorial);
		//mainComponents.ExitTutorial.onClick.AddListener (forExitTutorial2);
		tutorialScreen.SetActive (false);
		forExitTutorial.SetActive (false);
=======
        //mainComponents.Tutorial.SetActive(false);
        //mainComponents.Tutorial.onClick.AddListener(showTutorial);
>>>>>>> 5e71657b29e6abde05d86f2e5110454008517c90
    }
	
    public void  showTutorial()
    {
<<<<<<< HEAD
		mainComponents.Tutorial.gameObject.SetActive(true);
		//mainComponents.forExitTutorial.gameObject.SetActive (true);
		tutorialScreen.SetActive (true);
=======
        //mainComponents.Tutorial.SetActive(true);
>>>>>>> 5e71657b29e6abde05d86f2e5110454008517c90
    }

	public void  forExitTutorial2()
	{
	}
}
