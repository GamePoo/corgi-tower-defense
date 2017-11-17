using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {

	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.enabled = false;
	}

	void FixedUpdate ()
	{
		FollowMouse ();
	}

	private void FollowMouse()
	{
		transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
	}

	public void Activate(Sprite sprite)
	{
		spriteRenderer.enabled = true;
		this.spriteRenderer.sprite = sprite;
	}

	public void Deactivate()
	{
		spriteRenderer.enabled = false;
	}

}
