using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class settingsButton : MonoBehaviour {
    public M_UIComponents mainComponents;
    public M_UIController MainController;

    public GameObject settingsScreen;
    public GameObject forExitSettings;

    // Use this for initialization
    void Start () {
        mainComponents.Settings.gameObject.SetActive(true);
        mainComponents.Settings.onClick.AddListener(showSettings);

        settingsScreen.SetActive(false);
	}

    public void showSettings()
    {
        mainComponents.Settings.gameObject.SetActive(true);
        settingsScreen.SetActive(true);
    }
	
}
