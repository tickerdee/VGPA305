using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

	public staminaBar StaminaBar;
	public timerMin TimerMin;

	public GameObject qteHolder, qteBar;

	CharController player;

	void Start(){

	}

	public void getReferenceToplayer(){

		player = null;
		if (WorldObjectReference.GetInstance ().GetObject<WorldController> ()) {
			player = WorldObjectReference.GetInstance ().GetObject<WorldController> ().player;

			if (player == null)
				return;
		} else {
			return;
		}

	}

	public void ScaleQTEBar(){

		if (player == null) {
			getReferenceToplayer ();

			if (player == null) {
				qteHolder.SetActive(false);
				return;
			}
		}

		if (player.animState == CharController.AnimState.struggle) 
		{
			qteHolder.SetActive(true);
			float scale = player.getQTEPercent ();

			qteBar.transform.localScale = new Vector3 (player.getQTEPercent (), qteBar.transform.localScale.y, qteBar.transform.localScale.z);
		}
		else
		{
			qteHolder.SetActive(false);
		}

	}

	void Update(){

		ScaleQTEBar ();

	}

}
