using UnityEngine;
using System.Collections;

/// <summary>
/// Ball behaviour.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Ball : MonoBehaviour 
{
	public float speed = 5f; 
	public Vector2 velocity;
	public AudioClip fall;

	private bool _isAwake = false;
	private bool _collidedLastFrame = false;
	private GameController _controller;
	private TextMesh _startText;

	void Awake() {
		_controller = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		_startText  = GameObject.FindGameObjectWithTag(Tags.startText).GetComponent<TextMesh>();
	}

	void OnCollisionEnter2D(Collision2D col) 
	{
		if (!_collidedLastFrame) 
		{
			velocity = Vector3.Reflect(velocity, col.contacts[0].normal);
			audio.Play();
			_collidedLastFrame = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		// check which border ball has been hit
		if (other.tag == Tags.leftBorder) 
		{
			_controller.ScoreRight();
		} else 
		{
			_controller.ScoreLeft();
		}
		AudioSource.PlayClipAtPoint(fall, transform.position);

		Destroy(gameObject);
	}

	void Update() {
		// if ball is not awake, show guitext 
		if (!_isAwake) 
		{
			_startText.text = Tags.RestartText;
			if (Input.GetButtonDown("Start"))
			{
				_isAwake = true;
				_startText.text = "";
			}
		} else {
			velocity = velocity.normalized;
			transform.Translate(velocity * Time.deltaTime * speed);
			_collidedLastFrame = false;
		}
	}
}
