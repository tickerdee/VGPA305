﻿using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public int MazeSizeX = 20;
    public int MazeSizeY = 20;

    public MazeGenerationController MazeGenerator;
    public GameObject MazeHolder;

    public bool randomizeEntranceAndExit;

    public float mazeRunModifier, OpenWallModifier;

    public bool CalledMazeGeneration;

	// Use this for initialization
	void Start () {

        if (MazeGenerator == null)
            MazeGenerator = gameObject.AddComponent<MazeGenerationController>();
	}

    void MazeFinished()
    {

        for (int Y=0; Y < MazeGenerator.MazeSize.y; ++Y)
        {
            for(int X=0; X < MazeGenerator.MazeSize.x; ++X)
            {
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

                SpawnAndPlaceMazeCell(Resources.Load(openDesc), X, Y);

                if (MazeGenerator.MAZE[X, Y].IsExit)
                    SpawnEndTargetObject(Resources.Load("Enviroment/Prefabs/End Object"), X, Y);
            }
        }//End for maze y

        Vector3 playerSpawnLoc = GetWorldPositionForCell(new Point(MazeGenerator.MazeEntrance.x, MazeGenerator.MazeEntrance.y)) + new Vector3(0,1,0);

        FindObjectOfType<BasicFirstPersonController>().transform.position = playerSpawnLoc;

    }//End Maze Finished

    void SpawnAndPlaceMazeCell(Object MazePrefab, int MazePositionx, int MazePositiony)
    {
        MazePrefab = (GameObject)Instantiate(MazePrefab, new Vector3(MazePositionx * 3, 0, MazePositiony * 3), Quaternion.identity);

        ((GameObject)MazePrefab).transform.parent = MazeHolder.transform;
    }

    void SpawnEndTargetObject(Object EndObject, int MazePositionx, int MazePositiony)
    {
        EndObject = (GameObject)Instantiate(EndObject, new Vector3(MazePositionx * 3, 0, MazePositiony * 3), Quaternion.identity);

        ((GameObject)EndObject).transform.parent = MazeHolder.transform;
    }

    Vector3 GetWorldPositionForCell(Point MazePosition)
    {
        return new Vector3(MazePosition.x * 3, 0, MazePosition.y * 3);
    }

    void DeleteAllChildren(Transform transform)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        if (!CalledMazeGeneration)
        {
            DeleteAllChildren(MazeHolder.transform);
            MazeGenerator.MAZE = null;
            MazeGenerator.runModifier = mazeRunModifier;
            MazeGenerator.openCellModifer = OpenWallModifier;
            MazeGenerator.GenerateMaze(new Point(MazeSizeX, MazeSizeY), randomizeEntranceAndExit, MazeFinished);
            CalledMazeGeneration = true;
        }
    }
}
