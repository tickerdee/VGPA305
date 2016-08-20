using UnityEngine;
using System.Collections;

public class WorldEvents : MonoBehaviour {

	//A static referenceable actor
		//So we can globally subscribe and call events across our game

	public static event MazeFinished Event_MazeFinished;
	public static event PreBeginGame Event_PreBeginGame;
	public static event BeginGame Event_BeginGame;
	public static event BeginGame Event_PlayerJoined;
	public static event BeginGame Event_PlayerLeft;
	public static event EndGame Event_EndGame;

	public static event PrintToScreen Event_PrintToScreen;

	public delegate void MazeFinished();
	public delegate void PreBeginGame();
	public delegate void BeginGame();
	public delegate void PlayerJoined();
	public delegate void PlayerLeft();
	public delegate void EndGame();

	public delegate void PrintToScreen(string message);

	public void CallMazeFinished()					{	if(Event_MazeFinished != null) 	Event_MazeFinished();			}
	public void CallPreBeginGame()					{	if(Event_PreBeginGame != null) 	Event_PreBeginGame();			}
	public void CallBeginGame()						{	if(Event_BeginGame != null) 	Event_BeginGame();				}
	public void CallPlayerJoined()					{	if(Event_PlayerJoined != null) 	Event_PlayerJoined();			}
	public void CallPlayerLeft()					{	if(Event_PlayerLeft != null) 	Event_PlayerLeft();				}
	public void CallEndGame()						{	if(Event_EndGame != null) 		Event_EndGame();				}
	public static void CallPrintToScreen(string message)	{	if(Event_PrintToScreen != null) Event_PrintToScreen(message);	}
}
