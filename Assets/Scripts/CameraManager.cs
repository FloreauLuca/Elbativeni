using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private bool isPlayerView = true;

	public bool IsPlayerView
	{
		get { return isPlayerView; }
	}

	private event Action _hasModeSwitched = null;

	public event Action HasModeSwitched
	{
		add
		{
			_hasModeSwitched -= value;
			_hasModeSwitched += value;
		}
		remove { _hasModeSwitched -= value; }
	}


	[SerializeField] private float _moveAnimDuration = 5.0f;

	[SerializeField] private Camera _camera;

	[SerializeField] private float mapFov = 35.0f;
	[SerializeField] private float playerFov = 7.5f;

	private PlayerController _playerController = null;

	private void Start()
	{
		_playerController = FindObjectOfType<PlayerController>();
	}

	public void SwitchMode()
	{
		if (isPlayerView)
		{
			isPlayerView = false;
			_hasModeSwitched.Invoke();
			StartCoroutine(ZoomingAnimation(Vector2.zero, mapFov));
		}
		else
		{
			isPlayerView = true;
			_hasModeSwitched.Invoke();
			StartCoroutine(ZoomingAnimation(_playerController.Room.transform.position, playerFov));
		}
	}

	public void MoveCameraTo(Vector2 pos)
	{
		StartCoroutine(MovingAnimation(pos));
	}

	private IEnumerator MovingAnimation(Vector3 newPos)
	{
		float timeElapsed = 0;
		Vector3 startPos = _camera.transform.position;
		newPos.z = startPos.z;
		while (timeElapsed < _moveAnimDuration)
		{
			_camera.transform.position = Vector3.Lerp(startPos, newPos, timeElapsed / _moveAnimDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		_camera.transform.position = newPos;
	}

	private IEnumerator ZoomingAnimation(Vector3 newPos, float newFov)
	{
		float timeElapsed = 0;
		Vector3 startPos = _camera.transform.position;
		newPos.z = startPos.z;
		float oldFov = _camera.orthographicSize;
		while (timeElapsed < _moveAnimDuration)
		{
			_camera.transform.position = Vector3.Lerp(startPos, newPos, timeElapsed / _moveAnimDuration);
			_camera.orthographicSize = Mathf.Lerp(oldFov, newFov, timeElapsed / _moveAnimDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		_camera.orthographicSize = newFov;
		_camera.transform.position = newPos;
	}
}
