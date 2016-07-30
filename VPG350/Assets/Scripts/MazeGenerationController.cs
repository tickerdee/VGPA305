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

        public bool ISNULL, Initialized, Unchangeable;

        public bool IsEntrance, IsExit;

        public WallType OpenUp, OpenRight, OpenDown, OpenLeft;

        //We must initialise everything in a struct constructor
        public MazeCell(bool dumbyValue) {
            IsEntrance = IsExit = Initialized = Unchangeable = false;
            ISNULL = true;
            PositionInMaze = new Point();
            OpenUp = OpenRight = OpenDown = OpenLeft = WallType.notset;
        }

        public void SetAsEntrance() { IsEntrance = Initialized = Unchangeable = true; }
        public void SetAsExit() { IsExit = true; }

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

            return (passageValue < 10 ? "0" : "") + passageValue;
        }

        public void SetPositionInMaze(Point Position) { PositionInMaze = Position; ISNULL = false; }

        public void KnockdownAWallToConnectTo(Point connectTo)
        {
            if (PositionInMaze.y + 1 == connectTo.y)
                OpenUp = WallType.open;

            if (PositionInMaze.x + 1 == connectTo.x)
                OpenRight = WallType.open;

            if (PositionInMaze.y - 1 == connectTo.y)
                OpenDown = WallType.open;

            if (PositionInMaze.x - 1 == connectTo.x)
                OpenLeft = WallType.open;
        }

        public Point GetUp() { return new Point(PositionInMaze.x, PositionInMaze.y + 1); }
        public Point GetRight() { return new Point(PositionInMaze.x + 1, PositionInMaze.y); }
        public Point GetDown() { return new Point(PositionInMaze.x, PositionInMaze.y - 1); }
        public Point GetLeft() { return new Point(PositionInMaze.x - 1, PositionInMaze.y); }
	};

    Point MazeSize;
	MazeCell[,] MAZE;
    MazeCell CurrentWalker;

    bool foundNewCell;
    bool MazeSolved;

    Action CompletedCallback;

	// Use this for initialization
	void Start () {
	
	}

    //Main outside call
	public void GenerateMaze(Point Size, Action CompletedCallback){

        MazeSize = Size;
        if (Size.x == 0 || Size.y == 0)
        {
            Debug.Log("!!!No zero maze dimensions!!!");
            return;
        }

        MAZE = new MazeCell[Size.x,Size.y];

        this.CompletedCallback = CompletedCallback;

        int entrancePosX = (int)Mathf.Floor(Size.x/2);

        for (int Y = 0; Y < Size.y; ++Y)
        {
            for (int X = 0; X < Size.x; ++X)
            {
                MAZE[X, Y].SetPositionInMaze(new Point(X, Y));
            }
        }//End for Y size

        CurrentWalker = MAZE[entrancePosX, 0];
        GenerateWallsInThisCell(ref CurrentWalker);

        if (CurrentWalker.IsDeadEnd())
            KnockdownAWall(ref CurrentWalker);

        CurrentWalker.SetAsEntrance();
        MAZE[entrancePosX, Size.y - 1].SetAsExit();

        UpdateMazeData(CurrentWalker);

        //Loop until the maze is solved
        while (!MazeSolved)
        {
            DoMazeWalkingStep();
        }
	}

    /// <summary>
    /// This is for creating the maze.
    ///     Should only be called to fill in empty spaces of the maze
    /// </summary>
    void DoMazeWalkingStep()
    {

        if (!CurrentWalker.IsDeadEnd() && !ThisPathComplete(ref CurrentWalker))
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
            CurrentWalker = FindANewPathStart();

            //If it is NULL we couldn't find a new cell to start a path
            if (!CurrentWalker.ISNULL)
            {
                //Initialise this cell
                //knock down a neighbor wall
                KnockdownANeighborWall(ref CurrentWalker);
                GenerateWallsInThisCell(ref CurrentWalker);
                UpdateMazeData(CurrentWalker);
            }
            else
            {
                //Maze Generation End
                PrintOutMaze();
                MazeSolved = true;
            }
            

            return;

        }//End if CurrentWalked is deadEnd


    }

    MazeCell FindANewPathStart()
    {

        MazeCell outCell = new MazeCell();

        ArrayList inactiveNeighboredCellArray = new ArrayList();

        for (int Y = 0; Y < MazeSize.y; ++Y)
        {
            for(int X=0; X < MazeSize.x; ++X)
            {
                if (!MAZE[X, Y].Initialized)
                    if (HasInitialisedNeighbor(ref MAZE[X, Y]))
                        inactiveNeighboredCellArray.Add(MAZE[X, Y]);
            }
        }

        if(inactiveNeighboredCellArray.Count > 0)
            outCell = (MazeCell)inactiveNeighboredCellArray[(int)UnityEngine.Random.Range(0, inactiveNeighboredCellArray.Count)];
        else
            outCell.ISNULL = true;

        return outCell;
    }

    bool ThisPathComplete(ref MazeCell Cell)
    {
        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        int ConnectedUninitialisedNeighbors = 0;

        if(Cell.OpenUp == WallType.open)
            if (!MAZE[cellX, cellY + 1].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        if (Cell.OpenRight == WallType.open)
            if (!MAZE[cellX + 1, cellY].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        if (Cell.OpenDown == WallType.open)
            if (!MAZE[cellX, cellY - 1].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        if (Cell.OpenLeft == WallType.open)
            if (!MAZE[cellX - 1, cellY].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        return (ConnectedUninitialisedNeighbors == 0);
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
        
	}



    //---------------- Helper / Information Gathering Functions
    void CheckCellNeighborWalls(ref MazeCell Cell)
    {
        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        int needsUp, needsRight, needsDown, needsLeft;

        needsUp = needsRight = needsDown = needsLeft = 0;

        if (CellExists(Cell.GetUp()))
            if (MAZE[cellX, cellY + 1].OpenDown == WallType.open)
                needsUp = 2;
            else if (MAZE[cellX, cellY + 1].OpenDown == WallType.wall)
                needsUp = 1;

        if (CellExists(Cell.GetRight()))
            if (MAZE[cellX + 1, cellY].OpenLeft == WallType.open)
                needsRight = 2;
            else if (MAZE[cellX + 1, cellY].OpenLeft == WallType.wall)
                needsRight = 1;

        if (CellExists(Cell.GetDown()))
            if (MAZE[cellX, cellY - 1].OpenUp == WallType.open)
                needsDown = 2;
            else if (MAZE[cellX, cellY - 1].OpenUp == WallType.wall)
                needsDown = 1;

        if (CellExists(Cell.GetLeft()))
            if (MAZE[cellX - 1, cellY].OpenRight == WallType.open)
                needsLeft = 2;
            else if (MAZE[cellX - 1, cellY].OpenRight == WallType.wall)
                needsLeft = 1;

        if (needsUp == 1)
            Cell.OpenUp = WallType.wall;
        else if (needsUp == 2)
            Cell.OpenUp = WallType.open;

        if (needsRight == 1)
            Cell.OpenRight = WallType.wall;
        else if(needsRight == 2)
            Cell.OpenRight = WallType.open;

        if (needsDown == 1)
            Cell.OpenDown = WallType.wall;
        else if(needsDown == 2)
            Cell.OpenDown = WallType.open;

        if (needsLeft == 1)
            Cell.OpenLeft = WallType.wall;
        else if(needsLeft == 2)
            Cell.OpenLeft = WallType.open;
    }

    void KnockdownANeighborWall(ref MazeCell Cell)
    {

        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        ArrayList mazeLocs = new ArrayList();

        if (CellExists(Cell.GetUp()) && MAZE[cellX, cellY + 1].Initialized)
            mazeLocs.Add(new Point(cellX, cellY + 1));

        if (CellExists(Cell.GetRight()) && MAZE[cellX + 1, cellY].Initialized)
            mazeLocs.Add(new Point(cellX + 1, cellY));

        if (CellExists(Cell.GetDown()) && MAZE[cellX, cellY - 1].Initialized)
            mazeLocs.Add(new Point(cellX, cellY - 1));

        if (CellExists(Cell.GetLeft()) && MAZE[cellX - 1, cellY].Initialized)
            mazeLocs.Add(new Point(cellX - 1, cellY));

        

        if (mazeLocs.Count > 0)
        {
            Point temp = (Point)mazeLocs[(int)UnityEngine.Random.Range(0, mazeLocs.Count)];
            MAZE[temp.x, temp.y].KnockdownAWallToConnectTo(Cell.PositionInMaze);
        }
        else
            Debug.Log("Didn't find initialised neighbor to knock a wall down");
    }

    bool HasInitialisedNeighbor(ref MazeCell Cell)
    {
        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;
        
        if (CellExists(Cell.GetUp()) && MAZE[cellX, cellY + 1].Initialized)
            return true;

        if (CellExists(Cell.GetRight()) && MAZE[cellX + 1, cellY].Initialized)
            return true;

        if (CellExists(Cell.GetDown()) && MAZE[cellX, cellY - 1].Initialized)
            return true;

        if (CellExists(Cell.GetLeft()) && MAZE[cellX - 1, cellY].Initialized)
            return true;

        return false;
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

        for (int Y = MazeSize.y - 1; Y >= 0; --Y)
        {
            for (int X = 0; X < MazeSize.x; ++X)
            {
                OutString += MAZE[X, Y].Output() + " ";

                if (MAZE[X, Y].Output().CompareTo("00") == 0 && MAZE[X, Y].Initialized)
                    Debug.Log("00 in final Maze that is initialised");

                if (MAZE[X, Y].Output().CompareTo("00") == 0 && !MAZE[X, Y].Initialized)
                    Debug.Log("00 in final Maze that is NOT initialised");
            }
            OutString += '\n';
        }//End for Y size

        OutString += "\n\n" + GetCellNumberGuide();

        Debug.Log(OutString);
    }

    String GetCellNumberGuide()
    {

        String outString = "";

        outString += "1 : OpenUp" + "\n";
        outString += "2 : OpenRight" + "\n";
        outString += "4 : OpenDown" + "\n";
        outString += "8 : OpenLeft" + "\n";

        return outString;
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