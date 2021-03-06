using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private PlayableDirector _playableDirector;

	private void Start()
	{
		Time.timeScale = 1.0f;
	}

	public void Play()
	{
		_playableDirector.Play();
		_playableDirector.stopped += LoadScene;
	}

	private void LoadScene(PlayableDirector playableDirector)
	{
		SceneManager.LoadScene("SampleScene");
	}
}
