using UnityEngine;
using System.Collections;

public class Plaah : MonoBehaviour {

	public Transform camTarget;
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") Camera.main.transform.position = camTarget.position;
	}

}
