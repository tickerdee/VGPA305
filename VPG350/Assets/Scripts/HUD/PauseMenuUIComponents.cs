using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuUIComponents : MonoBehaviour {

    //Pause Menu Buttons
    public Button Resume, Settings, ExitSettings, Quit;

    //Settings Menu Buttons
    //Buttons that change the resolution of the game
    public Button b1080, b6x768, b1024, b800, b4x768, b600;

    //The sound bar in the menu
    public Slider soundBar;

    //Toggles fullscreen/window mode
    public Toggle windowMode;

    //For changing languages
    public Button English, Spanish;


    public GameObject PauseMenu;
}
