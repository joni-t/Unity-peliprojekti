using UnityEngine;
using System.Collections;



public class Done_BGScroller : MonoBehaviour
{
	public float speed = 0;

//	private float _mod = 0f;
//	private float _modStep = 0.01f;
//	private float _textureWidth;

//	public static Done_BGScroller current;
//	float pos = 0;

void Start () {
//		current = this;

//		_textureWidth = gameObject.transform.localScale.x;

	}
void Update () {

//		renderer.material.mainTextureOffset = new Vector2(_mod * speed, 0f);
//		_mod = _mod + _modStep;
//		if (_mod > _textureWidth) _mod = 0;





//			pos +=speed;
//			if(pos > 1.0f)
//				pos -= -1.0f;
//
//
//		renderer.material.mainTextureOffset = new Vector2(pos, 0);


		GetComponent<Renderer>().material.mainTextureOffset = new Vector2((Time.time * speed) % 1, 0f);

}﻿
}