﻿using UnityEngine;
using System.Collections;

public class ExamplePatrolAI : MonoBehaviour {
	
	enum GaurdState{
		NONE,
		patroling,
		chasing,
		struggling,
		staggered
	}

	Vector3 PatrolTarget;
	bool patrolTargetIsValid;
	GaurdState state;

	//Possibly a better idea. We inherit from NavMeshAgent?
	NavMeshAgent navAgent;
	NavMeshPath path;

	//This is just to visually see the target of the AI
	public GameObject targetPrefab;

	// Use this for initialization
	void Start () {
		//We don't want our guard to start doing anything until the maze is finished
		//So using WorldEvents we can "add listeners" to events
		WorldEvents.Event_MazeFinished += StartGaurd;
	}

	public void StartGaurd(){

		//Get the NavMeshAgent I hate doing this
		//but inheriting from NavMeshAgent is too bulky for my tests
		navAgent = GetComponent<NavMeshAgent>();
		if(navAgent == null){
			Debug.Log("WARNING!!! Patrol AI Needs a NavAgent Component");
		}

		//Set our state to patrolling for starters and get setup some patroling data
		state = GaurdState.patroling;
		GetNewPatrolToLocation();
		path = new NavMeshPath();
	}

	void GetNewPatrolToLocation(){
		//We can use the WorldObjectReferencer to find certain things. The WorldController is one of them
		WorldController wc = WorldObjectReference.GetInstance().GetObject<WorldController>();

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

		//Do some super nasty temporary state machine code
		switch(state){
		case GaurdState.NONE:
			break;

		case GaurdState.patroling:
			//Because we use Get on navAgent we must null check it
			if(navAgent != null){
				//Check navAgnet, is the path valid, is our patrol target valid ( do we need a new target )
				//We check if a path exist incase we are already moving to a target.
				//This causes another check needed That makes sure if we already have a path is the target of that path still valid
				if(!navAgent.hasPath && patrolTargetIsValid){
					navAgent.CalculatePath(PatrolTarget, path);
					navAgent.SetPath(path);
				}else if(!patrolTargetIsValid){
					//Call our method to get a new patrol target then wait for the next update to start moving
					GetNewPatrolToLocation();
				}
			}
			break;

		case GaurdState.chasing:
			break;
		case GaurdState.struggling:
			break;
		case GaurdState.staggered:
			break;
		}

		//If you not in patroling zero out navAgnet info
		//This looks terrible is there a better flow to call this?
		if(state != GaurdState.patroling){
			if(navAgent != null){
				navAgent.Stop();
				navAgent.ResetPath();
				patrolTargetIsValid = false;
			}
		}

	}
}