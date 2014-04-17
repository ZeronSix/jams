using UnityEngine;

/// <summary>
/// Base script for any paddle controlling.
/// </summary>
public class PaddleController : MonoBehaviour 
{
	public float speed = 5f; 

	protected float _movement; // translation on Y axis
	private float _startX;

	void Awake() {
		_startX = transform.position.x;
	}

	void FixedUpdate() 
	{
		// apply force using our movement value
		UpdateMovement();
		rigidbody2D.velocity = new Vector2(0, _movement * speed);
	}

	void Update() {
		transform.position = new Vector3(_startX, transform.position.y, 0);
	}
	
	protected virtual void UpdateMovement() 
	{
		// do moving code here
	}
}
