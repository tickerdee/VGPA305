using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class englishLang : MonoBehaviour {

	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public PauseMenuUIComponents pauseMenuComponents;
	public InGameUIController inGameUIController;

	//text on the main menu
	public Text newgameText,onlineText, settingsText, tutorialText, tutorialInsideText, tutorialTextControls, quitText;
	//text on the online
	public Text connectText, hostText;
	//text on settings 
	public Text resolutionText, winToggleText, soundText, languageText, englishBText, spanishBText; 

	// Use this for initialization
	void Start () {
		if (mainComponents != null) 
		{
			mainComponents.English.onClick.AddListener (englishData);
		}
		else
		{
			pauseMenuComponents.English.onClick.AddListener (englishData);
		}
	}

	public void englishData()
	{
		newgameText.text = "New Game";
		onlineText.text = "Online";
		settingsText.text = "Settings";
		tutorialText.text = "Tutorial";
		tutorialInsideText.text = "HOW TO PLAY\n";
		quitText.text = "Quit";

		tutorialTextControls.text = "W - Move Forward\nS - Move BackWards\nA - Strafe Left\nD - Strafe Right\nShift + Move - Sprint\nP/Esc - Pause\n";

		connectText.text = "Connect";
		hostText.text = "Host";
		resolutionText.text = "Resolution";
		winToggleText.text = "Windowed";
		soundText.text = "Sound";
		languageText.text = "Language";
		englishBText.text = "English";
		spanishBText.text = "Español";


	}
}
