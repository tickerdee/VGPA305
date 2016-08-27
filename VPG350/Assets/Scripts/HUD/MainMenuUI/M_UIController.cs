using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_UIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void startNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);//Starts the Game

    }

    public void settings()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
