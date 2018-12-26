using UnityEngine;
using System.Collections;
using Target_ohjaus;

//tässä annetaan parametrit hallintapaneelista Instantiate_Handling-scriptille, kun instantiointi suoritetaan
//Instantiate_GameObject_to_Position_and_Set_Rotation_Toward_Closest_Target
//suorittaa myös instantioinnin kutsusta
public class Instantiate_Parameters_in_rotation_Toward_Closest_Target : MonoBehaviour {
	//inspector
	public Target_ohjaus.Koordinaattiasetukset_Hallintapaneeliin initial_position_parameters;
	public Vector2 find_target_from_position;
	public float max_distance_to_target;
	public bool if_not_find_target_instantiate_still;
	
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
	public void AsetaParametrit(Vector2 p_initial_position, Vector2 p_find_target_from_position, float p_max_distance_to_target, bool p_if_not_find_target_instantiate_still) {
		initial_position=p_initial_position;
		find_target_from_position=p_find_target_from_position;
		max_distance_to_target=p_max_distance_to_target;
		if_not_find_target_instantiate_still=p_if_not_find_target_instantiate_still;
	}

	//suorittaa instantioinnin
	public Prefab_and_instances.Modified_GameObject Suorita(Prefab_and_instances prefab_and_instances, Target_ohjaus.Prefab_and_instances.Moving_Gameobject moving_handling) {
		PaivitaPositiot();
		return prefab_and_instances.Instantiate_GameObject_to_Position_and_Set_Rotation_Toward_Closest_Target(initial_position, find_target_from_position, max_distance_to_target, if_not_find_target_instantiate_still, moving_handling);
	}
}
