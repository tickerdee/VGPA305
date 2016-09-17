using UnityEngine;
using System.Collections;

public class LOS : MonoBehaviour {

	public GameObject target;
	public float speed = 0.1f;

	public GameObject guard;

	public bool seen;

	public Rigidbody rb;

	public float thrust = 5;

	public event LOSTargetSeen Event_LOSTargetSeen;
	public event LOSTargetLost Event_LOSTargetLost;

	public delegate void LOSTargetLost();
	public delegate void LOSTargetSeen();

	// Use this for initialization
	void Start () 
	{
		seen = false;



	}

	public void FireLOSTargetLost()
	{

		if (Event_LOSTargetLost != null) {
			Event_LOSTargetLost ();
		}
	}

	public void FireLOSTargetSeen()
	{

		if (Event_LOSTargetSeen != null) {
			Event_LOSTargetSeen ();
		}
	}

	public void OnTriggerEnter(Collider collision)
	{
		//if (collision.gameObject.tag == "Player") 
		if(collision.gameObject.GetComponent<CharController>() != null)
		{
			target = collision.gameObject;
			seen = true;
			Debug.Log ("Los Saw");

			FireLOSTargetSeen ();
		}

	}

	public void OnTriggerExit(Collider collision)
	{
		if(collision.gameObject.GetComponent<CharController>() != null)
		{
			seen = false;
			Debug.Log ("Los Lost");
			FireLOSTargetLost ();
		}


	}


	
	// Update is called once per frame
	void Update () 
	{
		
	

	}

	public void ChasePlayer ()
	{
		Vector3 targetPostition = new Vector3 (target.transform.position.x, guard.transform.position.y, target.transform.position.z);
		guard.transform.LookAt (targetPostition);


		rb.AddRelativeForce (Vector3.forward * thrust);

		if (rb.velocity.magnitude > 0.5f) {
			rb.velocity.Normalize ();
			rb.velocity *= 0.5f;
		}
	}




}
