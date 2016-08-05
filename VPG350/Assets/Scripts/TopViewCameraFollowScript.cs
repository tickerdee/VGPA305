using UnityEngine;
using System.Collections;

public class TopViewCameraFollowScript : MonoBehaviour {

    public GameObject folowObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (folowObject != null)
            transform.position = new Vector3(folowObject.transform.position.x, 15, folowObject.transform.position.z);
    }
}
