using UnityEngine;
using System;

public class AIController : BaseCharacterController 
{
	public Weapon currentWeapon = null;
	public float moveMargin = 1f;
	public float rateOfFireMp = 0.1f;

	private  GameController _gc;
	private Animator  _anim;
	private Transform _bulletSpawnPoint;
	private float     _timer;

	protected override void Init()
	{
		base.Init();
		_anim             = GetComponent<Animator>();
		_bulletSpawnPoint = transform.Find("bulletSpawnPoint");
		_bulletSpawnPoint.localPosition = currentWeapon.bulletSpawnPointPosition;
		GameObject wpn = Instantiate(currentWeapon.weapon, _bulletSpawnPoint.position + new Vector3(0, 0.05f), Quaternion.identity) as GameObject;
		wpn.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
		wpn.transform.parent = transform;
		_gc = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
	}

	protected override void ProcessUpdate() 
	{
		base.ProcessUpdate();
		
		_timer -= Time.deltaTime;
		if (_timer <= 0)
			_timer = 0;
	}

	protected override void ProcessFixedUpdate()
	{
		base.ProcessFixedUpdate();
		if (_gc.ended) 
		{
			_hInput = 0;
			return;
		}

		// if player is in line of sight
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _stats.lineOfSightDistance);
		foreach (Collider2D col in hits)
		{
			if (col.tag == Tags.Player && _grounded)
			{
				float distance = col.transform.position.x - transform.position.x;
				Vector3 scale = transform.localScale;
				scale.x = Mathf.Sign(distance);;
				transform.localScale = scale;
				Action<float> behaviour;
				
				if (currentWeapon.currentAmmo == 0)
				{
					behaviour = UnarmedBehavior;
				} else {
					behaviour = ArmedBehaviour;
				}
				behaviour(distance);
				break;
			}
		}
	}

	private void UnarmedBehavior(float distance) 
	{
		if (Mathf.Abs(distance) > moveMargin)
		{
			_hInput = Mathf.Sign(distance);
		}
		else _hInput = 0;

		if (!CheckForEdge(_hInput))
			_hInput = 0;

		_anim.SetFloat("Speed", Mathf.Abs(_hInput));
		_anim.SetBool("Grounded", _grounded);
	}

	private void ArmedBehaviour(float distance)
	{
		// if player is directly on a left or right side
		if (Physics2D.Raycast(_bulletSpawnPoint.position, Vector2.right * Mathf.Sign(distance), distance))
		{
			UpdateShooting();
		}
	}

	private void UpdateShooting()
	{
		bool shouldFire = _timer == 0 && currentWeapon.currentAmmo > 0;
		if (shouldFire)
		{
			GameObject shell = Instantiate(currentWeapon.shell, _bulletSpawnPoint.position, Quaternion.identity) as GameObject;
			shell.rigidbody2D.AddForce(-Vector2.right * transform.localScale.x * 50 + Vector2.up * 60);
			Destroy(shell, 5f);
			GameObject bullet = Instantiate(currentWeapon.bullet, _bulletSpawnPoint.position, Quaternion.identity) as GameObject;
			Bullet bt = bullet.GetComponent<Bullet>();
			bt.SetVelocity(Vector2.right * transform.localScale.x * bt.speed);
			bt.isPlayer = false;
			bt.damage = currentWeapon.damage;
			_timer = 1 / currentWeapon.rateOfFire / rateOfFireMp; // 1 / RPM is frequency
			if (currentWeapon.firingSound)
				AudioSource.PlayClipAtPoint(currentWeapon.firingSound, transform.position);
			currentWeapon.currentAmmo--;
		}
	}

	private bool CheckForEdge(float sign) 
	{
		return sign != 0 ? Physics2D.Raycast(_groundMarker.position, Vector2.right * sign, 2f) : false;
	}

	protected override void Die()
	{
		tag = "Untagged";
		rigidbody2D.fixedAngle = false;
		rigidbody2D.AddTorque(-transform.localScale.x * 50);
		gameObject.layer = LayerMask.NameToLayer("Particles");
		_anim.SetFloat("Speed", 0);
		// instantiate pickup item
		enabled = false;
	
	}
}
