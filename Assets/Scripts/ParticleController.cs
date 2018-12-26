using UnityEngine;
using System.Collections;



public class ParticleController : MonoBehaviour {



	void Start () {
		// You can use particleSystem instead of
		// gameObject.particleSystem.
		// They are the same, if I may say so
		  
	}
	
	void Update () {
		if( Input.GetKey( KeyCode.UpArrow ) ) {
			GetComponent<ParticleSystem>().enableEmission = true ;
		}
		else 
		{
			GetComponent<ParticleSystem>().enableEmission = false;
		}
	}
}