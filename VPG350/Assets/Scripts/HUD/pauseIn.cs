using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.FirstPerson;

public class pauseIn : MonoBehaviour {

    public GameObject pauseGame, resumeGame;
    public GameObject menuToggle;

    bool isPaused;

    staminaBar pauseStambar;

    //RigidbodyFirstPersonController person;
    BasicFirstPersonController person;

    public void onResumeGame()
    {
        resumeGame.SetActive(true);
    }

    void Start()
    {
        menuToggle.SetActive(false);

        //person = FindObjectOfType<RigidbodyFirstPersonController> ();
        person = FindObjectOfType<BasicFirstPersonController>();
    }


    void Update()
    {
		
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Escape))//when P or Esc is pressed
        {
			if (person == null)
				//person = FindObjectOfType<RigidbodyFirstPersonController> ();
                person = FindObjectOfType<BasicFirstPersonController>();

            //Pauses the game
            if (!isPaused)//if the game is running
            {
                //start_Button.gameObject.SetActive(true);//disabled - toggles visibily of the object
                menuToggle.SetActive(true);
                resumeGame.SetActive(true);

                person.lockPlayerControls();
                isPaused = true;
                //pauseStambar.;
            }
            //Resumes the Game after Pause key is pressed again
            else if (isPaused)//if the game is not running
            {
                menuToggle.SetActive(false);//enabled - toggles visibility of the object
                person.unlockPlayerControls();
                isPaused = false;
            }
        }
    }
}
