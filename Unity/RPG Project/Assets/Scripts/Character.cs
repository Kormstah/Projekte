using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

	[SerializeField]
	private float speed;

	Animator animator;

	protected Vector2 direction;

	// Use this for initialization
	protected virtual void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	protected virtual void Update()
	{

		Move();


	}
	public void Move()
	{
		transform.Translate(direction * speed * Time.deltaTime);

		if(direction.x != 0 || direction.y != 0)
		{
			AnimateMovement(direction);
		}
		else
		{
			animator.SetLayerWeight(1, 0);
		}

		
	}

	public void AnimateMovement(Vector2 dir)
	{
		animator.SetLayerWeight(1, 1);

		animator.SetFloat("x", dir.x);
		animator.SetFloat("y", dir.y);
	}
}

