using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ohjaus_2D;
using Target_ohjaus;
using Ohjaus_laite;
using Peliukkeli_ohjaus;
using CoordinatesScaling;

public class Ukkelin_ohjaus : MonoBehaviour {
	// inspector
	public List<Hallittava_Ohjauskomponetti_Asetukset_Parametrit_Hallintapaneeliin> ohjaussuunnat; //ohjausnappulat
	[System.Serializable]
	public class Automaattiset_ohjauskomponentit {
		public enum Tyyppi {
			JetBack,
			Maksimi_nopeusrajoitus,
			LiikkeenJarrutus
		}
		public Tyyppi tyyppi; //valitaan ohjauskomponentin tyyppi
		public MonoBehaviour parametrit; //annetaan vastaava parametrien määrittäjä (gameobject, jossa oikeantyyppinen scripti)
	}
	public List<Automaattiset_ohjauskomponentit> automaattiset_ohjauskomponentit;
	public Target_ohjaus.ForDestroyingInstantiated_Parametrit_Hallintapaneeliin hyppyaani_asetukset;

	[HideInInspector]
	public Peliukkeli_ohjaus.Peliukkeli_ohjaus ukkelin_ohjaus;
	[HideInInspector]
	public Ohjaus_laite.Ohjaus_laite peliukkelin_ohjauslaite;
	[HideInInspector]
	public Animator animator;
	[HideInInspector]
	public CharacterController character_controller;
	[HideInInspector]
	public delegate void Ukkelin_ohjaus_tilaus(Peliukkeli_ohjaus.Peliukkeli_ohjaus ukkelin_ohjaus); //muut järjestelmät voivat tilata ukkelinohjauksen itselleen rakennusvaiheessa ja saavat sen sitten, kun se on valmis
	[HideInInspector]
	public List<Ukkelin_ohjaus_tilaus> ukkelin_ohjaus_tilaukset=null;

	// Use this for initialization
	void Start () {
		Debug.Log("Ukkelin_ohjaus: " + gameObject.name);

		animator=GetComponent<Animator>();
		character_controller=GetComponent<CharacterController>();
		//ohjauslaite
		peliukkelin_ohjauslaite=new Ohjaus_laite.Ohjaus_laite(gameObject.name);
		Peliukkeli_ohjaus.Peliukkelin_Ohjausvoiman_Hallitsija peliukkelin_ohjausvoiman_hallitsija = new Peliukkeli_ohjaus.Peliukkelin_Ohjausvoiman_Hallitsija(new Peliukkeli_ohjaus.Peliukkelin_Liikkeen_Hallitsija_CharacterControllerilla(character_controller));
		//ohjausnappulat inspectorista
		Hallittava_Ohjauskomponetti_Hallintapaneelista_Rakentaja nappula_rakentaja=new Hallittava_Ohjauskomponetti_Hallintapaneelista_Rakentaja();
		ohjaussuunnat.ForEach(delegate(Hallittava_Ohjauskomponetti_Asetukset_Parametrit_Hallintapaneeliin obj) { //lisätään ohjausnappulat
			nappula_rakentaja.Rakenna(obj, peliukkelin_ohjausvoiman_hallitsija, peliukkelin_ohjauslaite);
		});
		//automaattiset ohjauskomponentit inspectorista
		automaattiset_ohjauskomponentit.ForEach(delegate(Automaattiset_ohjauskomponentit obj) {
			if(obj.tyyppi==Automaattiset_ohjauskomponentit.Tyyppi.JetBack)
				obj.parametrit.GetComponent<JetBack_Ohjauskomponentti_Maaritykset>().Rakenna(peliukkelin_ohjausvoiman_hallitsija);
			else if(obj.tyyppi==Automaattiset_ohjauskomponentit.Tyyppi.Maksimi_nopeusrajoitus)
				obj.parametrit.GetComponent<Maksimi_nopeusrajoitus_Ohjauskomponentti_Maaritykset>().Rakenna(peliukkelin_ohjausvoiman_hallitsija);
			else if(obj.tyyppi==Automaattiset_ohjauskomponentit.Tyyppi.LiikkeenJarrutus)
				obj.parametrit.GetComponent<LiikkeenJarrutus_Ohjauskomponentti_Maaritykset>().Rakenna(peliukkelin_ohjausvoiman_hallitsija);
		});
		//hyppyääni
		Target_ohjaus.ForDestroyingInstantiated hyppyaani=null;
		if(hyppyaani_asetukset.prefab_to_instantiate!=null) {
			hyppyaani=new Target_ohjaus.ForDestroyingInstantiated_Hallintapaneelista_Rakentaja().Rakenna(hyppyaani_asetukset);
		}
		ukkelin_ohjaus=new Peliukkeli_ohjaus.Peliukkeli_ohjaus(peliukkelin_ohjauslaite, peliukkelin_ohjausvoiman_hallitsija, animator, hyppyaani);
		//asetetaan koordinaattien skaalausoliolle piirto-funktio (otetaan piirtofunktio käyttöön). mikäli ohjauslaitteen nappulat toteutettiin GUI-elementteinä
		ukkelin_ohjaus.RegisterDrawingFunction_if_Necessary(Scaled_OnGUI);
		//toimitetaan ukkelin ohjaus niille, jotka sen on tilanneet
		if(ukkelin_ohjaus_tilaukset!=null) //jos on tilauksia
			ukkelin_ohjaus_tilaukset.ForEach(delegate(Ukkelin_ohjaus_tilaus obj) {
				obj(ukkelin_ohjaus);
			});
		//clean!!!
		ohjaussuunnat=null;
		automaattiset_ohjauskomponentit=null;
		hyppyaani_asetukset=null;
		//ukkelin_ohjaus=null;
		//peliukkelin_ohjauslaite=null;
		animator=null;
		character_controller=null;
		peliukkelin_ohjausvoiman_hallitsija=null;
		nappula_rakentaja=null;
		hyppyaani=null;
		//ukkelin_ohjaus_tilaukset=null;
	}

	//käytetään, mikäli ohjauslaitteen nappulat on GUI-elementtejä
	void Scaled_OnGUI() {
		//vain ohjausnappien tulostaminen
		//ei tapahdu ukkelin ohjausta OnGUI-aikana (siis nappulan tiloja ei päivitetä, kun on multitouch-nappi)
		peliukkelin_ohjauslaite.PaivitaTilat(); //piirtää nappulat
	}

	//ohjausvoimien asettaminen
	//ohjausnappuloiden vaikutus ratkaistu jo aiemmin
	void FixedUpdate() {
		ukkelin_ohjaus.Laske_ja_AsetaOhjausvoima(Time.fixedDeltaTime);
	}

	//tästä voi tilata ukkelin ohjauksen rakennusvaiheessa ja se toimitetaan sitten, kun se on valmis
	public void Tilaa_PeliukkelinOhjaus(Ukkelin_ohjaus_tilaus tilaus) {
		if(ukkelin_ohjaus_tilaukset==null) ukkelin_ohjaus_tilaukset=new List<Ukkelin_ohjaus_tilaus>();
		ukkelin_ohjaus_tilaukset.Add(tilaus);
	}
}
