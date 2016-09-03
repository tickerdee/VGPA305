using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class nGameButton : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

    void Start()
    {
        mainComponents.NewGame.onClick.AddListener(startGame);
    }

    //public 
    public void startGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
