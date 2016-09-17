using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class spanishLang : MonoBehaviour {
	
	public M_UIComponents mainComponents;
	public M_UIController MainController;

	//text on the main menu. NewGame, Online, Settings, Tutorial, Quit
	public Text nuevoJuegoText, enLineaText, opcionesText, tutorialesText, tutorialInsideText, tutorialTextControls, cerrarText;
	//text on the online. Connect, Host
	public Text conectarText, hostText;
	//text on settings. Resolution, Windowed, Sound, Language, English, Spanish
	public Text resolucionText, winToggleText, sonidoText, lenguajeText, inglesBText, espanolBText; 

	// Use this for initialization
	void Start () {
		mainComponents.Spanish.onClick.AddListener(spanishData);
	}
	
	public void spanishData()
	{
		nuevoJuegoText.text="Juego Nuevo";
		enLineaText.text = "En Linea";
		opcionesText.text = "Opciones";
		tutorialesText.text = "Tutorial";
		tutorialInsideText.text = "COMO JUGAR\n";
		cerrarText.text = "Cerrar";

		tutorialTextControls.text = "W - Mover Hacia Adelante\nS - Mover Haciar Atras\nA - Mover Izquierda\nD - Mover Derecha\nShift + Movimiento - Correr\nP/Esc - Pausa\n";

		conectarText.text = "Conectar";
		hostText.text = "Host";
		resolucionText.text = "Resolucion";
		winToggleText.text = "Ventana";
		sonidoText.text = "Sonido";
		lenguajeText.text = "Lenguaje";
		inglesBText.text = "Ingles";
		espanolBText.text = "Espanol";
	}
}
