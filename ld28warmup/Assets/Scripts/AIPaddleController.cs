using UnityEngine;
using System.Collections;

/// <summary>
/// AI paddle controller.
/// </summary>
public class AIPaddleController : PaddleController 
{
	public float reactionTime = 10f;

	private GameObject _ball;

	void Start() 
	{
		_ball = GameObject.FindGameObjectWithTag(Tags.ball);
	}
	
	protected override void UpdateMovement() 
	{
		if (_ball) 
		{ // check for ball again, it could be deleted
			if (_ball.transform.position.x < 0) 
			{
				float movementTarget = 0f;
				
				movementTarget = Mathf.Min(Mathf.Abs(_ball.transform.position.y - transform.position.y), speed);
				if (_ball.transform.position.y < transform.position.y)
					movementTarget = -movementTarget;
				_movement = Mathf.Lerp(_movement, movementTarget, reactionTime * Time.deltaTime);
			} else _movement = 0;
		} else {
			_ball = GameObject.FindGameObjectWithTag(Tags.ball);
		}
	}
}