using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tutorialButton : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

    // Use this for initialization
    void Start () {
        mainComponents.Tutorial.onClick.AddListener(showTutorial);
    }
	
    public void  showTutorial()
    {
        
    }

}
