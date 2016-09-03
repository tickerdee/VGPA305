using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class quit_MMenu_B : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

    // Use this for initialization
    void Start ()
    {
        mainComponents.QuitGame.onClick.AddListener(quitGame);
    }
	
    public void quitGame()
    {
        Application.Quit();
    }
}
