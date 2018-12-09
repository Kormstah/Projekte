using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float Speed = 6f;

	private Vector3 Movement;
	private Animator anim;
	private Rigidbody playerRigidbody;
	private int floorMask;
	private float camRayLength = 100f;

	void Awake()
	{
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();

	}

	void FixedUpdate()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Move(h, v);

		Turning();

		Animating(h, v);
	}

	void Move (float h, float v)
	{
		Movement.Set(h, 0f, v);
		Movement = Movement.normalized * Speed * Time.deltaTime;
		playerRigidbody.MovePosition(transform.position + Movement);
	}
	
	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;
		if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);

		}
	}

	void Animating (float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		anim.SetBool("IsWalking", walking);
	}
}
