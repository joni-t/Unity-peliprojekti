using UnityEngine;
using System.Collections;
using Ohjaus_2D;
using Ohjaus_laite;

public class Bullet : MonoBehaviour {
	//Attach to empty object, in front of barrel
	
	public Rigidbody projectile; //The Bullet	
	public float speed = 250f; //Speed Of The Bullets	
	public Rect ampumisnappula=new Rect(0, 100, 100, 30);
	public GUIStyle nappula_style;
	public Ohjaus_laite.Ohjaus_laite ampumisen_ohjauslaite;
	


	// Use this for initialization
	void Start () {
		Debug.Log("bullet");
		ampumisen_ohjauslaite=new Ohjaus_laite.Ohjaus_laite(gameObject.name);
		ampumisen_ohjauslaite.Lisaa_Ohjausnappi(GUI.RepeatButton, new GUIContent("Ammu! Ammuu"), ampumisnappula, "Ammu", nappula_style, true, "s", Fire);
	}
	
	// Update is called once per frame
//	void Update () {
//		//InvokeRepeating("Fire",fireRate,0); //Shoot Dela
//	}

	public void Fire(Ohjaustapahtuma kosketus) {
		
		//audio.Play();
		Debug.Log("fire");
		Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation ) as Rigidbody;

		
		instantiatedProjectile.velocity =
			
			transform.TransformDirection(new Vector3( speed, 0, 0 ) );
		
		//instantiatedFlash.velocity =
		
		//transform.TransformDirection( Vector3( 0, 0, 0 ) );
		
		
		Physics.IgnoreCollision( instantiatedProjectile. GetComponent<Collider>(),
		                        
		                        transform.root.GetComponent<Collider>() );
	}
}
