using UnityEngine;
using System.Collections;

/// <summary>
/// Menu controller.
/// </summary>
public class MainMenu : MonoBehaviour 
{
	void Update() 
	{
		if (Input.GetButtonDown("Start"))
			Application.LoadLevel(1);
	}
}
