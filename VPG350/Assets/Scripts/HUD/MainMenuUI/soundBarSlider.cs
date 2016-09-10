using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class soundBarSlider : MonoBehaviour {

    public M_UIComponents mainComponents;
    public M_UIController MainController;

	AudioSource bgm_SoM;

    // Use this for initialization
    void Start () {
		
		bgm_SoM = (AudioSource)Instantiate (Resources.Load<AudioSource> ("Music/AS_bgm_SoM"));
		bgm_SoM.Play();
		changeVolume (0);

		mainComponents.soundBar.onValueChanged.AddListener (changeVolume);
	}

	//changes the volume of the bgm
	public void changeVolume(float value)
	{
		bgm_SoM.volume = value;
	}
}
