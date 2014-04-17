using UnityEngine;
using System.Collections;

public class Hit
{
	public float damage;
	public Transform transform;

	public Hit(float dmg, Transform tr)
	{
		damage = dmg;
		transform = tr;
	}
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStats))]
public class BaseCharacterController : MonoBehaviour 
{
	public float damageHitmarkerTime = 0.5f;
	public float meleeAttackForce   = 500f;

	protected CharacterStats _stats;
	protected float _hInput = 0; // horizontal input value for the character
	protected bool  _jInput = false;
	protected bool  _facingRight = true;
	protected bool  _grounded = false;
	protected bool  _colliding = false;
	protected Transform   _groundMarker;
	private   SpriteRenderer _sr;

	void Awake()
	{
		Init();
	}

	void OnCollisionStay2D(Collision2D col)
	{
		foreach (ContactPoint2D c in col.contacts)
		{
			if (c.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) 
			{
				if ((Mathf.Approximately(0, Vector2.Angle(c.normal, Vector2.right))
				     || Mathf.Approximately(180, Vector2.Angle(c.normal, Vector2.right))))
					_colliding = true;
			}
		}
	}

	void Update()
	{
		ProcessUpdate();
		_grounded = Physics2D.Linecast(transform.position, _groundMarker.position, 1 << LayerMask.NameToLayer("Ground"));
	}

	void FixedUpdate() 
	{ 
		float velocityX = _colliding ? 0f : _hInput * _stats.maxSpeed;;
		rigidbody2D.velocity = new Vector2(velocityX, rigidbody2D.velocity.y);

		if (_hInput > 0 && !_facingRight) Flip();
		else if (_hInput < 0 && _facingRight) Flip();

		if (_jInput && _grounded)
		{
			// process jumping
			rigidbody2D.AddForce(new Vector2(0f, _stats.jumpForce));
			_grounded = false;
			_jInput   = false;
		}
		_colliding = false;
		ProcessFixedUpdate();
	}

	private void Flip() 
	{
		_facingRight = !_facingRight;

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	protected virtual void ProcessFixedUpdate()
	{

	}

	protected virtual void ProcessUpdate()
	{
		// change input here
	}

	protected virtual void Init() 
	{
		_stats        = GetComponent<CharacterStats>();
		_groundMarker = transform.Find("groundMarker");
		_sr           = GetComponent<SpriteRenderer>();
	}

	public void Hurt(Hit hit)
	{
		_stats.health -= hit.damage;
		if (_stats.health <= 0)
			Die();
		rigidbody2D.AddForce(Vector2.right * -Mathf.Sign(hit.transform.position.x - transform.position.x) * meleeAttackForce + Vector2.up * meleeAttackForce);
		StartCoroutine("ShowMarker");
	}

	protected virtual void Die() 
	{

	}

	public IEnumerator ShowMarker() 
	{
		_sr.color = new Color(190, 0, 0);
		yield return new WaitForSeconds(damageHitmarkerTime);
		_sr.color = Color.white;
	}
}




