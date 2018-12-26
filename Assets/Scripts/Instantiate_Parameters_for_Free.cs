using UnityEngine;
using System.Collections;
using Target_ohjaus;

//tässä annetaan parametrit hallintapaneelista Instantiate_Handling-scriptille, kun instantiointi suoritetaan
//Instantiate_new_GameObject eli parametrit vapaasti
//suorittaa myös instantioinnin kutsusta
public class Instantiate_Parameters_for_Free : MonoBehaviour {
	//inspector
	public Target_ohjaus.Koordinaattiasetukset_Hallintapaneeliin initial_position_parameters;
	public Target_ohjaus.Rotationasetukset_Hallintapaneeliin initial_quaternion_parameters;
	public GameObject target_gameobject;
	public Target_ohjaus.Koordinaattiasetukset_Hallintapaneeliin target_position_parameters;

	private Vector2 initial_position;
	private Quaternion initial_quaternion;
	private Vector2 target_position;
	private Koordinaattiasetukset_Hallintapaneelista_Rakentaja koordinaatti_rakentaja;
	private Rotationasetukset_Hallintapaneelista_Rakentaja rotation_rakentaja;

	// Use this for initialization
	void Start () {
		koordinaatti_rakentaja=new Koordinaattiasetukset_Hallintapaneelista_Rakentaja();
		rotation_rakentaja=new Rotationasetukset_Hallintapaneelista_Rakentaja();
	}

	//voidaan asettaa parametrit ulkoa -> voidaan suorittaa ulkoa
	public void AsetaParametrit(Vector2 p_initial_position, Quaternion p_initial_quaternion, GameObject p_target_gameobject, Vector2 p_target_position) {
		initial_position=p_initial_position;
		initial_quaternion=p_initial_quaternion;
		target_gameobject=p_target_gameobject;
		target_position=p_target_position;
	}

	//päivittää lähtö- ja kohdepisteet
	public void PaivitaPositiot() {
		//puretaan parametrit inspectorista
		initial_position=koordinaatti_rakentaja.Rakenna(initial_position_parameters, transform);
		initial_quaternion=rotation_rakentaja.Rakenna(initial_quaternion_parameters, transform);
		target_position=koordinaatti_rakentaja.Rakenna(target_position_parameters, transform);
	}

	//suorittaa instantioinnin
	public Prefab_and_instances.Modified_GameObject Suorita(Prefab_and_instances prefab_and_instances, Target_ohjaus.Prefab_and_instances.Moving_Gameobject moving_handling) {
		PaivitaPositiot();
		return prefab_and_instances.Instantiate_new_GameObject(initial_position, initial_quaternion, target_gameobject, target_position, moving_handling);
	}
}
