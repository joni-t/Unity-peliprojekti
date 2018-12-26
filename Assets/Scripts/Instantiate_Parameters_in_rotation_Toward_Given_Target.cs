using UnityEngine;
using System.Collections;
using Target_ohjaus;

//tässä annetaan parametrit hallintapaneelista Instantiate_Handling-scriptille, kun instantiointi suoritetaan
//Instantiate_GameObject_to_Position_and_Set_Rotation_Toward_Given_Target
//suorittaa myös instantioinnin kutsusta
public class Instantiate_Parameters_in_rotation_Toward_Given_Target : MonoBehaviour {
	//inspector
	public Target_ohjaus.Koordinaattiasetukset_Hallintapaneeliin initial_position_parameters;
	public GameObject target_gameobject;

	private Vector2 initial_position;
	private Koordinaattiasetukset_Hallintapaneelista_Rakentaja koordinaatti_rakentaja;

	// Use this for initialization
	void Start () {
		koordinaatti_rakentaja=new Koordinaattiasetukset_Hallintapaneelista_Rakentaja();
	}
	
	//voidaan asettaa parametrit ulkoa -> voidaan suorittaa ulkoa
	public void AsetaParametrit(Vector2 p_initial_position) {
		initial_position=p_initial_position;
	}
	
	//päivittää lähtö- ja kohdepisteet
	public void PaivitaPositiot() {
		//puretaan parametrit inspectorista
		initial_position=koordinaatti_rakentaja.Rakenna(initial_position_parameters, transform);
	}

	//voidaan asettaa parametrit ulkoa -> voidaan suorittaa ulkoa
	public void AsetaParametrit(Vector2 p_initial_position, GameObject p_target_gameobject) {
		initial_position=p_initial_position;
		target_gameobject=p_target_gameobject;
	}

	//suorittaa instantioinnin
	public Prefab_and_instances.Modified_GameObject Suorita(Prefab_and_instances prefab_and_instances, Target_ohjaus.Prefab_and_instances.Moving_Gameobject moving_handling) {
		PaivitaPositiot();
		return prefab_and_instances.Instantiate_GameObject_to_Position_and_Set_Rotation_Toward_Given_Target(initial_position, target_gameobject, moving_handling);
	}
}
