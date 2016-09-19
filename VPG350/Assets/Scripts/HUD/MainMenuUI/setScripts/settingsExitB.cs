using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class settingsExitB : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

    public GameObject forExitSettings;


    // Use this for initialization
    void Start () {
        mainComponents.ExitSettings.onClick.AddListener(exitSettings);
	}
	
    public void exitSettings()
    {
        mainComponents.ExitSettings.gameObject.SetActive(true);
        forExitSettings.SetActive(false);
    }
}
