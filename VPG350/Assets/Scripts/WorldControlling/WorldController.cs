using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public int MazeSizeX = 20;
    public int MazeSizeY = 20;

    public MazeGenerationController MazeGenerator;
    public GameObject MazeHolder;

    public GameObject Exit, Entrance;

    public bool randomizeEntranceAndExit;

    public float mazeRunModifier, OpenWallModifier;

    public bool CalledMazeGeneration;

	public WorldEvents worldEvents;

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

		if(Entrance != null){
			Vector3 playerSpawnLoc = Entrance.transform.position + new Vector3(0,1,0);
			FindObjectOfType<BasicFirstPersonController>().transform.position = playerSpawnLoc;
		}

		if(Exit != null){
			Vector3 guardSpawnLoc = Exit.transform.position + new Vector3(0,0.5f,0);

			var tempGaurd = FindObjectOfType<GuardAI>();
			if(tempGaurd != null){
				tempGaurd.transform.position = guardSpawnLoc;
			}
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

    void DeleteAllChildren(Transform transform)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
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
    }
}
