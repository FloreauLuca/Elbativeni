using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
	private Vector2Int _roomIndex;

	/// <summary>
	/// Is part of the room set.
	/// </summary>
	private bool _isInteractible = false;

	public bool IsInteractible
	{
		get { return _isInteractible; }
	}

	/// <summary>
	/// Has not been moved before.
	/// </summary>
	private bool _isMoveable = false;

	public bool IsMoveable
	{
		get { return _isMoveable; }
		set
		{
			_isMoveable = value; 
			UpdateColors();
		}
	}

	/// <summary>
	/// Contain the enemy.
	/// </summary>
	private bool _isEnemyOccupied = false;

	public bool IsEnemyOccupied
	{
		get { return _isEnemyOccupied; }
		set { _isEnemyOccupied = value; }
	}

	/// <summary>
	/// Contain the player.
	/// </summary>
	private bool _isPlayerOccupied = false;

	public bool IsPlayerOccupied
	{
		get { return _isPlayerOccupied; }
		set { _isPlayerOccupied = value; }
	}

	private bool _isMoving = false;

	public bool IsMoving
	{
		get { return _isMoving; }
		set { _isMoving = value; }
	}

	public Vector2Int RoomIndex
	{
		get { return _roomIndex; }
		set { _roomIndex = value; }
	}

	private event Action _hasMoved = null;

	public event Action HasMoved
	{
		add
		{
			_hasMoved -= value;
			_hasMoved += value;
		}
		remove { _hasMoved -= value; }
	}

	[Flags] public enum RoomType
	{
		NONE = 1 << 0,
		TOP = 1 << 1,
		BOTTOM = 1 << 2,
		LEFT = 1 << 3,
		RIGHT = 1 << 4,
		ALL = TOP | BOTTOM | LEFT | RIGHT
	}

	private RoomType _myRoomType = RoomType.NONE;
	public RoomType MyRoomType
	{
		get { return _myRoomType; }
	}

	[SerializeField] private List<GameObject> _doors;

	[SerializeField] private Tilemap _spriteRenderer;

	public void GenerateRoom(Vector2Int pos, RoomType roomType, bool isInteractible)
	{
		_isMoveable = true;
		_isInteractible = isInteractible;
		_roomIndex = pos;
		_myRoomType = roomType;
		UpdateDoors();
		UpdateColors();
	}
	
	public void ChangeType(RoomType newRoomType)
	{
		_myRoomType = newRoomType;
		UpdateDoors();
		UpdateColors();
	}

	private void UpdateDoors()
	{
		foreach (GameObject door in _doors)
		{
			door.SetActive(true);
		}

		if (MyRoomType.HasFlag(RoomType.TOP))
		{
			_doors[0].SetActive(false);
		}
		if (MyRoomType.HasFlag(RoomType.BOTTOM))
		{
			_doors[1].SetActive(false);
		}
		if (MyRoomType.HasFlag(RoomType.LEFT))
		{
			_doors[2].SetActive(false);
		}
		if (MyRoomType.HasFlag(RoomType.RIGHT))
		{
			_doors[3].SetActive(false);
		}

	}

	private void UpdateColors()
	{
		if (!_isInteractible)
		{
			_spriteRenderer.color = Color.red;
		}
		else if (MyRoomType == RoomType.NONE)
		{
			_spriteRenderer.color = Color.black;
		}
		else if (!_isMoveable)
		{
			_spriteRenderer.color = Color.blue;
		}
		else
		{
			_spriteRenderer.color = Color.white;
		}
	}

	public void OnFinishMove()
	{
		_hasMoved.Invoke();
	}
}
