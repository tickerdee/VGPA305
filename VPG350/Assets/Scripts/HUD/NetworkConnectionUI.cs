using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkConnectionUI : MonoBehaviour {

	public Button Connect, Host;
	public InputField IP, Port;
	public Text displayText;
	
	// Use this for initialization
	void Start () {
		Connect.onClick.AddListener(OnConnectClick);

		Host.onClick.AddListener(OnHostClick);

		Runner_NetworkManager.Event_FailedToConnect += OnConnectFailed;

		WorldEvents.Event_PrintToScreen += PrintToScreen;
	}

	void OnConnectFailed(string error){
		displayText.text += "\n"+error;
	}

	Runner_NetworkManager GetNetworkManager(){
		return WorldObjectReference.GetInstance().GetObject<Runner_NetworkManager>();
	}

	void OnConnectClick(){
		GetNetworkManager().Connect(IP.text, int.Parse(Port.text));
	}

	void OnHostClick(){
		GetNetworkManager().Host(int.Parse(Port.text));
	}

	void PrintToScreen(string message){
		displayText.text += "\n"+message;
	}
}
