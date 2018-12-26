using UnityEngine;
using System.Collections;
using Target_ohjaus_Level_hallintaan;

//tällä määritetään Vaihda prefab asetukset tapahtuman rekisteröijälle
public class Vaihda_prefab_Maaritykset : MonoBehaviour {
	//inspector
	public Target_ohjaus_Level_hallintaan.Vaihda_prefab_Parametrit_Hallintapaneeliin parametrit;
	
	public Vaihda_prefab Rakenna() {
		Target_ohjaus_Level_hallintaan.Vaihda_prefab_Hallintapaneelista_Rakentaja rakentaja=new Target_ohjaus_Level_hallintaan.Vaihda_prefab_Hallintapaneelista_Rakentaja();
		return rakentaja.Rakenna(parametrit, gameObject);
		//clean!!!
		parametrit=null;
		rakentaja=null;
	}
}
