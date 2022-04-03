using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : CharacterController
{
	private PlayerController _playerController = null;
	private AudioSource _audioSource = null;

	private Room _room = null;

	public Room Room
	{
		get { return _room; }
	}

	public override void Start()
	{
		base.Start();

		_audioSource = GetComponent<AudioSource>();
		_playerController = FindObjectOfType<PlayerController>();
	}

	public override void Update()
	{
		MoveTo(_playerController.transform.position);

		base.Update();
	}

	public void SetRoom(Room room)
	{
		if (_room != null)
		{
			_room.IsEnemyOccupied = false;
		}
		_room = room;
		_room.IsEnemyOccupied = true;
		if (!_room.IsPlayerOccupied)
		{
			_room.CloseRoom();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			FindObjectOfType<EndPanel>().EndGame();
			_audioSource.Stop();
		}
	}
}