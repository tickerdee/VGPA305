using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Cell of the maze
/// </summary>
public class MazeCell {

    public enum WallType
    {
        notset = 0,
        wall = 1,
        open = 2
    }

    public Point PositionInMaze;

    public bool ISNULL, Initialized, Unchangeable;

    public bool IsEntrance, IsExit;

    public WallType OpenUp, OpenRight, OpenDown, OpenLeft;

    public MazeCell()
    {
        IsEntrance = IsExit = Initialized = Unchangeable = false;
        ISNULL = true;
        PositionInMaze = new Point();
        OpenUp = OpenRight = OpenDown = OpenLeft = WallType.notset;
    }

    public void SetAsEntrance() { IsEntrance = Initialized = Unchangeable = true; }
    public void SetAsExit() { IsExit = true; }

    public bool IsDeadEnd()
    {

        int ConnectionCount = 0;

        ConnectionCount += (OpenUp == WallType.open ? 1 : 0);
        ConnectionCount += (OpenRight == WallType.open ? 1 : 0);
        ConnectionCount += (OpenDown == WallType.open ? 1 : 0);
        ConnectionCount += (OpenLeft == WallType.open ? 1 : 0);

        return ConnectionCount <= 1;
    }

    public void RandomlyKnockdownThisWall(ref WallType wall, float openWallModifier)
    {
        if (wall == WallType.notset)
            wall = (MazeGenerationController.TrueFalseGeneration(openWallModifier) ? WallType.open : WallType.wall);
    }

    public String Output()
    {
        int passageValue = 0;

        passageValue = GetMazeCellEntranceValue();

        return (passageValue < 10 ? "0" : "") + passageValue;
    }

    //For print out
    public int GetMazeCellEntranceValue()
    {
        int passageValue = 0;

        passageValue += (OpenUp == WallType.open ? 1 : 0);
        passageValue += (OpenRight == WallType.open ? 2 : 0);
        passageValue += (OpenDown == WallType.open ? 4 : 0);
        passageValue += (OpenLeft == WallType.open ? 8 : 0);

        return passageValue;
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
}
