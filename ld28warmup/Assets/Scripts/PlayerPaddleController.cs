using UnityEngine;
using System.Collections;

/// <summary>
/// Paddle controller for the player.
/// </summary>
public class PlayerPaddleController : PaddleController 
{
	protected override void UpdateMovement() 
	{
		_movement = Input.GetAxis("Mouse Y");
	}
}
