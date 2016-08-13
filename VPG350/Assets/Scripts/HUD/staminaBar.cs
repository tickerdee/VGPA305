using UnityEngine;
using System.Collections;

/// <summary>
/// Francisco Medel
/// Stamina Bar
/// using local Scale to increase and decrease a stamina depending on user input
/// </summary>

public class staminaBar : MonoBehaviour {

    //variables for the stamina bar calculations
    public float currentStamina= 0;
    public float maxStamina = 100;
    public float calc_sta;
    public float staminaDepletion = 2;
    public float staminaRecovery = 1;
    //variables for the delay
    public float delayPerSecond = 1.0f;
    public float recDelay;
    //how much stamina regenerates
    bool staRegen;
    //for using the game object to rescale
    public GameObject barMod;
    public bool isPaused;

    public bool runEnabled;

    //pauseIn pausedGame;

    public bool barIsActive;
    private pauseIn checkForPause;
    

	// Use this for initialization
	void Start ()
    {
       staRegen = false;
       currentStamina  = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.GetComponent<pauseIn>().isPaused)//do not use .Find("nameOfObject") if both scripts are already in the same object
        {
            runEnabled = true;
            stamRegenFunc();
        }
        else if (gameObject.GetComponent<pauseIn>().isPaused)//returns nothing, stamina bar no longer works when paused
        {
            runEnabled = false;
        }
    }

    public void stamRegenFunc()
    {
        //converts current stamina into a 0 to 1 range or numbers so that the can be used to scale the Stamina bar
        calc_sta = currentStamina / maxStamina;
        //call setStaBar and places the calculations on X
        setStaBar(calc_sta);

        //conditions 
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && calc_sta > 0)
        {
            currentStamina -= staminaDepletion;
            //makes sure current stamina does not decrease lower than zero
            if (currentStamina <= 0)
            {
                currentStamina = 0.1f;
                //delayPerSecond = 2.5f;

            }

            else if (currentStamina > 0)
            {
                delayPerSecond = 1.0f;
            }

            staRegen = true;
        }

        //delay is always increasing and resetting anytime a user does an actions that decreases stamina
        recDelay += Time.deltaTime;

        //when reDelay reaches delayPerSecond checks if stamina is not full
        if (recDelay >= delayPerSecond)
        {
            if (staRegen && currentStamina < maxStamina)
            {
                currentStamina += staminaRecovery;
                //stops stamina regeneration when full and staRegen is false
                if (currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;
                    staRegen = false;
                }
            }
        }

        //resets the timer for the delay so that the stamina delay is consisten where ever is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (calc_sta < 1.0)
            {
                recDelay = 0;
            }
        }
    }

    //takes the calculated state of the green stamina bar and put it on the X scale(stBar)
    public void setStaBar(float stBar)
    {
        barMod.transform.localScale= new Vector3(stBar, barMod.transform.localScale.y, barMod.transform.localScale.z);
    }

    
}
