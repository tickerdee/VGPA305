using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public MazeGenerationController MazeGenerator;

	// Use this for initialization
	void Start () {

        if (MazeGenerator == null)
            MazeGenerator = new MazeGenerationController();

        MazeGenerator.GenerateMaze(new Point(6, 6), MazeFinished);
	}

    void MazeFinished()
    {
        
    }

    void Update()
    {
        MazeGenerator.Update();
    }
}
