using UnityEngine;
using System.Collections;

/// <summary>
/// Game controller.
/// </summary>
public class GameController : MonoBehaviour 
{
	public GameObject ball;             // ball prefab
	public TextMesh    leftScore;
	public TextMesh    rightScore;
	public TextMesh   restartText;

	public float minAngle = 30;
	public float maxAngle = 40;  // ball will have angle with less that |angleConstraint| value
	public float ballPosConstraint = 2.5f; // a bound for the ball instantiate position
	public string winText = "WIN!";
	public string failText = "FAIL :C";

	private int _leftScore  = 0;
	private int _rightScore = 0;
	private bool _isGameEnded = false;

	void Awake() 
	{
		if (ball)
			InstantiateBall(true);
	}

	void Update() {
		Screen.showCursor = false;
		if (_isGameEnded) {
			if (Input.GetButtonDown("Start"))
			{
				Application.LoadLevel(Application.loadedLevel); // restart
			}
		} 
	}

	private void InstantiateBall(bool isDirectionRight) 
	{
		// if isDirectionRight is true - ball will fry to the right side (if right player wins)
		Vector3    ballPosition = new Vector3(0, Random.Range(-ballPosConstraint, ballPosConstraint), 0);
		float      angle        = Random.Range(minAngle, maxAngle);
				   angle        = MathHelper.RandBool() ? angle : 360 - angle;  
		Quaternion ballRotation = Quaternion.Euler(0, 0, angle);
		Vector2    velocity     = new Vector2();
		if (isDirectionRight) // calculate ball's velocity
		{
			velocity = ballRotation * Vector2.right;
		} else {
			velocity = ballRotation * -Vector2.right;
		}

		GameObject newBall = Instantiate(ball, ballPosition, Quaternion.identity) as GameObject;
		newBall.GetComponent<Ball>().velocity = velocity;
	}

	public void ScoreLeft() 
	{
		_leftScore++;
		if (_leftScore < 10) 
		{
			leftScore.text = _leftScore.ToString();
			InstantiateBall(false);
		} else {
			leftScore.text = winText;
			rightScore.text = failText;
			restartText.text = Tags.RestartText;
			_isGameEnded = true;
		}
	}

	public void ScoreRight() 
	{
		_rightScore++;
		if (_rightScore < 10)
		{
			rightScore.text = _rightScore.ToString();
			InstantiateBall(true);
		} else {
			rightScore.text = winText;
			leftScore.text = failText;
			restartText.text = Tags.RestartText;
			_isGameEnded = true;
		}
	}
}
