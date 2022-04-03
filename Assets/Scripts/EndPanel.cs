using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{

	[SerializeField] private UnityEvent _onEndDisplay = null;
	[SerializeField] private TextMeshProUGUI _endTimer = null;
	[SerializeField] private Timer _timer = null;
	private AudioSource _audioSource = null;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void EndGame()
	{
		Time.timeScale = 0.0f;
		_audioSource.Play();
		_timer.Pause = true;
		_onEndDisplay.Invoke();
		_endTimer.text = TimeSpan.FromSeconds(_timer.CurrentTime).ToString(@"mm\:ss\.fff");
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("MenuScene");
	}
}
