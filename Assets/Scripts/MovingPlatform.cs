﻿using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
	private float useSpeed;
	public float directionSpeed = 9.0f;
	float origX;

	float origY;
	public float distance = 10.0f;
	
	// Use this for initialization
	void Start ()
	{
		//origX = transform.position.x;
		origY = transform.position.y;
		useSpeed = -directionSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if(origX - transform.position.x > distance)

		if(origY - transform.position.y > distance)
		{
			useSpeed = directionSpeed; //flip direction
		}

		//else if(origX - transform.position.x < -distance)
		else if(origY - transform.position.y < -distance)
		{
			useSpeed = -directionSpeed; //flip direction
		}
		transform.Translate(0,useSpeed*Time.deltaTime,0);
	}
	
}