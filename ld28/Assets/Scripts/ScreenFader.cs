using UnityEngine;
using System;
using System.Collections;

public class ScreenFader : MonoBehaviour 
{
	public float fadeSpeed;
	public float levelEndTimer = 2f;

	void Awake() 
	{
		guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
		guiTexture.color = Color.black;
	}

	public IEnumerator StartLevel()
	{
		while (guiTexture.color != Color.clear)
		{
			FadeToClear();
			yield return 0;
		}
	}

	public IEnumerator EndLevel(Action onEnd)
	{
		yield return new WaitForSeconds(levelEndTimer);
		float _timer = levelEndTimer;
		while (_timer >= 0)
		{
			FadeToBlack();
			_timer -= Time.deltaTime;
			yield return 0;
		}
		onEnd();
	}

	private void FadeToClear()
	{
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	private void FadeToBlack()
	{
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
}
