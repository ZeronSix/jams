using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterStats))]
public class PlayerHealth : MonoBehaviour
{
	public float repeatDamagePeriod = 2f;
	public float meleeAttackDamage  = 10f;
	public float meleeAttackForce   = 100f;

	private CharacterStats _stats;
	private GameController _gc;
	private BoxCollider2D  _box;
	private float          _lastHitTime = 0;

	void Awake()
	{
		_stats = GetComponent<CharacterStats>();
		_box   = GetComponent<BoxCollider2D>();
		_gc = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Tags.PickupItem)
		{
			PickupItem item = other.GetComponent<PickupItem>();
			if (item.itemType == ItemType.HealthPack && _stats.health < _stats.maxHealth)
			{
				_stats.health = Mathf.Clamp(_stats.health + item.item.healthPack, _stats.health + item.item.healthPack, _stats.maxHealth);
			}
		}
	}

	void Update() 
	{
		_gc.SetUIHealth(_stats.health / _stats.maxHealth);

		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _box.size.x / 2 + 0.1f);
		foreach (Collider2D hit in hits)
		{
			if (hit.tag == Tags.Enemy)
			{
				if (Time.time > _lastHitTime + repeatDamagePeriod)
				{
					SendMessage("Hurt", new Hit(meleeAttackDamage, transform));
					_lastHitTime = Time.time;
				}
			}
		}
	}
}
