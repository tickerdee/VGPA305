using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class resolutionScript : MonoBehaviour {
	
	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public PauseMenuUIComponents pauseMenuComponents;
	public InGameUIController inGameUIController;

	// Use this for initialization
	void Start () {

		if (mainComponents != null) 
		{
			//1920x1080
			mainComponents.b1080.onClick.AddListener (to1080);
			//1366x768
			mainComponents.b6x768.onClick.AddListener (to6x768);
			//1280x1024
			mainComponents.b1024.onClick.AddListener (to1024);
			//1280x800
			mainComponents.b800.onClick.AddListener (to800);
			//1024x768
			mainComponents.b4x768.onClick.AddListener (to4x768);
			//800x600
			mainComponents.b600.onClick.AddListener (to600);
		}
		else
		{
			//1920x1080
			pauseMenuComponents.b1080.onClick.AddListener (to1080);
			//1366x768
			pauseMenuComponents.b6x768.onClick.AddListener (to6x768);
			//1280x1024
			pauseMenuComponents.b1024.onClick.AddListener (to1024);
			//1280x800
			pauseMenuComponents.b800.onClick.AddListener (to800);
			//1024x768
			pauseMenuComponents.b4x768.onClick.AddListener (to4x768);
			//800x600
			pauseMenuComponents.b600.onClick.AddListener (to600);
		}
	}

	public void to1080()//1920x1080
	{
        Screen.SetResolution(1920, 1080, false); //MainController.windowInC);//the 3rd value is whether or not the window is full is screen
    }
	public void to6x768()//1366x768
	{
		Screen.SetResolution(1366, 768, false);
	}	
	public void to1024()//1280x1024
	{
		Screen.SetResolution(1280, 1024, false);
	}	
	public void to800()//1280x800
	{
		Screen.SetResolution(1280, 800, false);
	}	
	public void to4x768()//1024x768
	{
		Screen.SetResolution(1024, 768, false);
	}	
	public void to600()//800x600
	{
		Screen.SetResolution(800, 600, false);
	}
		
}
