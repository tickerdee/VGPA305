using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class settingsExitB : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

	public PauseMenuUIComponents pauseMenuComponents;
	public InGameUIController inGameUIController;

    public GameObject forExitSettings;


    // Use this for initialization
    void Start () {
		if (mainComponents != null) 
		{
			mainComponents.ExitSettings.onClick.AddListener (exitSettings);
		}
		else
		{
			pauseMenuComponents.ExitSettings.onClick.AddListener (exitSettings);
		}
	}
	
    public void exitSettings()
    {
		if (mainComponents != null) 
		{
			mainComponents.ExitSettings.gameObject.SetActive (true);
		}
		else
		{
			pauseMenuComponents.ExitSettings.gameObject.SetActive (true);
		}
        forExitSettings.SetActive(false);
    }
}
