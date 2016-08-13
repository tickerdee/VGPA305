using UnityEngine;
using System.Collections;

public class LOS : MonoBehaviour {

	public Transform target;
	public float speed = 0.1f;

	public GameObject guard;

	public bool seen;

	public Rigidbody rb;

	public float thrust = 5;


	// Use this for initialization
	void Start () 
	{
		seen = false;
		//rb = GetComponent<Rigidbody> ();
	}

	public void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			Debug.Log (seen);
			seen = true;
		}

	}

	public void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			seen = false;
		}


	}


	
	// Update is called once per frame
	void Update () 
	{
		//float step = speed * Time.deltaTime;
		/*
		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;
		*/



		if (seen == true) 
		{
			Vector3 targetPostition = new Vector3( target.position.x, 
				guard.transform.position.y, 
				target.position.z ) ;
			guard.transform.LookAt( targetPostition ) ;

			rb.AddRelativeForce (Vector3.forward * thrust);


			//guard.transform.position = Vector3.MoveTowards (transform.position, target.position, speed);

		}

	}


}
