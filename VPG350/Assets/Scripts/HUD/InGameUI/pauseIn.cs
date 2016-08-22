using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.FirstPerson;

public class pauseIn : MonoBehaviour {

	public PauseMenuUIComponents pauseComponents;
	public InGameUIController UIController;

    staminaBar pauseStambar;

    void Start()
    {
		pauseComponents.PauseMenu.SetActive(false);//menu not visible at start

		pauseComponents.Resume.onClick.AddListener(onResumeGame);
		pauseComponents.Settings.onClick.AddListener(onSettings);
		pauseComponents.Quit.onClick.AddListener(onQuit);
    }

	public void onResumeGame()
	{
		if(UIController != null){
			UIController.UnPause();
		}
	}

	public void onSettings(){

	}

	public void onQuit(){

	}

	public void ShowPauseMenu(){
		//start_Button.gameObject.SetActive(true);//disabled - toggles visibily of the object
		pauseComponents.PauseMenu.SetActive(true);//shows menu
		//resumeGame.SetActive(true);//shows resume button
		if(UIController != null && UIController.worldController !=null && UIController.worldController.player != null){
			UIController.worldController.player.lockPlayerControls();//prevents player from moving
		}
	}

	public void HidePauseMeunu(){
		pauseComponents.PauseMenu.SetActive(false);//enabled - toggles visibility of the object
		if(UIController != null && UIController.worldController !=null && UIController.worldController.player != null){
			UIController.worldController.player.unlockPlayerControls();//enables player movement
		}
	}

    void Update()
    {
		
    }
}
