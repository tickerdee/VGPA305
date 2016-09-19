using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour {

	//This will sit outside all of our UI components and manage them

	//our pause menu
	public pauseIn PauseIn;
	//our hud
	public HUDController hudController;

	//One central place to see if UI is set to pause
	public bool isPaused;

	//Hopefully someone will set this
	//A reference to our WorldController
	public WorldController worldController;

    //calling settings

    //opens settings
    public settingsButton settingsButtonInC;

    //closes the settings window
    public settingsExitB sExitButton;
    
    //slider for controlling sound
    public soundBarSlider soundBar;

    //toggle window/fullscreen
    public windowChange windowInC;

    //script controlling resolution
    public resolutionScript resolutionChange;

    //Changing language
    public englishLang changeEnglish;
    public spanishLang changeSpanish;

    // Use this for initialization
    void Start () {

		//We hope that it isn't null but if it is
			//Someone failed to plugin a reference for us let's fall back to trying to find it
		if(worldController == null){
			worldController = FindObjectOfType<WorldController>();
			//If we still can't find it someone fucked up in the scene
			if(worldController == null){
				Debug.Log("In Game Ui Controller could not find WorldController");
			}
		}

		if(PauseIn !=null){
			PauseIn.UIController = this;
		}
	}

	public void Pause(){
		PauseIn.ShowPauseMenu();
		hudController.gameObject.SetActive(false);
	}

	public void UnPause(){
		PauseIn.HidePauseMenu();
		hudController.gameObject.SetActive(true);
	}

    public void quitToMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);//Scene 1 is main menu. Scene 2 is the game
    }

    //inside the settings these will be called
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

    //changes the volume of the background music
    public void changeSound()
    {
        soundBar.changeVolume(0);
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
        changeEnglish.englishData();
    }

    //changes the game's language to spanish
    public void toSpanish()
    {
        changeSpanish.spanishData();
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))//when P or Esc is pressed
		{
			isPaused = !isPaused;//toggles true or false on key press
			if(isPaused){
				Pause();
			}else{
				UnPause();
			}
		}
	}


}
