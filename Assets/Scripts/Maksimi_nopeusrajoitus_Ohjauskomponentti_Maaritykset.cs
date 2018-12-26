using UnityEngine;
using System.Collections;
using Peliukkeli_ohjaus;

//tällä määritetään maksiminopeusrajoituksen ohjauskomponentin asetukset peliukkelin ohjaukseen
//nopeusrajoitus on yksisuuntainen
public class Maksimi_nopeusrajoitus_Ohjauskomponentti_Maaritykset : MonoBehaviour {
	//inspector
	public Peliukkeli_ohjaus.Maksimi_nopeusrajoitus_Asetukset_Parametrit_Hallintapaneeliin parametrit;
	
	public void Rakenna(Peliukkelin_Ohjausvoiman_Hallitsija peliukkelin_ohjausvoiman_hallitsija) {
		Peliukkeli_ohjaus.Maksimi_nopeusrajoitus_Hallintapaneelista_Rakentaja rakentaja=new Peliukkeli_ohjaus.Maksimi_nopeusrajoitus_Hallintapaneelista_Rakentaja();
		rakentaja.Rakenna(parametrit, peliukkelin_ohjausvoiman_hallitsija);
	}
}
