using UnityEngine;
using System.Collections;
using Target_ohjaus;
using Target_ohjaus_Level_hallintaan;
using Level_hallinta;

//liittää prefabin target-ohjaukseen
//liittää myös törmäystapahtumat Target-Ohjaukseen
public class Liita_Prefab_Target_ohjaukseen : MonoBehaviour {
	//inspector
	public Target_ohjaus.Prefab_and_instances_Parametrit_Hallintapaneeliin tiedot;
	public bool aseta_tapahtumien_rekisterointi_for_instantiate;
	public Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances tapahtumien_rekisterointi_for_instantiate;
	public bool aseta_tapahtumien_rekisterointi_for_destroy;
	public Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances tapahtumien_rekisterointi_for_destroy;

	[HideInInspector]
	public Target_ohjaus.Prefab_and_instances_Hallintapaneelista_Rakentaja rakentaja=null;
	[HideInInspector]
	Prefab_and_instances prefab_and_instances=null;

	void Awake() {
		rakentaja=new Target_ohjaus.Prefab_and_instances_Hallintapaneelista_Rakentaja();
		//rekisteröidään prefab target-ohjaukseen ja otetaan scenelle lisätyt instanssit talteen
		prefab_and_instances=rakentaja.Rekisteroi_Prefab(gameObject); //rekisteröi prefabin Target-ohjaukseen. ei rakenneta loppuun asti, koska järjestelmästä ei löydy tarvittavia tietoja vielä
	}

	// Use this for initialization
	void Start () {
		if(prefab_and_instances.parametrit_taydennetty==false) { //parametrit täydennetään vain kerran (eka instanssi)
			Debug.Log("Start prefab: " + gameObject.name);
			rakentaja.Rakenna(tiedot, gameObject); //täydennetään parametreilla
			//asetetaan tapahtuman rekisteröijät
			Toiminnon_suorittaja toiminnon_suorittaja_for_instantiate=null;
			Toiminnon_suorittaja toiminnon_suorittaja_for_destroy=null;

			if(aseta_tapahtumien_rekisterointi_for_instantiate)
				toiminnon_suorittaja_for_instantiate=Rakenna_Toiminnon_suorittaja_for_Prefab_and_Instances(tapahtumien_rekisterointi_for_instantiate);

			Debug.Log("aseta_tapahtumien_rekisterointi_for_destroy: " + gameObject.name);

			if(aseta_tapahtumien_rekisterointi_for_destroy)
				toiminnon_suorittaja_for_destroy=Rakenna_Toiminnon_suorittaja_for_Prefab_and_Instances(tapahtumien_rekisterointi_for_destroy);
			prefab_and_instances.AsetaTapahtumanRekisteroija(new Target_ohjaus_Tapahtumien_rekisteroija<Prefab_and_instances>(toiminnon_suorittaja_for_instantiate, tapahtumien_rekisterointi_for_instantiate.ohita_levelin_tila), new Target_ohjaus_Tapahtumien_rekisteroija<Prefab_and_instances>(toiminnon_suorittaja_for_destroy, tapahtumien_rekisterointi_for_destroy.ohita_levelin_tila));
			//liitetään mahdolliselle Vaihda_prefab toiminnansuorittajalle tapahtumanrekisteröijä
			LiitaTapahtumanRekisterointi_VaihdaPrefab_Toiminnolle(tapahtumien_rekisterointi_for_instantiate.tyyppilista2, toiminnon_suorittaja_for_instantiate, prefab_and_instances.tapahtuman_rekisteroija_for_instantiate);
			LiitaTapahtumanRekisterointi_VaihdaPrefab_Toiminnolle(tapahtumien_rekisterointi_for_destroy.tyyppilista2, toiminnon_suorittaja_for_destroy, prefab_and_instances.tapahtuman_rekisteroija_for_destroy);
			//spawnerille
			Toiminnon_suorittaja toiminnon_suorittaja_for_spawner=null;
			if(tiedot.luo_prefabille_spawneri)
				if(tiedot.spawnerin_asetukset.aseta_tapahtumien_rekisterointi) {
					toiminnon_suorittaja_for_spawner=Rakenna_Toiminnon_suorittaja(tiedot.spawnerin_asetukset.tapahtumien_rekisterointi);
				rakentaja.gameobject_spawner.AsetaTapahtumanRekisteroija(new Target_ohjaus_Tapahtumien_rekisteroija<Target_ohjaus.Target_ohjaus.GameObject_Spawner>(toiminnon_suorittaja_for_spawner, tiedot.spawnerin_asetukset.tapahtumien_rekisterointi.ohita_levelin_tila));
				}
			//clean!!!
			toiminnon_suorittaja_for_instantiate=null;
			toiminnon_suorittaja_for_destroy=null;
			toiminnon_suorittaja_for_spawner=null;
		}
		prefab_and_instances.Add_new_GameObject(gameObject, null, Vector2.zero, null);
		//clean!!!
		//tiedot=null;
		rakentaja=null;
		prefab_and_instances=null;
	}

	//vaihda-prefabtoiminnolle asetetaan tapahtumanrekisteröijä, joka kytkee sen samoihin tapahtumiin kuin Prefab_and_instances-oliokin
	//kun tapahtuma rekisteröidään (tai yritetään rekisteröidä) prefab_and_instances-olioon, se rekisteröidään myös vaihda-prefabiin
	//esim. voidaan asettaa prefab vaihtumaan toiseksi 3:sta osumasta -> osuma yrittää tuhota prefabin -> ei anna suorittaa -> rekisteröityy myös Vaihda_prefab:lle -> 3:sta tuhoutumisyrityksestä vaihdetaan (ei saa edelleenkään tuhota)
	public void LiitaTapahtumanRekisterointi_VaihdaPrefab_Toiminnolle(Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances.Tyyppilista2 tyyppi, Toiminnon_suorittaja toiminnon_suorittaja, Target_ohjaus_Tapahtumien_rekisteroija<Prefab_and_instances> tapahtuman_rekisteroija) {
		if(toiminnon_suorittaja!=null && tyyppi==Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances.Tyyppilista2.Vaihda_prefab)
			((Vaihda_prefab) toiminnon_suorittaja).AsetaTapahtumien_rekisteroija(tapahtuman_rekisteroija);
	}

	//toiminnon suorittaja vain prefab_and_instance:lle
	public static Toiminnon_suorittaja Rakenna_Toiminnon_suorittaja_for_Prefab_and_Instances(Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances tiedot) {
		Toiminnon_suorittaja toiminnon_suorittaja=null;
		if(tiedot.tyyppilista2==Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances.Tyyppilista2.Valitse_listasta_1) { //valitaan listasta 1
			if(tiedot.tyyppilista==Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances.Tyyppilista.Muuta_laskuria)
				toiminnon_suorittaja=Rakenna_Toiminnon_suorittaja(tiedot);
		} else if(tiedot.tyyppilista2==Toiminnon_kohteen_Valitsin_for_Prefab_and_Instances.Tyyppilista2.Vaihda_prefab) // valitaan listasta 2
			toiminnon_suorittaja=tiedot.parametrit.GetComponent<Vaihda_prefab_Maaritykset>().Rakenna();
		return toiminnon_suorittaja;
	}
	//toiminnon suorittaja kaikille
	public static Toiminnon_suorittaja Rakenna_Toiminnon_suorittaja(Toiminnon_kohteen_Valitsin_Kaikille tiedot) {
		Toiminnon_suorittaja toiminnon_suorittaja=null;
		if(tiedot.tyyppilista==Toiminnon_kohteen_Valitsin_Kaikille.Tyyppilista.Muuta_laskuria)
			toiminnon_suorittaja=tiedot.parametrit.GetComponent<MuutaLaskuria_Maaritykset>().Rakenna();
		return toiminnon_suorittaja;
	}

	//********* Törmäystapahtumien havainnointi *******************
	//nämä havaitsee törmäystapahtumat ja lähettää tiedot edelleen Target_ohjaukselle
	void OnCollisionEnter (Collision other)
	{
		Target_ohjaus.Target_ohjaus.Instance.GetCollisions(gameObject, Target_ohjaus.Target_ohjaus.int_OnCollisionEnter, other);
	}	
	void OnCollisionExit (Collision other)
	{
		Target_ohjaus.Target_ohjaus.Instance.GetCollisions(gameObject, Target_ohjaus.Target_ohjaus.int_OnCollisionExit, other);
	}	
	void OnCollisionStay (Collision other)
	{
		Target_ohjaus.Target_ohjaus.Instance.GetCollisions(gameObject, Target_ohjaus.Target_ohjaus.int_OnCollisionStay, other);
	}	
	void OnTriggerEnter (Collider other)
	{
		Target_ohjaus.Target_ohjaus.Instance.GetTriggers(gameObject, Target_ohjaus.Target_ohjaus.int_OnTriggerEnter, other);
	}	
	void OnTriggerExit (Collider other)
	{
		Target_ohjaus.Target_ohjaus.Instance.GetTriggers(gameObject, Target_ohjaus.Target_ohjaus.int_OnTriggerExit, other);
	}	
	void OnTriggerStay (Collider other)
	{
		Target_ohjaus.Target_ohjaus.Instance.GetTriggers(gameObject, Target_ohjaus.Target_ohjaus.int_OnTriggerStay, other);
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		Target_ohjaus.Target_ohjaus.Instance.GetTriggers(gameObject, Target_ohjaus.Target_ohjaus.int_OnControllerColliderHit, hit.collider);
	}
}
