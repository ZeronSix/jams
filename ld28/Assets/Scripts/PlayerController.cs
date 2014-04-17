using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : BaseCharacterController 
{
	public Weapon currentWeapon;

	private Animator  _anim;
	private Transform _bulletSpawnPoint;
	private float     _timer = 0f;
	private bool      _fInput = false;
	private GameController _gc;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == Tags.PickupItem)
		{
			if (Input.GetButtonDown("Use"))
			{
				PickupItem item = other.GetComponent<PickupItem>();
				if (item)
					PickupItem(item);
			}
		} else if (other.tag == Tags.Bomb) {
			_gc.TriggerEnd(false);
		}
	}

	protected override void Init()
	{
		base.Init();
		_anim             = GetComponent<Animator>();
		_bulletSpawnPoint = transform.Find("bulletSpawnPoint");
		_gc = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
	}
	
	protected override void ProcessUpdate()
	{
		_hInput = Input.GetAxis("Horizontal");
		_jInput = Input.GetButtonDown("Jump");
		_fInput = currentWeapon.isAutomatic ? Input.GetButton("Fire") : Input.GetButtonDown("Fire");
		if (_gc.ended) 
		{
			_hInput = 1;
		} else UpdateShooting(_fInput);
		UpdateAnimation(_hInput, _jInput);

		_timer -= Time.deltaTime;
		if (_timer <= 0)
			_timer = 0;
	}

	private void UpdateAnimation(float h, bool j) 
	{
		_anim.SetFloat("Speed", Mathf.Abs(h));
		if (j && _grounded)
			_anim.SetTrigger("Jump");
		_anim.SetBool("Grounded", _grounded);
	}

	private void UpdateShooting(bool f)
	{
		bool shouldFire = f && _timer == 0 && currentWeapon.currentAmmo > 0;
		if (shouldFire)
		{
			GameObject shell = Instantiate(currentWeapon.shell, _bulletSpawnPoint.position, Quaternion.identity) as GameObject;
			shell.rigidbody2D.AddForce(-Vector2.right * transform.localScale.x * 50 + Vector2.up * 60);
			Destroy(shell, 5f);
			GameObject bullet = Instantiate(currentWeapon.bullet, _bulletSpawnPoint.position, Quaternion.identity) as GameObject;
			Bullet bt = bullet.GetComponent<Bullet>();
			bt.SetVelocity(Vector2.right * transform.localScale.x * bt.speed);
			bt.damage = currentWeapon.damage;
			_timer = 1 / currentWeapon.rateOfFire; // 1 / RPM is frequency
			if (currentWeapon.firingSound)
				AudioSource.PlayClipAtPoint(currentWeapon.firingSound, transform.position);
			currentWeapon.currentAmmo--;
		} else if (f && _timer == 0 && currentWeapon.currentAmmo == 0) {
			if (currentWeapon.emptySound)
				AudioSource.PlayClipAtPoint(currentWeapon.emptySound, transform.position);
		}

		_gc.SetUIAmmo(currentWeapon.currentAmmo);
	}

	private void PickupItem(PickupItem item) 
	{
		switch (item.itemType)
		{
		case ItemType.Weapon:
			// change weapon
			SetWeapon(item.item.weapon);
			break;
		}
		Destroy(item.gameObject);
	}

	private void SetWeapon(Weapon weapon)
	{
		if (weapon != null)
		{
			currentWeapon = weapon;
			_bulletSpawnPoint.localPosition = currentWeapon.bulletSpawnPointPosition;
			_gc.SetUIAmmoIcon(currentWeapon.icon);
			GameObject wpn = Instantiate(currentWeapon.weapon, _bulletSpawnPoint.position + new Vector3(0, 0.05f), Quaternion.identity) as GameObject;
			wpn.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
			wpn.transform.parent = transform;
		}
	}

	protected override void Die()
	{
		_anim.SetFloat("Speed", 0);
		rigidbody2D.fixedAngle = false;
		rigidbody2D.AddTorque(-transform.localScale.x * 50);
		gameObject.layer = LayerMask.NameToLayer("Particles"); // player should not collide with anything
		_gc.TriggerEnd(true);
		enabled = false;
	}
}
