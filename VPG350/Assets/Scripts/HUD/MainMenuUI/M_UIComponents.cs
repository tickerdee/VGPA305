using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_UIComponents : MonoBehaviour
{
	//Need BreakAway bar for getting out of a struggle and a timer that only appears at that time.

    //The buttons used on the Main Menu
    public Button NewGame, Online, ExitOnline, Settings, ExitSettings, Tutorial, ExitTutorial, QuitGame;

	//Buttons that change the resolution of the game
	public Button b1080, b6x768, b1024, b800, b4x768, b600; 

    //The sound bar in the menu
    public Slider soundBar;

	//Toggles fullscreen/window mode
	public Toggle windowMode;

	//For changing languages
	public Button English, Spanish; 

//The Panel/GameObject that has these buttons
    public GameObject MainMenu_Panel;
}
