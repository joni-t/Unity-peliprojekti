using UnityEngine;
using System.Collections;
using Artificial_Intelligence;
using Menu;

public class LiikkuvaHissiMaaritykset : MonoBehaviour {
	//inspector
	public Artificial_Intelligence.LiikkuvaHissiHallinta_Parametrit_Hallintapaneeliin hissi_asetukset;
	
	[HideInInspector]
	public LiikkuvaHissiHallinta hissi_hallinta;
	
	// Use this for initialization
	void Start () {
		Debug.Log("LiikkuvaHissiMaaritykset: " + gameObject.name);

		hissi_hallinta=new LiikkuvaHissiHallinta_Hallintapaneelista_Rakentaja().Rakenna(hissi_asetukset, gameObject);

		//clean!!!
		//hissi_asetukset=null;
		//hissi_hallinta=null;
	}
}
