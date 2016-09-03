using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_UIController : MonoBehaviour {
    public nGameButton newGameButton;
	public onlineButton onlineButtonInC;
    public onlineExitB oExitButton;
    public settingsButton settingsButtonInC;
    public settingsExitB sExitButton;
    public tutorialButton tutorialButtonInC;
	public tutorialExitB tExitButton; 
    public quit_MMenu_B quitGameInC;

    public void newGame()
    {
        newGameButton.startGame();
    }

	public void onlineOpen ()
	{
		onlineButtonInC.openOnlineMenu();
	}

	public void onlineClose()
	{
		oExitButton.exitOnline();
	}

    public void settingsOpen()
    {
        settingsButtonInC.showSettings();
    }

    public void settingsClose()
    {
        sExitButton.exitSettings();
    }

    public void tutorialOpen()
    {
		tutorialButtonInC.showTutorial();
    }

	public void tutorialExit()
	{
		tExitButton.exitTutorial();
	}


    public void quitGameC()
    {
        quitGameInC.quitGame();
    }


}
