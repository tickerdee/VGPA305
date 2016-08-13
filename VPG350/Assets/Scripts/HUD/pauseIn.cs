using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.FirstPerson;

public class pauseIn : MonoBehaviour {

    public GameObject pauseGame, resumeGame;
    public GameObject menuToggle;
    public bool isPaused;

    staminaBar pauseStambar;

    //RigidbodyFirstPersonController person;
    BasicFirstPersonController person;

    public void onResumeGame()
    {
        resumeGame.SetActive(true);
    }

    void Start()
    {
        menuToggle.SetActive(false);//menu not visible at start
        person = FindObjectOfType<BasicFirstPersonController>();
        isPaused = false;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))//when P or Esc is pressed
        {
            isPaused = !isPaused;//toggles true or false on key press

            if (person == null)
            {
                //person = FindObjectOfType<RigidbodyFirstPersonController> ();
                person = FindObjectOfType<BasicFirstPersonController>();//find the player controller
            }
            if (isPaused)//GamePaused
            {
                //start_Button.gameObject.SetActive(true);//disabled - toggles visibily of the object
                menuToggle.SetActive(true);//shows menu
                resumeGame.SetActive(true);//shows resume button
                person.lockPlayerControls();//prevents player from moving
                isPaused = true;
            }
            else if (!isPaused)//GamePlaying
            {
                menuToggle.SetActive(false);//enabled - toggles visibility of the object
                person.unlockPlayerControls();//enables player movement
                isPaused = false;
            }
        }
    }
}
