using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class windowChange : MonoBehaviour {
	
	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public Toggle toggleButton;

	// Use this for initialization
	void Start () {
		mainComponents.windowMode.onValueChanged.AddListener (toggleW);
		toggleButton.isOn = true;
	}

	public void toggleW(bool winActive)
	{
			Screen.fullScreen = !winActive;
	}
}
