using UnityEngine;
using System.Collections;


public class playerController : MonoBehaviour
{
	
	private Animator animator;
	
	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update()
	{
		
		//var vertical = Input.GetAxis("Vertical");
		//var horizontal = Input.GetAxis("Horizontal");
		
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			animator.SetInteger("Direction", 0);
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
	
		{
			animator.SetInteger("Direction", 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			animator.SetInteger("Direction", 1);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			animator.SetInteger("Direction", 1);

		}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				animator.SetInteger("Direction", 3);
			}
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				animator.SetInteger("Direction", 0);
			}
		}
	}
