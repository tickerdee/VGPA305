using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class spanishLang : MonoBehaviour {
	
	public M_UIComponents mainComponents;
	public M_UIController MainController;

	public PauseMenuUIComponents pauseMenuComponents;
	public InGameUIController inGameUIController;

	//text on the main menu. NewGame, Online, Settings, Tutorial, Quit
	public Text nuevoJuegoText, enLineaText, opcionesText, tutorialesText, tutorialInsideText, tutorialTextControls, cerrarText;
	//text on the online. Connect, Host
	public Text conectarText, hostText;
	//text on settings. Resolution, Windowed, Sound, Language, English, Spanish
	public Text resolucionText, winToggleText, sonidoText, lenguajeText, inglesBText, espanolBText; 

	// Use this for initialization
	void Start () {
		if (mainComponents != null) 
		{
			mainComponents.Spanish.onClick.AddListener (spanishData);
		}
		else
		{
			pauseMenuComponents.Spanish.onClick.AddListener (spanishData);
		}
	}
	
	public void spanishData()
	{
		nuevoJuegoText.text="Nueva Partida";
		enLineaText.text = "En Linea";
		opcionesText.text = "Ajustes";
		tutorialesText.text = "Tutorial";
		tutorialInsideText.text = "COMO JUGAR\n";
		cerrarText.text = "Salir";

		tutorialTextControls.text = "W - Adelante\nS - Atrás\nA - Izquierda\nD - Derecha\nShift + Movimiento - Correr\nP/Esc - Pausa\n";

		conectarText.text = "Conectar";
		hostText.text = "Host";
		resolucionText.text = "Resolución";
		winToggleText.text = "Ventana";
		sonidoText.text = "Sonido";
		lenguajeText.text = "Lenguaje";
		inglesBText.text = "English";
		espanolBText.text = "Español";
	}
}
