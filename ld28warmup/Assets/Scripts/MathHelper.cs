using UnityEngine;
using System.Collections;

/// <summary>
/// Class with useful math functions
/// </summary>
public class MathHelper 
{
	public static bool RandBool()
	{
		return Random.Range(-1, 1) >= 0 ? true : false;
	}
}
