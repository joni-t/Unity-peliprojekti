using UnityEngine;
using System.Collections;
using Target_ohjaus;
using Ohjaus_2D;

//sinkoaa peliukkelin räjähdyksen voimasta ilmaan tuhoaa sen
//playerin on sisällettävä CharacterController ja räjähdyksen SphereCollider
//singottavan playerin pitää olla asetettuna targetiksi Target_ohjauksessa
public class ExplosionJump : MonoBehaviour {
	public float max_distance_to_target_for_checking=3; //määrittää vaikutusalueen laajuuden, jonka sisällä oleva ukkeli tutkitaan tarkemmin
	public float matka_tuhoamis_positioon=5; //etäisyys, jonka päähän player "ammutaan"
	public float liipaisuetaisyys_tuhoutumis_positiosta=1; //target_ohjauksen parametri
	public float sinkoutumis_nopeus=20;
	private Prefab_and_instances this_prefab_and_instances;
	private GameObject target_gameobject;
	private UkkelinMitat ukkelin_mitat;
	private Vector2 this_position;
	private CharacterController character_controller;

	// homma tapahtuu, kun räjähdys syntyy
	void Start () {
		this_prefab_and_instances=Target_ohjaus.Target_ohjaus.Instance.Get_Prefab_with_Name(gameObject.name);
		target_gameobject=this_prefab_and_instances.Find_Nearest_Target_for_GameObject_from_given_position(Ohjattava_2D.Convert_to_Vector2(transform.position), max_distance_to_target_for_checking);
		if(target_gameobject!=null) { //on lähellä vaikutusalueella -> tutkitaan tarkemmin, onko ukkeli (varpaat) räjähdyksen sisällä
			character_controller=this_prefab_and_instances.target_prefabs[0].thePrefab.gameObject.GetComponent<CharacterController>();
			ukkelin_mitat=new UkkelinMitat(Ohjattava_2D.Convert_to_Vector2(character_controller.center), character_controller.radius, character_controller.height);
			this_position=Ohjattava_2D.Convert_to_Vector2(gameObject.transform.position);
			Sinkoa_ja_Tuhoa_jos_Koskee();
		}
	}

	//jos varpaat koskettaa räjähdystä, ukkeli singotaan huisi helvettiin siitä ja tuhotaan
	//suunta määräytyy räjähdyksen keskipisteestä ukkelin keskipisteen suuntaan
	public void Sinkoa_ja_Tuhoa_jos_Koskee() {
		Vector2 ukkelin_positio=Ohjattava_2D.Convert_to_Vector2(target_gameobject.transform.position);
		Vector2 ukkelin_kosketuspositio=ukkelin_mitat.AnnaLahinKosketusPositio(ukkelin_positio, this_position);
		if((this_position-ukkelin_kosketuspositio).sqrMagnitude<=gameObject.GetComponent<SphereCollider>().radius) { //jos on räjähdyksen sisällä, singotaan
			Prefab_and_instances target_prefab_and_instances=Target_ohjaus.Target_ohjaus.Instance.Get_Prefab_with_Name(target_gameobject.name);
			Prefab_and_instances.Modified_GameObject modified_gameobject=target_prefab_and_instances.Get_Modified_GameObject(target_gameobject);
			modified_gameobject.target_position=(ukkelin_positio-this_position).normalized*matka_tuhoamis_positioon+this_position;
			modified_gameobject.moving_handling=new Prefab_and_instances.MoveToSelectedPosition_and_ThenDestroying(sinkoutumis_nopeus, 0, null);
			((Prefab_and_instances.MoveToSelectedPosition_and_ThenDestroying) modified_gameobject.moving_handling).AsetaLiipaisuetaisyys_kohdepositionista(liipaisuetaisyys_tuhoutumis_positiosta);
			character_controller.enabled=false;
		}
	}

	//ukkelin mitat character-controller esityksenä
	public class UkkelinMitat {
		public Vector2 center;
		public float radius;
		public float height;

		public UkkelinMitat(Vector2 p_center, float p_radius, float p_height) {
			center=p_center;
			radius=p_radius;
			height=p_height;
		}

		//palauttaa varpaiden tasolta lähimmän pisteen annettua pistettä vastaan
		//x-suunnassa testataan vain molemmat ääripisteet ja keskipiste
		public Vector2 AnnaLahinKosketusPositio(Vector2 ukkelin_positio, Vector2 explosion_positio) {
			Vector2 kosketus_positio=new Vector2(0,ukkelin_positio.y-height/2+center.y);
			float x=ukkelin_positio.x+center.x;
			float dist=Mathf.Abs(explosion_positio.x-x);
			if(Mathf.Abs(explosion_positio.x-(x+radius))<dist)
				x+=radius;
			else if(Mathf.Abs(explosion_positio.x-(x-radius))<dist)
				x-=radius;
			kosketus_positio.x=x;
			return kosketus_positio;
		}
	}
}
