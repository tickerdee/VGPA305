using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_UIController : MonoBehaviour {

    //Button for opening menus followed by the buttons to close the same menus
    public nGameButton newGameButton;
	public onlineButton onlineButtonInC;
    public onlineExitB oExitButton;
    public settingsButton settingsButtonInC;
    public settingsExitB sExitButton;
    public tutorialButton tutorialButtonInC;
	public tutorialExitB tExitButton; 
    public quit_MMenu_B quitGameInC;

    //slider for controlling sound
    public soundBarSlider soundBar;

	//toggle window/fullscreen
	public windowChange windowInC;

	//script controlling resolution
	public resolutionScript resolutionChange;

	//Changing language
	public englishLang changeEnglish;
	public spanishLang changeSpanish;

    void Start()
    {
        changeEnglish.englishData();
    }

    //starts the game
    public void newGame()
    {
        newGameButton.startGame();
    }
    //opens online menu
	public void onlineOpen ()
	{
		onlineButtonInC.openOnlineMenu();
	}
    //closes online menu
	public void onlineClose()
	{
		oExitButton.exitOnline();
	}
    //opens settings menu
    public void settingsOpen()
    {
        settingsButtonInC.showSettings();
    }
    //closes settings menu
    public void settingsClose()
    {
        sExitButton.exitSettings();
    }
	//changes resolutions
	public void resolutionMenu()
	{
		resolutionChange.to1080();
	}
	//toggles game window
	public void windowToggle()
	{
		windowInC.toggleW(true);
	}

	//changes the game's language to english
	public void toEnglish()
	{
		changeEnglish.englishData ();
	}

	//changes the game's language to spanish
	public void toSpanish()
	{
		changeSpanish.spanishData ();
	}

	//changes the volume of the background music
	public void changeSound()
	{
		soundBar.changeVolume (0);
	}
    //opens tutorial
    public void tutorialOpen()
    {
		tutorialButtonInC.showTutorial();
    }
    //closes tutorial
	public void tutorialExit()
	{
		tExitButton.exitTutorial();
	}
    //Quits the game and closes the window
    public void quitGameC()
    {
        quitGameInC.quitGame();
    }
}
