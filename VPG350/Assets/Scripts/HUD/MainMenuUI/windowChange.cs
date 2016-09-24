using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class windowChange : MonoBehaviour {
	
	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public PauseMenuUIComponents pauseMenuComponents;
	public InGameUIController inGameUIController;

	public Toggle toggleButton;

	// Use this for initialization
	void Start () {

		if (mainComponents != null) 
		{
			mainComponents.windowMode.onValueChanged.AddListener (toggleW);
		}
		else
		{
			pauseMenuComponents.windowMode.onValueChanged.AddListener (toggleW);
		}
		toggleButton.isOn = true;
	}

	public void toggleW(bool winActive)
	{
			Screen.fullScreen = !winActive;
	}
}
