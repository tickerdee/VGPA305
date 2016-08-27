using UnityEngine;
using System.Collections;

public class InGameUIController : MonoBehaviour {

	//This will sit outside all of our UI components and manage them

	//our pause menu
	public pauseIn PauseIn;
	//our hud
	public HUDController hudController;

	//One central place to see if UI is set to pause
	public bool isPaused;

	//Hopefully someone will set this
	//A refernce to our WorldController
	public WorldController worldController;

	// Use this for initialization
	void Start () {

		//We hope that it isn't null but if it is
			//Someone failed to plugin a refernce for us let's fall back to trying to find it
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
		isPaused = false;
	}

    public void quitToMain()
    {
        //PauseIn.
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);//Scene 1 is main menu
    }

	// Update is called once per frame
	void Update () {
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
