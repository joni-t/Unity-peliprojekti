using UnityEngine;
using System.Collections;
using Peliukkeli_ohjaus;

//tällä määritetään jetback-ohjauskomponentin asetukset peliukkelin ohjaukseen
public class JetBack_Ohjauskomponentti_Maaritykset : MonoBehaviour {
	//inspector
	public Peliukkeli_ohjaus.JetBack_Asetukset_Parametrit_Hallintapaneeliin parametrit;

	public void Rakenna(Peliukkelin_Ohjausvoiman_Hallitsija peliukkelin_ohjausvoiman_hallitsija) {
		Peliukkeli_ohjaus.JetBack_Hallintapaneelista_Rakentaja rakentaja=new Peliukkeli_ohjaus.JetBack_Hallintapaneelista_Rakentaja();
		rakentaja.Rakenna(parametrit, peliukkelin_ohjausvoiman_hallitsija);
	}
}