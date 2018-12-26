using UnityEngine;
using System.Collections;
using Artificial_Intelligence;
using Menu;

public class TekoAlyMaaritykset : MonoBehaviour {
	//inspector
	public Artificial_Intelligence.Artificial_Intelligence_Handling_Parametrit_Hallintapaneeliin tekoaly_asetukset;

	[HideInInspector]
	public Artificial_Intelligence_Handling teko_aly_hallinta;
	[HideInInspector]
	public Peliukkeli_ohjaus.Peliukkeli_ohjaus ukkelin_ohjaus;
	[HideInInspector]
	public Button_elem ampumisnappula;
	public bool ei_ampujaa_saatavilla=false;

	// Use this for initialization
	void Start () {
		Debug.Log("TekoAlyMaaritykset: " + gameObject.name);

		//ohjaus
		ukkelin_ohjaus=GetComponent<Ukkelin_ohjaus>().ukkelin_ohjaus;
		if(ukkelin_ohjaus==null) //ei ole vielä rakennettu -> laitetaan tilaukseen
			GetComponent<Ukkelin_ohjaus>().Tilaa_PeliukkelinOhjaus(OtaVastaan_Ukkeliohjaus);
		//ampuminen
		OhjauslaiteRajapinta ampumisrajapinta=null;
		if(GetComponent<Instantiate_Handling>()!=null) {
			ampumisrajapinta=GetComponent<Instantiate_Handling>().ohjauslaiterajapinta;
			if(ampumisrajapinta==null) //ei ole vielä rakennettu -> laitetaan tilaukseen
				GetComponent<Instantiate_Handling>().Tilaa_Ohjausnappula(OtaVastaan_Ampumisnappula);
			else
				ampumisnappula=GetComponent<Instantiate_Handling>().ohjauslaiterajapinta.nappula;
		} else
			ei_ampujaa_saatavilla=true;
		Initializing();
		//clean!!!
		//tekoaly_asetukset=null;
		//teko_aly_hallinta=null;
		//ukkelin_ohjaus=null;
		//ampumisnappula=null;
	}

	void Initializing() {
		if((ampumisnappula!=null | ei_ampujaa_saatavilla) && ukkelin_ohjaus!=null) // jos kaikki tilaukset on toimitettu
			teko_aly_hallinta=new Artificial_Intelligence_Handling_Hallintapaneelista_Rakentaja().Rakenna(tekoaly_asetukset, gameObject, ukkelin_ohjaus.ohjausvoiman_hallitsija.peliukkelin_liikkeen_hallitsija, ukkelin_ohjaus.ohjausvoiman_hallitsija.hallittavat_ohjaus_komponentit, ampumisnappula);
	}

	public void OtaVastaan_Ukkeliohjaus(Peliukkeli_ohjaus.Peliukkeli_ohjaus p_ukkelin_ohjaus) {
		ukkelin_ohjaus=p_ukkelin_ohjaus;
		Initializing();
	}

	public void OtaVastaan_Ampumisnappula(Button_elem p_ampumisnappula) {
		ampumisnappula=p_ampumisnappula;
		Initializing();
	}
}
