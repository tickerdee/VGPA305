using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct Node{
	public Point InMazePos;
	public Vector3 WorldLoc;
	//Node[] connections;

	public Node(Point MazePos, Vector3 WorldLoc){
		this.InMazePos = MazePos;
		this.WorldLoc = WorldLoc;
	}
};

public class NodeMap{

	/// <summary>
	/// Completly unorganized unlinked list of all our nodes in the maze
	/// </summary>
	ArrayList NodeList;

	public NodeMap(){}

	public void GenerateNodeMap(MazeCell[,] MAZE){

		//If the nodeList doesn't exist make it
		if(NodeList == null){
			NodeList = new ArrayList();
		}

		//Before we generate the NodeMap make sure our Nodelist is empty
		NodeList.Clear();

		//Loop over all cells in the maze
		foreach(MazeCell cell in MAZE){

			//If the cell is empty don't register it in the nodeMap
			if(cell.Empty){
				continue;
			}

			Vector3 worldLoc = new Vector3(cell.PositionInMaze.x * 3, 0, cell.PositionInMaze.y * 3);

			//Node newNode = new Node(cell.PositionInMaze, worldLoc);

			NodeList.Add(new Node(cell.PositionInMaze, worldLoc));
		}
	}//End Generate NodeMap

	public Vector3 GetRandomNodePosition(){
		if(NodeList.Count > 0){
			return ((Node)NodeList[Random.Range(0, NodeList.Count - 1)]).WorldLoc;
		}else{
			Debug.Log("Node List is empty");
			return Vector3.zero;
		}
	}

	public Vector3 GetRandomNodeAtLeastDistanceAway(Vector3 relativeLocation, float distanceAway)
	{
		float recentDistance = 0.0f;
		Vector3 outLocation = Vector3.zero;

		if(NodeList.Count > 0){

			int searchCount = 0;
			while(recentDistance < distanceAway && searchCount < 10){
				
				outLocation = ((Node)NodeList[Random.Range(0, NodeList.Count - 1)]).WorldLoc;
				recentDistance = (relativeLocation - outLocation).magnitude;
				searchCount++;
			}

			return outLocation;
		}else{
			Debug.Log("Node List is empty");
			return Vector3.zero;
		}
	}
}