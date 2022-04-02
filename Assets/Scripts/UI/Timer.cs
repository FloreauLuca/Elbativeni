using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
	private TextMeshProUGUI _timerText;

	private double _time = 0.0f;

	private void Start()
	{
		_timerText = GetComponent<TextMeshProUGUI>();
		_time = 0.0f;
	}

	private void LateUpdate()
	{
		_time += Time.deltaTime;
		_timerText.text = TimeSpan.FromSeconds(_time).ToString(@"mm\:ss\.fff");
	}
}
