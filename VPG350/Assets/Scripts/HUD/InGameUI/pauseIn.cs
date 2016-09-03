using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pauseIn : MonoBehaviour {

	public PauseMenuUIComponents pauseComponents;
	public InGameUIController UIController;

    staminaBar pauseStambar;

    void Start()
    {
		pauseComponents.PauseMenu.SetActive(false);//menu not visible at start
        //Here, getting all buttons(components) from PauseMenuUIComponents
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
        if (UIController != null)
        {
            
        }
    }

	public void onQuit(){
        if(UIController != null)
        {
            UIController.quitToMain();
        }

	}

	public void ShowPauseMenu(){
		//start_Button.gameObject.SetActive(true);//disabled - toggles visibily of the object
		pauseComponents.PauseMenu.SetActive(true);//shows menu
		//resumeGame.SetActive(true);//shows resume button
		if(UIController != null && UIController.worldController !=null && UIController.worldController.player != null){
			UIController.worldController.player.lockPlayerControls();//prevents player from moving
		}
	}

	public void HidePauseMenu(){
		pauseComponents.PauseMenu.SetActive(false);//enabled - toggles visibility of the object
		if(UIController != null && UIController.worldController !=null && UIController.worldController.player != null){
			UIController.worldController.player.unlockPlayerControls();//enables player movement
		}
	}

    public void ShowSettings()
    {

    }
}
