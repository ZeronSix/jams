using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{
	public GUITexture healthBar;
	public GUIText    timeText;
	public GUIText    stageText;
	public GUIText    ammoText;
	public GUITexture ammoIcon;
	public float      currentHealth;
	public float      currentTimer;
	public float      currentAmmo;

	private float _startHBWidth;

	void Start()
	{
		_startHBWidth = healthBar.pixelInset.width;
		stageText.text = (Application.loadedLevel + 1).ToString();
	}

	void OnGUI() 
	{
		Rect hb = healthBar.pixelInset;
		hb.width = _startHBWidth * currentHealth;
		healthBar.pixelInset = hb;

		timeText.text = currentTimer.ToString();
		ammoText.text = currentAmmo.ToString();
	}
}
