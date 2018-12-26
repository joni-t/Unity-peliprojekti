using UnityEngine;
using System.Collections;
using Peliukkeli_ohjaus;

//tällä määritetään liikkeenjarrutus ohjauskomponentin asetukset peliukkelin ohjaukseen
//liikkeenjarrutus toimii molempiin suuntiin (+ ja -)
public class LiikkeenJarrutus_Ohjauskomponentti_Maaritykset : MonoBehaviour {
	//inspector
	public Peliukkeli_ohjaus.LiikkeenJarrutus_Asetukset_Parametrit_Hallintapaneeliin parametrit;
	
	public void Rakenna(Peliukkelin_Ohjausvoiman_Hallitsija peliukkelin_ohjausvoiman_hallitsija) {
		Peliukkeli_ohjaus.LiikkeenJarrutus_Hallintapaneelista_Rakentaja rakentaja=new Peliukkeli_ohjaus.LiikkeenJarrutus_Hallintapaneelista_Rakentaja();
		rakentaja.Rakenna(parametrit, peliukkelin_ohjausvoiman_hallitsija);
	}
}
