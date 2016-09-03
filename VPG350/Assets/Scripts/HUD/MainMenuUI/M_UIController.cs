using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_UIController : MonoBehaviour {
    public nGameButton newGameButton;
    public tutorialButton tutorialButtonInC;
    public quit_MMenu_B quitGameInC;

    public void newGame()
    {
        newGameButton.startGame();
    }

    public void tutorialOpen()
    {
       tutorialButtonInC.showTutorial();
    }


    public void quitGameC()
    {
        quitGameInC.quitGame();
    }


}
