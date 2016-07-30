using UnityEngine;
using System.Collections;
//using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class pauseIn : MonoBehaviour {

    public GameObject pauseGame, resumeGame;
    public GameObject menuToggle;

	RigidbodyFirstPersonController person;

    public void onResumeGame()
    {
        Time.timeScale = 1;
        resumeGame.SetActive(true);
    }

    void Start()
    {
        menuToggle.SetActive(false);

		person = FindObjectOfType<RigidbodyFirstPersonController> ();
    }


    void Update()
    {
		
        if (Input.GetKeyDown(KeyCode.P))//when P is pressed
        {
			if (person == null)
				person = FindObjectOfType<RigidbodyFirstPersonController> ();
			
            //Pauses the game
            if (Time.timeScale == 1)//if the game is running
            {
                //start_Button.gameObject.SetActive(true);//disabled - toggles visibily of the object
                Time.timeScale = 0;
                menuToggle.SetActive(true);
                resumeGame.SetActive(true);
                //Cursor.visible = true;
                //Cursor.active = true;
				person.mouseLook.SetCursorLock(false);
            }
            //Resumes the Game
            else if (Time.timeScale == 0 || resumeGame)//if the game is not running
            {
                menuToggle.SetActive(false);//enabled - toggles visibility of the object
                Time.timeScale = 1;
                // inPause = false;
               // menuToggle; 
				person.mouseLook.SetCursorLock(true);
                
            }

        }
    }
}
