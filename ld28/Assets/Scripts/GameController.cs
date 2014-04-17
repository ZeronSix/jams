using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
	public float levelTime = 60.0f;
	public bool  ended = false;

	private ScreenFader _fader;
	private GameUI      _ui;
	private float       _timer;

	void Awake() 
	{
		_timer = levelTime;
		_ui = GetComponent<GameUI>();
		_fader = GameObject.FindGameObjectWithTag(Tags.ScreenFader).GetComponent<ScreenFader>();
		StartCoroutine(_fader.StartLevel());
		SetupCollisionLayers();
	}

	void Update()
	{
		// TODO: white explosion
		_timer -= Time.deltaTime;
		if (_timer <= 0)
		{
			_timer = 0;
			TriggerEnd(true);
		}

		_ui.currentTimer = Mathf.Floor(_timer);
	}

	public void SetUIHealth(float ratio)
	{
		_ui.currentHealth = ratio;
	}

	public void SetUIAmmo(float ammo)
	{
		_ui.currentAmmo = ammo;
	}

	public void SetUIAmmoIcon(Texture2D icon)
	{
		_ui.ammoIcon.texture = icon;
	}

	public void TriggerEnd(bool fail)
	{
		ended = true;
		Action onFinish;
		if (fail) 
		{
			onFinish = () => {
				Application.LoadLevel(Application.loadedLevel);
			};
		} else {
			onFinish = () => {
				if (Application.loadedLevel != Application.levelCount - 1)
					Application.LoadLevel(Application.loadedLevel + 1);
				else
					Application.LoadLevel(0);
			};
		}
		_fader.StartCoroutine(_fader.EndLevel(onFinish));
		enabled = false;
	}

	private void SetupCollisionLayers()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Particles"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Particles"), LayerMask.NameToLayer("Particles"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Particles"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Character"));
	}
}

