using UnityEngine;
using System.Collections;
//using SpriteFactory;

public class EnemyAi : MonoBehaviour {
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxDistance;
	//private Sprite sprite;
	
	private Transform myTransform;
	
	void Awake() {
		myTransform = transform;
		//sprite = (Sprite)GetComponent(typeof(Sprite));
	}
	
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		target = go.transform;
		
		//maxDistance = 8;
	}
	
	
	
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Vector3.Distance(target.position, myTransform.position) > maxDistance) {
			
			myTransform.position -= myTransform.right * moveSpeed * Time.deltaTime;{
				
				if (target.position.x < myTransform.position.x) myTransform.position -= myTransform.right * moveSpeed * Time.deltaTime; // player is left of enemy, move left
				
				else if (target.position.x > myTransform.position.x) myTransform.position += myTransform.right * moveSpeed * Time.deltaTime; // player is right of enemy, move right
				//sprite.Play ("EnemyWalk");{
				}
				
				//if(sprite.IsAnimationPlaying ("death")){
					
					
				}
				
				
				
				
			}
			
}