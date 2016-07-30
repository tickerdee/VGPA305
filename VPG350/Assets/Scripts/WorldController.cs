using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public int MazeSizeX = 20;
    public int MazeSizeY = 20;

    public MazeGenerationController MazeGenerator;

	// Use this for initialization
	void Start () {

        if (MazeGenerator == null)
            MazeGenerator = new MazeGenerationController();

        MazeGenerator.GenerateMaze(new Point(MazeSizeX, MazeSizeY), MazeFinished);
	}

    void MazeFinished()
    {
        
    }

    void Update()
    {

    }
}
