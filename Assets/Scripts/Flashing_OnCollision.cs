using UnityEngine;
using System.Collections;

//välähtää kun osuu
public class Flashing_OnCollision : MonoBehaviour {
	public string other_tag;
	public float vilkkumisaika=0.1f;

	//void OnCollisionEnter (Collision other)
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag.CompareTo(other_tag)==0) {
			GetComponent<Renderer>().enabled = false;
			Invoke("AsetaTrue", vilkkumisaika);
		}
	}

	void AsetaTrue() {
		GetComponent<Renderer>().enabled = true;
	}
}