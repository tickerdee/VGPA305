using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LOS : MonoBehaviour {

	public GameObject target;
	public float speed = 0.1f;

	public GameObject guard;
	public GameObject Eye;

	public bool seen;

	public Rigidbody rb;

	public float thrust = 5;

	public float sightDistance = 10.0f, guaranteedSensingDistance = 1;
	public float guardSightFOV = 30.0f;

	public bool drawDebugLines;

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

			//TestSensedCharacter(collision.gameObject);
		}
	}

	public void TestSensedCharacter(GameObject gameObject)
	{
		if(!CheckSightOfPlayer(gameObject))
		{
			FireLOSTargetLost();
			return;
		}

		target = gameObject;
		seen = true;
		Debug.Log ("Los Saw");

		FireLOSTargetSeen ();
	}

	public void DoConePlayerSensing()
	{
		CharController player = WorldObjectReference.GetInstance().GetObject<WorldController>().player;

		float characterDistance = (player.transform.position - guard.gameObject.transform.position).magnitude;

		if(characterDistance < sightDistance)
		{
			if(characterDistance < guaranteedSensingDistance)
			{
				TestSensedCharacter(player.gameObject);
			}
			else
			{
				Vector3 relativePos = (player.transform.position - guard.gameObject.transform.position);
				relativePos.y = 0;//We don't care about height
				relativePos.Normalize();//We will use this as a direction so length is not needed

				float degsToSee = Vector3.Angle(relativePos, guard.gameObject.transform.forward);

				if(degsToSee <= guardSightFOV)
				{
					TestSensedCharacter(player.gameObject);
				}
			}
		}
	}

	public bool CheckSightOfPlayer(GameObject sensedObject)
	{
		bool hasClearSightOfPlayer = false;

		Vector3 colliderPosition = sensedObject.transform.position;
		Vector3 eyePositon = transform.position;
		if(Eye != null)
		{
			eyePositon = Eye.transform.position;
		}

		eyePositon = new Vector3(eyePositon.x, 0.5f, eyePositon.z);

		Vector3  playerDirection = (colliderPosition - eyePositon).normalized;

		if(drawDebugLines)
		{
			Debug.DrawLine(eyePositon - new Vector3(0, 0.2f, 0), eyePositon + new Vector3(0, 0.2f, 0), Color.green, 0.1f);
			Debug.DrawLine(eyePositon, eyePositon + playerDirection * 10, Color.red, 0.1f);
		}

		Ray lookRay = new Ray(eyePositon, playerDirection);
		RaycastHit[] hits = Physics.RaycastAll(lookRay, 10);
		if(hits.Length > 0)
		{

			ArrayList orderedHits = new ArrayList();

			for(int i=0; i < hits.Length; ++i)
			{
				float distance = hits[i].distance;

				bool didInsert = false;
				for(int j=0; j < orderedHits.Count; ++j)
				{
					if(hits[i].distance < ((RaycastHit)orderedHits[j]).distance)
					{
						orderedHits.Insert(j, hits[i]);
						didInsert = true;
						break;
					}
				}

				if(!didInsert)
				{
					orderedHits.Add(hits[i]);
				}
			}

			foreach(RaycastHit sortedHits in orderedHits)
			{
				if(sortedHits.collider.gameObject.GetComponent<CharController>() != null)
				{
					hasClearSightOfPlayer = true;
					break;
				}
				else
				{
					hasClearSightOfPlayer = false;
					break;
				}
			}
		}

		return hasClearSightOfPlayer;
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
		
		DoConePlayerSensing();

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
