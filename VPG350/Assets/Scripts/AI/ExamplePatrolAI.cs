using UnityEngine;
using System.Collections;

public class ExamplePatrolAI : MonoBehaviour {
	
	enum GaurdState{
		NONE,
		patroling,
		chasing,
		struggling,
		staggered
	}

	public CharController.AnimState animState;

	Vector3 PatrolTarget;
	bool patrolTargetIsValid;
	public bool showPatrolTarget = false;
	GaurdState state, oldState;

	//Possibly a better idea. We inherit from NavMeshAgent?
	NavMeshAgent navAgent;
	NavMeshPath path;

	//This is just to visually see the target of the AI
	public GameObject targetPrefab;

	public float distanceFromNode = 0.95f;

	public LOS myLOS;

	// Use this for initialization
	void Start () {

		//We don't want our guard to start doing anything until the maze is finished
		//So using WorldEvents we can "add listeners" to events
		WorldEvents.Event_MazeFinished += StartGuard;

		animState = CharController.AnimState.idle;

		if (myLOS == null) {
			Debug.Log ("GUARD HAS NO LOS");
		} 
		else 
		{
			myLOS.Event_LOSTargetSeen += LOSTargetSeen;
			myLOS.Event_LOSTargetLost += LOSTargetLost;
		}
	}

	public void LOSTargetSeen()
	{
		state = GaurdState.chasing;
	}

	public void LOSTargetLost()
	{
		state = GaurdState.patroling;
	}

	public void StartGuard(){
		
		//Get the NavMeshAgent I hate doing this
		//but inheriting from NavMeshAgent is too bulky for my tests
		navAgent = GetComponent<NavMeshAgent>();
		if(navAgent == null){
			Debug.Log("WARNING!!! Patrol AI Needs a NavAgent Component");
		}

		navAgent.enabled = true;

		//Set our state to patrolling for starters and get setup some patroling data
		state = GaurdState.patroling;
		path = new NavMeshPath();
		GetNewPatrolToLocation();
	}

	void GetNewPatrolToLocation(){
		//We can use the WorldObjectReferencer to find certain things. The WorldController is one of them
		WorldController wc = WorldObjectReference.GetInstance().GetObject<WorldController>();

		CleanUpPath();

		//Make sure we actually found the WorldController
		//And that the nodeMap has been constructed
		if(wc != null && wc.nodeMap != null){
			PatrolTarget = wc.nodeMap.GetRandomNodePosition();
			patrolTargetIsValid = true;

			//This is just to visually see the target of the AI
			if(targetPrefab != null){
				targetPrefab.transform.position = PatrolTarget;
			}
		}else{
			//If the WorldController wasn't found of the NodeMap isn't valid we set our target to invalid
			patrolTargetIsValid = false;
		}
	}

	// Update is called once per frame
	void Update () {

		CheckShowGuardTarget();

		//Do not put code between this and the switch case
		if (oldState != state) 
		{
			OnStateChanged (state);
		}

		//Do some super nasty temporary state machine code
		switch(state){
			case GaurdState.NONE:
				break;

			case GaurdState.patroling:

				animState = CharController.AnimState.walk;

				//Because we use Get on navAgent we must null check it
				if(navAgent != null && navAgent.isOnNavMesh){
					//Check navAgnet, is the path valid, is our patrol target valid ( do we need a new target )
					//We check if a path exist incase we are already moving to a target.
					//This causes another check needed That makes sure if we already have a path is the target of that path still valid
					if(!navAgent.hasPath && patrolTargetIsValid){
						path = new NavMeshPath();

						bool result = navAgent.CalculatePath(PatrolTarget, path);

						result = navAgent.SetPath(path);

					}else if(!patrolTargetIsValid){
						//Call our method to get a new patrol target then wait for the next update to start moving
						GetNewPatrolToLocation();
					}

					if (patrolTargetIsValid && Vector3.Distance (PatrolTarget, transform.position) <= distanceFromNode) {
						
						GetNewPatrolToLocation ();
					
					} else {
						//Debug.Log (Vector2.Distance (PatrolTarget, transform.position) + " : " + distanceFromNode);
					}
						
				}
				break;

			case GaurdState.chasing:
				animState = CharController.AnimState.run;
				myLOS.ChasePlayer ();
				Debug.Log ("Chasing");
				break;
			case GaurdState.struggling:
				animState = CharController.AnimState.struggle;
				break;
			case GaurdState.staggered:
				animState = CharController.AnimState.struggleLose;
				break;
		}

		oldState = state;
	}

	void DrawPath()
	{
		if(state == GaurdState.patroling){

			animState = CharController.AnimState.walk;

			//Because we use Get on navAgent we must null check it
			if(navAgent != null && navAgent.isOnNavMesh){
				//Check navAgnet, is the path valid, is our patrol target valid ( do we need a new target )
				//We check if a path exist incase we are already moving to a target.
				//This causes another check needed That makes sure if we already have a path is the target of that path still valid
				if(navAgent.hasPath && patrolTargetIsValid){


					for(int i=0; i < navAgent.path.corners.Length; ++i){
						
						if(i == 0)
						{
							Debug.DrawLine(transform.position, navAgent.path.corners[i+1], Color.blue, 0.1f);
						}
						else
						{
							if(i+1 < navAgent.path.corners.Length)
							{
								Debug.DrawLine(navAgent.path.corners[i], navAgent.path.corners[i+1], Color.blue, 0.1f);
							}
						}
					}//End for corners
				}
			}
		}
	}

	void CheckShowGuardTarget()
	{

		if(showPatrolTarget)
		{
			if(patrolTargetIsValid)
			{
				targetPrefab.transform.position = PatrolTarget;
				DrawPath();
			}else
			{
				targetPrefab.transform.position = Vector3.zero;
			}
		}
		else
		{
			
		}

		targetPrefab.SetActive(showPatrolTarget);
	}

	void OnStateChanged(GaurdState state)
	{
		if (state != GaurdState.patroling) 
		{
			//Do any fix for switching from patrolling
			CleanUpPath ();
		} 
		else if (state != GaurdState.chasing) 
		{
			//Do any fix for switching from chasing
		}
	}

	void CleanUpPath()
	{
		//If you not in patroling zero out navAgnet info
		//This looks terrible is there a better flow to call this?
		if (navAgent != null && navAgent.hasPath) {
			navAgent.Stop ();
			navAgent.ResetPath ();
			patrolTargetIsValid = false;
		}
	}
}
