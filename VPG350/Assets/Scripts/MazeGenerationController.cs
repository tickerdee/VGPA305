using UnityEngine;
using System.Collections;
using System;

//We declare outside the class so that other classes can use this type
/// <summary>
/// Small struct that holds two ints labeled x and y
/// </summary>
public struct Point
{
    public int x, y;
    public Point(int x, int y) { this.x = x; this.y = y; }
};

public class MazeGenerationController : MonoBehaviour {

	/// <summary>
	/// This will be used to generate a maze
	/// 	We will use this data to make the maze our players will run through
	/// 
	/// Requirements of the maze:
	/// 	Have a solution
	/// 	Have different sized rooms
	/// 	Fill the entirety of the map (Atleast until we cull dead ends)
	/// 	Be able to remove dead ends
	/// 	Adjustable parameters:
	/// 		Size of the maze
	/// 		Run of the maze (Length of continued hallways)
	/// 		Amount of Dead ends
	/// 		Room sizes
	/// </summary>

    enum WallType
    {
        notset = 0,
        wall = 1,
        open = 2
    }

	/// <summary>
	/// Cell of the maze
	/// </summary>
	struct MazeCell{
        public Point PositionInMaze;

        public bool Initialized, Unchangeable;

        public bool IsEntrance, IsExit;

        public WallType OpenUp, OpenRight, OpenDown, OpenLeft;

        public void SetAsEntrance() { IsEntrance = Initialized = Unchangeable = true; }
        public void SetAsExit() { IsExit = Initialized = true; }

        public bool IsDeadEnd() {

            int ConnectionCount = 0;

            ConnectionCount += (OpenUp    == WallType.open ? 1 : 0);
            ConnectionCount += (OpenRight == WallType.open ? 1 : 0);
            ConnectionCount += (OpenDown  == WallType.open ? 1 : 0);
            ConnectionCount += (OpenLeft  == WallType.open ? 1 : 0);

            return ConnectionCount <= 1;
        }

        public void RandomlyKnockdownThisWall(ref WallType wall)
        {
            if (wall == WallType.notset)
                wall = (TrueFalseGeneration(0) ? WallType.open : WallType.wall);
        }

        public String Output()
        {
            int passageValue = 0;

            passageValue += (OpenUp == WallType.open ? 1 : 0);
            passageValue += (OpenRight == WallType.open ? 2 : 0);
            passageValue += (OpenDown == WallType.open ? 4 : 0);
            passageValue += (OpenLeft == WallType.open ? 8 : 0);

            return passageValue + "";
        }

        public Point GetUp() { return new Point(PositionInMaze.x, PositionInMaze.y + 1); }
        public Point GetRight() { return new Point(PositionInMaze.x + 1, PositionInMaze.y); }
        public Point GetDown() { return new Point(PositionInMaze.x, PositionInMaze.y - 1); }
        public Point GetLeft() { return new Point(PositionInMaze.x - 1, PositionInMaze.y); }
	};

    Point MazeSize;
	MazeCell[,] MAZE;
    MazeCell CurrentWalker;

    Action CompletedCallback;

	// Use this for initialization
	void Start () {
	
	}

	public void GenerateMaze(Point Size, Action CompletedCallback){

        MazeSize = Size;

        MAZE = new MazeCell[Size.x,Size.y];

        this.CompletedCallback = CompletedCallback;

        int entrancePosX = (int)Mathf.Floor(Size.x/2);

        for (int Y = 0; Y < Size.y; ++Y)
        {
            for (int X = 0; X < Size.x; ++X)
            {
                MAZE[X, Y].PositionInMaze = new Point(X, Y);
            }
        }//End for Y size

        CurrentWalker = MAZE[entrancePosX, 0];
        GenerateWallsInThisCell(ref CurrentWalker);

        if (CurrentWalker.IsDeadEnd())
            KnockdownAWall(ref CurrentWalker);

        CurrentWalker.SetAsEntrance();
        MAZE[entrancePosX, Size.y - 1].SetAsExit();

        UpdateMazeData(CurrentWalker);
	}

    /// <summary>
    /// This is for creating the maze.
    ///     Should only be called to fill in empty spaces of the maze
    /// </summary>
    void DoMazeWalkingStep()
    {

        if(!CurrentWalker.IsDeadEnd())
        {
            //Not dead end
            bool foundNewCell = GetNewAdjacentPathedCell(ref CurrentWalker);

            if (!foundNewCell)
                Debug.Log("Error in maze generation expected not dead end");

            GenerateWallsInThisCell(ref CurrentWalker);
            UpdateMazeData(CurrentWalker);
        }
        else 
        { 
            //if is dead end
            //PrintOutMaze();

            return;

        }//End if CurrentWalked is deadEnd


    }

    void GenerateWallsInThisCell(ref MazeCell Cell)
    {

        if (Cell.Unchangeable)
        {
            //We can't edit an unchageable cell
            return;
        }

        //This will make walls where we have to have them to match already existing cells
        CheckCellNeighborWalls(ref CurrentWalker);

        if (Cell.OpenUp == WallType.notset)
            if (CellExists(Cell.GetUp()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenUp);
            else
                CurrentWalker.OpenUp = WallType.wall;

        if (Cell.OpenRight == WallType.notset)
            if (CellExists(Cell.GetRight()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenRight);
            else
                CurrentWalker.OpenRight = WallType.wall;

        if (Cell.OpenDown == WallType.notset)
            if (CellExists(Cell.GetDown()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenDown);
            else
                CurrentWalker.OpenDown = WallType.wall;

        if (Cell.OpenLeft == WallType.notset)
            if (CellExists(Cell.GetLeft()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenLeft);
            else
                CurrentWalker.OpenLeft = WallType.wall;

        CurrentWalker.Initialized = true;
    }

    void KnockdownAWall(ref MazeCell Cell)
    {

        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        if (CellExists(Cell.GetUp()) && Cell.OpenUp != WallType.open)
        {
            Cell.OpenUp = WallType.open;
            return;
        }

        if (CellExists(Cell.GetRight()) && Cell.OpenRight != WallType.open)
        {
            Cell.OpenRight = WallType.open;
            return;
        }

        if (CellExists(Cell.GetDown()) && Cell.OpenDown != WallType.open)
        {
            Cell.OpenDown = WallType.open;
            return;
        }

        if (CellExists(Cell.GetLeft()) && Cell.OpenLeft != WallType.open)
        {
            Cell.OpenLeft = WallType.open;
            return;
        }
    }

	// Update is called once per frame
	public void Update () {
        DoMazeWalkingStep();
	}



    //---------------- Helper / Information Gathering Functions
    void CheckCellNeighborWalls(ref MazeCell Cell)
    {
        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        bool needsUp, needsRight, needsDown, needsLeft;

        needsUp = needsRight = needsDown = needsLeft = false;

        if (CellExists(Cell.GetUp()))
            needsUp = MAZE[cellX, cellY + 1].OpenDown == WallType.open;

        if (CellExists(Cell.GetRight()))
            needsRight = MAZE[cellX + 1, cellY].OpenLeft == WallType.open;

        if (CellExists(Cell.GetDown()))
            needsDown = MAZE[cellX, cellY - 1].OpenUp == WallType.open;

        if (CellExists(Cell.GetLeft()))
            needsLeft = MAZE[cellX - 1, cellY].OpenRight == WallType.open;

        if (needsUp)
            Cell.OpenUp = WallType.open;

        if (needsRight)
            Cell.OpenRight = WallType.open;

        if (needsDown)
            Cell.OpenDown = WallType.open;

        if (needsLeft)
            Cell.OpenLeft = WallType.open;
    }

    bool CellExists(Point pos)
    {
        if (pos.x >= 0 && pos.x < MazeSize.x)
            if (pos.y >= 0 && pos.y < MazeSize.y)
                return true;
        return false;
    }

    bool GetNewAdjacentPathedCell(ref MazeCell Cell)
    {

        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        if (Cell.OpenUp == WallType.open && CellExists(Cell.GetUp()) && !MAZE[Cell.GetUp().x, Cell.GetUp().y].Initialized)
        {
            Cell = MAZE[cellX, cellY + 1];
            return true;
        }
        else if (Cell.OpenRight == WallType.open && CellExists(Cell.GetRight()) && !MAZE[Cell.GetRight().x, Cell.GetRight().y].Initialized)
        {
            Cell = MAZE[cellX + 1, cellY];
            return true;
        }
        else if (Cell.OpenDown == WallType.open && CellExists(Cell.GetDown()) && !MAZE[Cell.GetDown().x, Cell.GetDown().y].Initialized)
        {
            Cell = MAZE[cellX, cellY - 1];
            return true;
        }
        else if (Cell.OpenLeft == WallType.open && CellExists(Cell.GetLeft()) && !MAZE[Cell.GetLeft().x, Cell.GetLeft().y].Initialized)
        {
            Cell = MAZE[cellX - 1, cellY];
            return true;
        }

        return false;
    }

    void PrintOutMaze()
    {

        String OutString = "Maze:\n";

        for (int Y = 0; Y < MazeSize.y; ++Y)
        {
            for (int X = 0; X < MazeSize.x; ++X)
            {
                OutString += MAZE[X, Y].Output() + " ";
            }
            OutString += '\n';
        }//End for Y size

        Debug.Log(OutString);
    }

    void UpdateMazeData(MazeCell Cell)
    {
        MAZE[Cell.PositionInMaze.x, Cell.PositionInMaze.y] = Cell;
    }

    static bool TrueFalseGeneration(float trueChance)
    {
        float random = UnityEngine.Random.Range(0, 10); // Chance to be 0 -> 9

        return random + trueChance > 5; // 0,1,2,3,4 true but 5,6,7,8,9 false
    }
}
