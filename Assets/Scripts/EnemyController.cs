using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : CharacterController
{
	private PlayerController _playerController = null;

	private Room _room = null;

	public Room Room
	{
		get { return _room; }
	}

	public override void Start()
	{
		base.Start();

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
			if (!_room.IsPlayerOccupied)
			{
				_room.ChangeType(Room.RoomType.NONE);
			}
		}
		_room = room;
		_room.IsEnemyOccupied = true;
	}
}