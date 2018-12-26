using UnityEngine;
using System.Collections;
using CoordinatesScaling;
using Menu;
using MenuFactory;

public class Piirtomokkuloiden_asetukset : MonoBehaviour {
	public float originalScreenWidth;
	public float originalScreenHeight;

	[HideInInspector] // Hides var below
	public CoordinatesScaling.GUICoordinatesScaling coordinates_scaling=GUICoordinatesScaling.Instance;
	public Menu.Menu menu=Menu.Menu.Instance;
	public MenuFactory.MenuFactory menufactory=MenuFactory.MenuFactory.Instance;

	void Awake () {
		Debug.Log("Piirtomokkuloiden_asetukset: Awake and Clean");
		coordinates_scaling.Clean();
		coordinates_scaling.AsetaParametrit(originalScreenWidth, originalScreenHeight);
		coordinates_scaling.RegisterDrawingFunction(Scaled_OnGUI);
	}

	void OnGUI () {
		//skaalaa ja piirtää
		coordinates_scaling.Scaling_and_Drawing();
	}

	void Scaled_OnGUI() {
		menu.Piirra();
		if(menu.current_page.sisalto!=null)
			menufactory.ToiminnotPiirrettavanSivunMukaan();
	}
}
