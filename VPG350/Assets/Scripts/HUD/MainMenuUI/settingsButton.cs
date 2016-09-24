using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class settingsButton : MonoBehaviour {
    public M_UIComponents mainComponents;
    public M_UIController MainController;

	public PauseMenuUIComponents pauseMenuComponents;
	public InGameUIController inGameUIController;

    public GameObject settingsScreen;
    public GameObject forExitSettings;

    // Use this for initialization
    void Start () {

		if (mainComponents != null) {
			
			mainComponents.Settings.gameObject.SetActive (true);
			mainComponents.Settings.onClick.AddListener (showSettings);
		} 
		else 
		{
			
			pauseMenuComponents.Settings.gameObject.SetActive (true);
			pauseMenuComponents.Settings.onClick.AddListener (showSettings);
		}

        settingsScreen.SetActive(false);
	}

    public void showSettings()
    {
		if (mainComponents != null) 
		{
			mainComponents.Settings.gameObject.SetActive (true);
		}
		else
		{

		}
        settingsScreen.SetActive(true);
    }
	
}
