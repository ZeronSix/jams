using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	public float speed;
	public float damage;
	public bool  isPlayer = true;

	void OnCollisionEnter2D(Collision2D col)
	{
		if ((col.gameObject.tag == Tags.Player && !isPlayer) || (col.gameObject.tag == Tags.Enemy && isPlayer))
		{
			col.gameObject.SendMessage("Hurt", new Hit(damage, transform), SendMessageOptions.DontRequireReceiver);
		}
		Destroy(gameObject);
	}
	
	public void SetVelocity(Vector2 velocity)
	{
		rigidbody2D.velocity = velocity;
	}
}

