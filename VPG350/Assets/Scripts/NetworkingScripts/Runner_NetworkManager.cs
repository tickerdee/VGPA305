using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Runner_NetworkManager : NetworkManager {

	public static event FailedToConnect Event_FailedToConnect;
	public delegate void FailedToConnect(string error);

	NetworkManager netManag;

	GameObject myPlayerPrefab;

	// Use this for initialization
	void Start () {
		WorldObjectReference.GetInstance().AddObject(this);

		netManag = GetComponent<NetworkManager>();

		this.autoCreatePlayer = false;
		this.playerPrefab = playerPrefab;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//SERVER---------
	public void Host(int port){
		//NetworkConnectionError errorNet = Network.InitializeServer(6, port, false);
		Network.InitializeServer(6, port, false);
	}

	override public void OnServerConnect(NetworkConnection conn){
		WorldEvents.CallPrintToScreen("On Server Connected");

		this.OnServerAddPlayer(conn, 1);
	}

	void OnPlayerConnected(NetworkPlayer player) {
		WorldEvents.CallPrintToScreen("On Player Connected");


	}

	override public void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		WorldEvents.CallPrintToScreen("On Server Add Player");
		Debug.Log("On Server Add Player");
		var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	void OnServerInitialized() {
		WorldEvents.CallPrintToScreen("Server Initialized");
	}

	void SpawnPlayer(Network player){
		//SendMessage();

	}

	//CLIENT-------------
	void OnFailedToConnect(NetworkConnectionError error)
	{
		Event_FailedToConnect(error+"");
		Debug.Log(error);
	}

	public void Connect(string IP, int port){
		//NetworkConnectionError errorNet = Network.Connect(IP, port);
		Network.Connect(IP, port);
	}

	void OnConnectedToServer() {
		WorldEvents.CallPrintToScreen("Connected to server");
	}

	override public void OnClientConnect(NetworkConnection conn){
		WorldEvents.CallPrintToScreen("On Client Connected");

	}
}
