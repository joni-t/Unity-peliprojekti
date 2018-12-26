using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Target_ohjaus;
using Ohjaus_laite;
using Ohjaus_2D;
using Menu;
using CoordinatesScaling;

public class Instantiate_Handling : MonoBehaviour {
	// inspector
	public GameObject the_prefab_to_instantiate;
	public bool ignore_collision;
	[System.Serializable]
	public enum Instantiate_Parameters_Type {
		Free_Parameters,
		Toward_Another_Position,
		Rotation_Toward_Given_Target,
		Rotation_Toward_Closest_Target
	}
	public Instantiate_Parameters_Type instantiate_parameters_type;
	public MonoBehaviour instantiate_parameters; //annetaan vastaava parametrien määrittäjä (gameobject, jossa oikeantyyppinen scripti)
	[System.Serializable]
	public enum Moving_Olion_Maaritys {
		kayta_prefabin_yleista,
		anna_instanssille_oma
	}
	public Moving_Olion_Maaritys moving_olion_maaritys;
	public Target_ohjaus.Moving_Gameobject_Parametrit_Hallintapaneeliin moving_parametrit;
	public bool lisataan_ohjausnappula; //voidaan kutsua ulkopuolelta ilman ohjasnappulaakin
	public Ohjaus_laite.Parametrit_Hallintapaneeliin ohjausnappulan_asetukset;
	public bool lisataan_spawneri;
	public Target_ohjaus.GameObject_SpawnerTimer_Parametrit_Hallintapaneeliin spawnerin_asetukset;
	public bool instantioi_lisaprefab;
	public Target_ohjaus.ForDestroyingInstantiated_Parametrit_Hallintapaneeliin lisaprefab_instantiointiasetukset;

	[HideInInspector]
	public Prefab_and_instances prefab_and_instances;
	[HideInInspector]
	public Target_ohjaus.Prefab_and_instances.Moving_Gameobject moving_handling=null; //oletuksena käytetään prefabin yleistä liikuttelijaa
	[HideInInspector]
	public OhjauslaiteRajapinta ohjauslaiterajapinta=null;
	[HideInInspector]
	public SpawnerTimerRajapinta_for_InstatiateHandling spawner_timer_rajapinta=null;
	[HideInInspector]
	public Target_ohjaus.ForDestroyingInstantiated instantioitava_lisaprefab=null;
	[HideInInspector]
	public delegate void Nappula_tilaus(Button_elem ohjausnappula); //muut järjestelmät voivat tilata ohjausnappulan itselleen rakennusvaiheessa ja saavat sen sitten, kun se on valmis
	[HideInInspector]
	public List<Nappula_tilaus> ohjausnappula_tilaukset=null;

	// Use this for initialization
	void Start () {
		Debug.Log("Instantiate_Handling: " + gameObject.name);

		Prefab_and_instances_Hallintapaneelista_Rakentaja prefab_and_instances_rakentaja=new Prefab_and_instances_Hallintapaneelista_Rakentaja();
		prefab_and_instances=prefab_and_instances_rakentaja.Rekisteroi_Prefab(the_prefab_to_instantiate); //rekisteröidään varmuuden vuoksi, koska prefabista ei ole vielä instanssia (prefabin asetuksia ei ole vielä suoritettu). parametrit täydentyy, kun eka instanssi luodaan
		//moving_handling
		if(moving_olion_maaritys==Moving_Olion_Maaritys.anna_instanssille_oma)
			moving_handling=new Target_ohjaus.Moving_Gameobject_Hallintapaneelista_Rakentaja().Rakenna(moving_parametrit);
		//lisätään nappula
		if(lisataan_ohjausnappula) {
			ohjauslaiterajapinta=new OhjauslaiteRajapinta(new Ohjaus_laite.Ohjaus_laite(gameObject.name), this);
			ohjauslaiterajapinta.nappula=new Ohjaus_laite.Hallintapaneelista_Rakentaja().RakennaNappula_Hallintapaneelista(ohjausnappulan_asetukset, ohjauslaiterajapinta.ohjaus_laite);
			//asetetaan koordinaattien skaalausoliolle piirto-funktio (otetaan piirtofunktio käyttöön). mikäli ohjauslaitteen nappulat toteutettiin GUI-elementteinä
			ohjauslaiterajapinta.RegisterDrawingFunction_if_Necessary(Scaled_OnGUI);
		}
		//toimitetaan ohjausnappula niille, jotka sen on tilanneet
		if(lisataan_ohjausnappula & ohjausnappula_tilaukset!=null) //jos on tilauksia
			ohjausnappula_tilaukset.ForEach(delegate(Nappula_tilaus obj) {
				obj(ohjauslaiterajapinta.nappula);
			});
		//lisätään_spawneri
		if(lisataan_spawneri)
			spawner_timer_rajapinta=new SpawnerTimerRajapinta_for_InstatiateHandling(new Target_ohjaus.GameObject_SpawnerTimer_Hallintapaneelista_Rakentaja().Rakenna(spawnerin_asetukset), this);
		//lisäprefab instantiointi
		instantioitava_lisaprefab=new Target_ohjaus.ForDestroyingInstantiated_Hallintapaneelista_Rakentaja().Rakenna(lisaprefab_instantiointiasetukset);
	}

	//käytetään, mikäli ohjauslaitteen nappulat on GUI-elementtejä
	void Scaled_OnGUI() {
		//vain ohjausnappien tulostaminen
		//ei tapahdu instantiointia OnGUI-aikana (siis nappulan tiloja ei päivitetä, kun on multitouch-nappi)
		ohjauslaiterajapinta.ohjaus_laite.PaivitaTilat(); //piirtää nappulat
	}

	//suorittaa instantioinnin
	public void Suorita() {
		Prefab_and_instances.Modified_GameObject modified_gameobject=null;
		if(instantiate_parameters_type==Instantiate_Parameters_Type.Free_Parameters)
			modified_gameobject=instantiate_parameters.GetComponent<Instantiate_Parameters_for_Free>().Suorita(prefab_and_instances, moving_handling);
		else if(instantiate_parameters_type==Instantiate_Parameters_Type.Toward_Another_Position)
			modified_gameobject=instantiate_parameters.GetComponent<Instantiate_Parameters_in_rotation_Toward_Another_Position>().Suorita(prefab_and_instances, moving_handling);
		else if(instantiate_parameters_type==Instantiate_Parameters_Type.Rotation_Toward_Closest_Target)
			modified_gameobject=instantiate_parameters.GetComponent<Instantiate_Parameters_in_rotation_Toward_Closest_Target>().Suorita(prefab_and_instances, moving_handling);
		else if(instantiate_parameters_type==Instantiate_Parameters_Type.Rotation_Toward_Given_Target)
			modified_gameobject=instantiate_parameters.GetComponent<Instantiate_Parameters_in_rotation_Toward_Given_Target>().Suorita(prefab_and_instances, moving_handling);
		if(ignore_collision) Physics.IgnoreCollision(modified_gameobject.gameobject.GetComponent<Collider>(), GetComponent<Collider>());
		modified_gameobject=null;
	}

	//tästä voi tilata ukkelin ohjauksen rakennusvaiheessa ja se toimitetaan sitten, kun se on valmis
	public void Tilaa_Ohjausnappula(Nappula_tilaus tilaus) {
		if(ohjausnappula_tilaukset==null) ohjausnappula_tilaukset=new List<Nappula_tilaus>();
		ohjausnappula_tilaukset.Add(tilaus);
	}
}

public class OhjauslaiteRajapinta : Ohjaus_laite.Ohjauslaitteen_Kayttaja {
	public Instantiate_Handling instantiate_handling;
	public Menu.Button_elem nappula;

	public OhjauslaiteRajapinta(Ohjaus_laite.Ohjaus_laite p_ohjaus_laite, Instantiate_Handling p_instantiate_handling) :base(p_ohjaus_laite) {
		instantiate_handling=p_instantiate_handling;
	}

	public override string ToString ()
	{
		return string.Format ("[Instantiate_Handling]");
	}

	//suorittaa instantioinnin
	//ohjausnappuloiden tilat on tunnistettu aiemmin
	public override void SuoritaToiminnot() {
		if(nappula.tila==true) instantiate_handling.Suorita();
	}
}

public class SpawnerTimerRajapinta_for_InstatiateHandling : Target_ohjaus.Target_ohjaus.GameObject_Spawner.Spawner_Timerin_Kayttaja {
	public Instantiate_Handling instantiate_handling;

	public SpawnerTimerRajapinta_for_InstatiateHandling(Target_ohjaus.Target_ohjaus.GameObject_Spawner.Spawner_Timer p_spawner_timer, Instantiate_Handling p_instantiate_handling) : base(p_spawner_timer) {
		instantiate_handling=p_instantiate_handling;
	}

	//suorittaa instantioinnin
	public override void Spawn() {
		if(instantiate_handling!=null) { //jos ei ole tuhottu
			if(instantiate_handling.gameObject.activeSelf==true) { //mikäli on aktiivinen
				base.Spawn(); //päivittää, kuinka monta kertaa suoritetaan
				for(int i=0; i<nmb_of_to_spawn; i++)
					instantiate_handling.Suorita();
			}
		} else Removing(); //poistetaan
	}
}
