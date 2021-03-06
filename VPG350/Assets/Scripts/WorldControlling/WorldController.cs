﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

    public int MazeSizeX = 20;
    public int MazeSizeY = 20;

    public MazeGenerationController MazeGenerator;
	public NodeMap nodeMap;
    public GameObject MazeHolder;

    public GameObject Exit, Entrance;

    public bool randomizeEntranceAndExit;

    public float mazeRunModifier, OpenWallModifier;

    public bool CalledMazeGeneration;

	public WorldEvents worldEvents;

	public GameObject PlayerPrefab;
	public Vector3 DefaultLocation;
	public CharController player;

	public GameObject GuardPrefab;
	public IList<ExamplePatrolAI> Guards;
	public int numberOfGuards = 1;

	// Use this for initialization
	void Start () {

        if (MazeGenerator == null)
            MazeGenerator = gameObject.AddComponent<MazeGenerationController>();

		worldEvents = GetComponent<WorldEvents>();

		//Use our WorldObjectReferncer to register our WordlController
		WorldObjectReference.GetInstance().AddObject(this);

		if(MazeHolder == null){
			Debug.Log("World Controller has no reference to a MazeHolder. No maze Generated");
		}

		Guards = new List<ExamplePatrolAI>();

		//If we don't have a player spawn one
		if(PlayerPrefab != null && player == null){
			player = ((GameObject)Instantiate(PlayerPrefab, DefaultLocation, Quaternion.identity)).GetComponent<CharController>();

			//Debug.Log ("Found Prefab = true");
		}else{
			//else if all else fails try to find the object in the scene
			player = FindObjectOfType<CharController>();
		}
	}

    void MazeFinished()
    {

        for (int Y=0; Y < MazeGenerator.MazeSize.y; ++Y)
        {
            for(int X=0; X < MazeGenerator.MazeSize.x; ++X)
            {
				//If the Cell is marked empty then just continue on to next loop
				if (MazeGenerator.MAZE[X, Y].Empty){
                    continue;
				}

                string path = @"Enviroment/Prefabs/Maze Rooms/";
                string openDesc = "";

                path += (MazeGenerator.MAZE[X, Y].OpenUp == MazeCell.WallType.open ? "Up " : "");
                path += (MazeGenerator.MAZE[X, Y].OpenRight == MazeCell.WallType.open ? "Right " : "");
                path += (MazeGenerator.MAZE[X, Y].OpenDown == MazeCell.WallType.open ? "Down " : "");
                path += (MazeGenerator.MAZE[X, Y].OpenLeft == MazeCell.WallType.open ? "Left " : "");

                path += @"Open/";

                openDesc += (MazeGenerator.MAZE[X, Y].OpenUp == MazeCell.WallType.open ? "U" : "");
                openDesc += (MazeGenerator.MAZE[X, Y].OpenRight == MazeCell.WallType.open ? "R" : "");
                openDesc += (MazeGenerator.MAZE[X, Y].OpenDown == MazeCell.WallType.open ? "D" : "");
                openDesc += (MazeGenerator.MAZE[X, Y].OpenLeft == MazeCell.WallType.open ? "L" : "");

                openDesc = path + "Cell " + openDesc;

                Object temp = Resources.Load(openDesc);

                if (temp == null)
                {
                    Debug.Log(openDesc);
                    continue;
                }

                GameObject Cell = SpawnAndPlaceMazeCell(Resources.Load(openDesc), X, Y);

                if (MazeGenerator.MAZE[X, Y].IsExit)
                {
                    Exit = SpawnEndTargetObject(Resources.Load("Enviroment/Prefabs/End Object"), X, Y);
				}else if(MazeGenerator.MAZE[X, Y].IsEntrance){
					Entrance = Cell;
				}
            }
        }//End for maze y
		
		nodeMap = new NodeMap();
		nodeMap.GenerateNodeMap(MazeGenerator.MAZE);

		if(Entrance != null && player != null){
			Vector3 playerSpawnLoc = Entrance.transform.position + new Vector3(0, player.transform.localScale.y,0);

			if(player != null){
				player.transform.position = playerSpawnLoc;
			}
		}

		if(Exit != null){
			//Do things that rely on exit node
		}

		for(int i=0; i < numberOfGuards; ++i)
		{
			SpawnGaurd(Entrance.transform.position);
		}

		if(worldEvents != null)
			worldEvents.CallMazeFinished();
    }//End Maze Finished

    GameObject SpawnAndPlaceMazeCell(Object MazePrefab, int MazePositionx, int MazePositiony)
    {
        GameObject MazeObject = (GameObject)Instantiate(MazePrefab, new Vector3(MazePositionx * 3, 0, MazePositiony * 3), Quaternion.identity);

		MazeObject.transform.parent = MazeHolder.transform;

		return MazeObject;
    }

    GameObject SpawnEndTargetObject(Object EndObject, int MazePositionx, int MazePositiony)
    {
        EndObject = (GameObject)Instantiate(EndObject, new Vector3(MazePositionx * 3, 0, MazePositiony * 3), Quaternion.identity);

        ((GameObject)EndObject).transform.parent = MazeHolder.transform;

        return ((GameObject)EndObject);
    }

    Vector3 GetWorldPositionForCell(Point MazePosition)
    {
        return new Vector3(MazePosition.x * 3, 0, MazePosition.y * 3);
    }

	public void SpawnGaurd(Vector3 playerSpawnLocation)
	{
		Vector3 guardSpawnLoc = nodeMap.GetRandomNodeAtLeastDistanceAway(playerSpawnLocation, 10) + new Vector3(0,3.0f,0);

		GameObject tempGuard = null;

		if(GuardPrefab != null)
		{
			tempGuard = (GameObject)Instantiate(GuardPrefab, new Vector3 (0, 100, 0), Quaternion.identity);

			if (tempGuard != null) {
				tempGuard.transform.position = guardSpawnLoc;
				ExamplePatrolAI temp = tempGuard.GetComponent<ExamplePatrolAI>();
				Guards.Add(temp);
				tempGuard.GetComponent<ExamplePatrolAI> ().StartGuard ();
			}
		} else {
			Debug.Log ("WorldController no Gaurd found");
		}
	}

    void DeleteAllChildren(Transform transform)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

	void DoStruggleCheck()
	{
		if (player.animState != CharController.AnimState.struggle) 
		{

			foreach(ExamplePatrolAI guard in Guards)
			{
				if ((player.transform.position - guard.transform.position).magnitude < 1) 
				{
					if (guard.state != ExamplePatrolAI.GaurdState.struggling && guard.state != ExamplePatrolAI.GaurdState.staggered) 
					{
						guard.state = ExamplePatrolAI.GaurdState.struggling;
						player.SetAnimState (CharController.AnimState.struggle);
						player.ActivateQTE ();
						break;
					}
				}
			}
		}
	}

	public void QTEFinished(bool playerWon)
	{

		if (!playerWon) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene (0);
			return;
		}

		foreach(ExamplePatrolAI guard in Guards)
		{
			if (guard.state == ExamplePatrolAI.GaurdState.struggling) 
			{
				if (playerWon) 
				{
					guard.SetStaggered (true);
				} 
				else 
				{
					guard.SetStaggered (false);
				}
			}
		}
	}

	public void CheckFinishedMaze()
	{
		if ((player.transform.position - Exit.transform.position).magnitude <= 1) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (0);
		}
	}

    void Update()
    {
		if (!CalledMazeGeneration && MazeHolder != null)
        {
            DeleteAllChildren(MazeHolder.transform);
            MazeGenerator.MAZE = null;
            MazeGenerator.runModifier = mazeRunModifier;
            MazeGenerator.openCellModifer = OpenWallModifier;
            MazeGenerator.GenerateMaze(new Point(MazeSizeX, MazeSizeY), randomizeEntranceAndExit, MazeFinished);
            CalledMazeGeneration = true;
        }
		else
		{
			DoStruggleCheck ();
			CheckFinishedMaze ();
		}
    }

	void OnApplicationQuit(){
		WorldObjectReference.GetInstance().Destroy();
	}
}
