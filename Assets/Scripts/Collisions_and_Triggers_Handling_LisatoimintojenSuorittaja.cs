using UnityEngine;
using System.Collections;
using Target_ohjaus;

//tässä mallipohja lisätoimintojen suorittajalle
//ota tämä tiedosto kohdeprojektiin törmäys-keskus-prefabiin (prefab, jonka childeiksi lisätään törmäysmääritykset) ja toteuta ToiminnonSuorittaja-metodit eri törmäystapahtumille
public class Collisions_and_Triggers_Handling_LisatoimintojenSuorittaja : MonoBehaviour {
	//Collisions_and_Triggers_Handling_Hallintapaneelista_Rakentaja() kutsuu tätä (delegaatin välityksellä) ja saa delegaatin toiminnon suorittajaan
	//parametrina annetaan prefab, jossa törmäysparametrit annetaan, niin voidaan toteuttaa lisätoiminnot kaikille törmäyskokoonpanoille
	public Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling.ToiminnonSuorittaja Luo_ja_Palauta_Delegaatti_ToiminnonSuorittajaan(GameObject tormays_prefab) {
		Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling.ToiminnonSuorittaja toiminto=null;
		if(tormays_prefab.name=="tormays1")
			toiminto=ToiminnonSuorittaja1;
		else if(tormays_prefab.name=="tormays2")
			toiminto=ToiminnonSuorittaja2;
		return toiminto;
	}

	//toteuta tähän toiminnot
	public void ToiminnonSuorittaja1(Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling tormays_olio) {

	}

	public void ToiminnonSuorittaja2(Target_ohjaus.Target_ohjaus.Collisions_and_Triggers_Handling tormays_olio) {
		
	}
}