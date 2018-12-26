using UnityEngine;
using System.Collections;
using Level_hallinta;

//tällä määritetään Muuta laskuria asetukset tapahtuman rekisteröijälle
public class MuutaLaskuria_Maaritykset : MonoBehaviour {
	//inspector
	public Level_hallinta.Muuta_laskuria_Parametrit_Hallintapaneeliin parametrit;
	
	public Muuta_laskuria Rakenna() {
		Level_hallinta.Muuta_laskuria_Hallintapaneelista_Rakentaja rakentaja=new Level_hallinta.Muuta_laskuria_Hallintapaneelista_Rakentaja();
		return rakentaja.Rakenna(parametrit, gameObject);
	}
}
