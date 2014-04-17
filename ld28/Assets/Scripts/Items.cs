using UnityEngine;
using System.Collections;

public enum ItemType 
{
	Weapon,
	HealthPack,
	Bomb
}

[System.Serializable]
public class Item
{
	public ItemType type;
	public float    healthPack;
	public Weapon   weapon;
}

public enum WeaponType
{
	Pistol = 1
}

[System.Serializable]
public class Weapon
{
	public WeaponType type;
	public Texture2D  icon;
	public bool       isAutomatic;
	public float      damage;
	public float      rateOfFire;
	public float      startAmmo;
	public float      currentAmmo;
	public GameObject weapon;
	public GameObject bullet;
	public GameObject shell;
	public Vector2    bulletSpawnPointPosition;
	public AudioClip  firingSound;
	public AudioClip  emptySound;

	// TODO: sounds
}

