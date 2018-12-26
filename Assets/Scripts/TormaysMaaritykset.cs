using UnityEngine;
using System.Collections;
using Target_ohjaus;
using Target_ohjaus_Level_hallintaan;
using Level_hallinta;

public class TormaysMaaritykset : MonoBehaviour {
	//inspector
	public Target_ohjaus.Collisions_and_Triggers_Handling_Parametrit_Hallintapaneeliin asetukset;

	// Use this for initialization
	void Start () {
		Target_ohjaus.Collisions_and_Triggers_Handling_Hallintapaneelista_Rakentaja.LisatoiminnonSuorittajaDelegaatinPalauttaja lisatoiminto_delegaatin_palauttaja=PalautaDelegaatti;
		Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling tormayshallinta = new Target_ohjaus.Collisions_and_Triggers_Handling_Hallintapaneelista_Rakentaja().Rakenna(asetukset, gameObject, lisatoiminto_delegaatin_palauttaja);
		//asetetaan tapahtuman rekisteröijät
		Toiminnon_suorittaja toiminnon_suorittaja=null;
		if(asetukset.aseta_tapahtumien_rekisterointi) {
			toiminnon_suorittaja=Liita_Prefab_Target_ohjaukseen.Rakenna_Toiminnon_suorittaja(asetukset.tapahtumien_rekisterointi);
			tormayshallinta.AsetaTapahtumanRekisteroija(new Target_ohjaus_Tapahtumien_rekisteroija<Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling>(toiminnon_suorittaja, asetukset.tapahtumien_rekisterointi.ohita_levelin_tila));
		}
		//clean!!!
		asetukset=null;
		lisatoiminto_delegaatin_palauttaja=null;
		tormayshallinta=null;
		toiminnon_suorittaja=null;
	}

	//palauttaa törmäyshallinnan rakentajalle delegaatin lisätoiminnon suorittajaan
	public Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling.ToiminnonSuorittaja PalautaDelegaatti() {
		return GetComponent<Collisions_and_Triggers_Handling_LisatoimintojenSuorittaja>().Luo_ja_Palauta_Delegaatti_ToiminnonSuorittajaan(gameObject);
	}
}
