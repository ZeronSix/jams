using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform player;
	public float     smoothness = 5f;

	private GameController _gc;

	void Awake() 
	{
		_gc = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
	}

	void Update() 
	{
		if (!_gc.ended) 
		{
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, smoothness * Time.deltaTime), 
			                                 transform.position.y, transform.position.z);
		}
	}
}
