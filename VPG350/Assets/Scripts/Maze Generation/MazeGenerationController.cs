using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

    public Point MazeSize, MazeEntrance, MazeExit;
	public MazeCell[,] MAZE;
    MazeCell CurrentWalker;

    bool foundNewCell;
    bool MazeSolved;

    public float runModifier, openCellModifer;

    Action CompletedCallback;

	// Use this for initialization
	void Start () {
	
	}

    //Main outside call
	public void GenerateMaze(Point Size, bool randomiseEntranceAndExit, Action CompletedCallback){

        MazeSize = Size;
        if (Size.x == 0 || Size.y == 0)
        {
            Debug.Log("!!!No zero maze dimensions!!!");
            return;
        }

        if(Size.x <= 1 && Size.y <= 1 )
        {
            Debug.Log("!!!Maze needs more than 1 cell!!!");
            return;
        }

        MAZE = new MazeCell[Size.x,Size.y];

        this.CompletedCallback = CompletedCallback;

        for (int Y = 0; Y < Size.y; ++Y)
        {
            for (int X = 0; X < Size.x; ++X)
            {
                MAZE[X, Y] = new MazeCell();
                MAZE[X, Y].SetPositionInMaze(new Point(X, Y));
            }
        }//End for Y size

        int entrancePosX, entrancePosY;
        int exitX, exitY;

        entrancePosX = entrancePosY = exitX = exitY = 0;

        if (!randomiseEntranceAndExit)
        {
            entrancePosX = (int)Mathf.Floor(Size.x / 2);
            entrancePosY = 0;

            exitX = entrancePosX;
            exitY = Size.y - 1;
        }
        else
        {
            entrancePosX = UnityEngine.Random.Range(0, MazeSize.x);
            entrancePosY = UnityEngine.Random.Range(0, MazeSize.y);

            exitX = UnityEngine.Random.Range(0, MazeSize.x);
            exitY = UnityEngine.Random.Range(0, MazeSize.y);

            while(entrancePosX == exitX && entrancePosY == exitY)
            {
                exitX = UnityEngine.Random.Range(0, MazeSize.x);
                exitY = UnityEngine.Random.Range(0, MazeSize.y);
            }
        }

        MazeEntrance = new Point(entrancePosX, entrancePosY);
        MazeExit = new Point(exitX, exitY);

        CurrentWalker = MAZE[entrancePosX, entrancePosY];
        GenerateWallsInThisCell(ref CurrentWalker);

        if (CurrentWalker.IsDeadEnd())
            KnockdownAWall(ref CurrentWalker);

        CurrentWalker.SetAsEntrance();
        MAZE[exitX, exitY].SetAsExit();

        UpdateMazeData(CurrentWalker);

        //Loop until the maze is solved
        while (!MazeSolved)
        {
            DoMazeWalkingStep();
        }

        MazeSolved = false;
        CompletedCallback();
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

        if(Cell.OpenUp == MazeCell.WallType.open)
            if (!MAZE[cellX, cellY + 1].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        if (Cell.OpenRight == MazeCell.WallType.open)
            if (!MAZE[cellX + 1, cellY].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        if (Cell.OpenDown == MazeCell.WallType.open)
            if (!MAZE[cellX, cellY - 1].Initialized)
            {
                ++ConnectedUninitialisedNeighbors;
            }

        if (Cell.OpenLeft == MazeCell.WallType.open)
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

        if (Cell.OpenUp == MazeCell.WallType.notset)
            if (CellExists(Cell.GetUp()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenUp, ParseModifier(openCellModifer));
            else
                CurrentWalker.OpenUp = MazeCell.WallType.wall;

        if (Cell.OpenRight == MazeCell.WallType.notset)
            if (CellExists(Cell.GetRight()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenRight, ParseModifier(openCellModifer));
            else
                CurrentWalker.OpenRight = MazeCell.WallType.wall;

        if (Cell.OpenDown == MazeCell.WallType.notset)
            if (CellExists(Cell.GetDown()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenDown, ParseModifier(openCellModifer));
            else
                CurrentWalker.OpenDown = MazeCell.WallType.wall;

        if (Cell.OpenLeft == MazeCell.WallType.notset)
            if (CellExists(Cell.GetLeft()))
                CurrentWalker.RandomlyKnockdownThisWall(ref Cell.OpenLeft, ParseModifier(openCellModifer));
            else
                CurrentWalker.OpenLeft = MazeCell.WallType.wall;

        if (CurrentWalker.IsDeadEnd())
        {
            if (TrueFalseGeneration(ParseModifier(runModifier)))
            {
                //knock down a wall based on our maze runModifier
                KnockdownAWallForPathing(ref CurrentWalker);
            }
        }

        CurrentWalker.Initialized = true;
    }

    void KnockdownAWall(ref MazeCell Cell)
    {

        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        List<int> availableWalls = new List<int>();

        if (CellExists(Cell.GetUp()) && Cell.OpenUp != MazeCell.WallType.open)
        {
            availableWalls.Add(0);
        }

        if (CellExists(Cell.GetRight()) && Cell.OpenRight != MazeCell.WallType.open)
        {
            availableWalls.Add(1);
        }

        if (CellExists(Cell.GetDown()) && Cell.OpenDown != MazeCell.WallType.open)
        {
            availableWalls.Add(2);
        }

        if (CellExists(Cell.GetLeft()) && Cell.OpenLeft != MazeCell.WallType.open)
        {
            availableWalls.Add(3);
        }

        if(availableWalls.Count > 0)
        {
            int pickedWall = availableWalls[UnityEngine.Random.Range(0, availableWalls.Count)];

            if (pickedWall == 0) Cell.OpenUp = MazeCell.WallType.open;
            if (pickedWall == 1) Cell.OpenRight = MazeCell.WallType.open;
            if (pickedWall == 2) Cell.OpenDown = MazeCell.WallType.open;
            if (pickedWall == 3) Cell.OpenLeft = MazeCell.WallType.open;
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
            if (MAZE[cellX, cellY + 1].OpenDown == MazeCell.WallType.open)
                needsUp = 2;
            else if (MAZE[cellX, cellY + 1].OpenDown == MazeCell.WallType.wall)
                needsUp = 1;

        if (CellExists(Cell.GetRight()))
            if (MAZE[cellX + 1, cellY].OpenLeft == MazeCell.WallType.open)
                needsRight = 2;
            else if (MAZE[cellX + 1, cellY].OpenLeft == MazeCell.WallType.wall)
                needsRight = 1;

        if (CellExists(Cell.GetDown()))
            if (MAZE[cellX, cellY - 1].OpenUp == MazeCell.WallType.open)
                needsDown = 2;
            else if (MAZE[cellX, cellY - 1].OpenUp == MazeCell.WallType.wall)
                needsDown = 1;

        if (CellExists(Cell.GetLeft()))
            if (MAZE[cellX - 1, cellY].OpenRight == MazeCell.WallType.open)
                needsLeft = 2;
            else if (MAZE[cellX - 1, cellY].OpenRight == MazeCell.WallType.wall)
                needsLeft = 1;

        if (needsUp == 1)
            Cell.OpenUp = MazeCell.WallType.wall;
        else if (needsUp == 2)
            Cell.OpenUp = MazeCell.WallType.open;

        if (needsRight == 1)
            Cell.OpenRight = MazeCell.WallType.wall;
        else if(needsRight == 2)
            Cell.OpenRight = MazeCell.WallType.open;

        if (needsDown == 1)
            Cell.OpenDown = MazeCell.WallType.wall;
        else if(needsDown == 2)
            Cell.OpenDown = MazeCell.WallType.open;

        if (needsLeft == 1)
            Cell.OpenLeft = MazeCell.WallType.wall;
        else if(needsLeft == 2)
            Cell.OpenLeft = MazeCell.WallType.open;
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

    //Call to knockdown atleast one wall in any valid direction
    void KnockdownAWallForPathing(ref MazeCell Cell)
    {
        int cellX = Cell.PositionInMaze.x;
        int cellY = Cell.PositionInMaze.y;

        List<int> availableWalls = new List<int>();

        if (CellExists(Cell.GetUp()) && !MAZE[cellX, cellY + 1].Initialized && Cell.OpenUp != MazeCell.WallType.open)
        {
            availableWalls.Add(0);
        }

        if (CellExists(Cell.GetRight()) && !MAZE[cellX + 1, cellY].Initialized && Cell.OpenRight != MazeCell.WallType.open)
        {
            availableWalls.Add(1);
        }

        if (CellExists(Cell.GetDown()) && !MAZE[cellX, cellY - 1].Initialized && Cell.OpenDown != MazeCell.WallType.open)
        {
            availableWalls.Add(2);
        }

        if (CellExists(Cell.GetLeft()) && !MAZE[cellX - 1, cellY].Initialized && Cell.OpenLeft != MazeCell.WallType.open)
        {
            availableWalls.Add(3);
        }

        if (availableWalls.Count > 0)
        {
            int pickedWall = availableWalls[UnityEngine.Random.Range(0, availableWalls.Count)];

            if (pickedWall == 0) Cell.OpenUp = MazeCell.WallType.open;
            if (pickedWall == 1) Cell.OpenRight = MazeCell.WallType.open;
            if (pickedWall == 2) Cell.OpenDown = MazeCell.WallType.open;
            if (pickedWall == 3) Cell.OpenLeft = MazeCell.WallType.open;
        }
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

        if (Cell.OpenUp == MazeCell.WallType.open && CellExists(Cell.GetUp()) && !MAZE[Cell.GetUp().x, Cell.GetUp().y].Initialized)
        {
            Cell = MAZE[cellX, cellY + 1];
            return true;
        }
        else if (Cell.OpenRight == MazeCell.WallType.open && CellExists(Cell.GetRight()) && !MAZE[Cell.GetRight().x, Cell.GetRight().y].Initialized)
        {
            Cell = MAZE[cellX + 1, cellY];
            return true;
        }
        else if (Cell.OpenDown == MazeCell.WallType.open && CellExists(Cell.GetDown()) && !MAZE[Cell.GetDown().x, Cell.GetDown().y].Initialized)
        {
            Cell = MAZE[cellX, cellY - 1];
            return true;
        }
        else if (Cell.OpenLeft == MazeCell.WallType.open && CellExists(Cell.GetLeft()) && !MAZE[Cell.GetLeft().x, Cell.GetLeft().y].Initialized)
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

    public int ParseModifier(float modifier)
    {
        return (int)Math.Floor(modifier * 10);
    }

    public static bool TrueFalseGeneration(float trueChance)
    {
        //trueChance -= 4;

        float random = UnityEngine.Random.Range(0+1, 20+1); // Chance to be 0 -> 9

        return random + trueChance >= 10; // 1,2,3,4,5 true but 6,7,8,9,10 false
    }
}