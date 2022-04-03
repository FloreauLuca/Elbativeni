using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
	private CameraManager _cameraManager = null;
	private RoomsManager _roomsManager = null;
	private SpriteRenderer _spriteRenderer;

	private static RoomController _selectedController = null;
	[SerializeField] private Room _room = null;

	public Room Room
	{
		get { return _room; }
	}

	[SerializeField] private Color _selectedColor;
	private Color _startColor = Color.clear;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_startColor = _spriteRenderer.color;

		_cameraManager = FindObjectOfType<CameraManager>();
		_cameraManager.HasModeSwitched += Deselect;

		_roomsManager = FindObjectOfType<RoomsManager>();
	}

	private void OnMouseDown()
	{
		if (_cameraManager.IsPlayerView || !_room.IsInteractible)
			return;
		if (_selectedController == null)
		{
			if (_room.IsMoveable)
			{
				_selectedController = this;
				_spriteRenderer.color = _selectedColor;
			}
		}
		else if (_selectedController == this)
		{
			_selectedController.Deselect();
		} 
		else
		{
			if (!_room.IsPlayerOccupied && _room.IsMoveable)
			{
				_roomsManager.SwitchRoomsPos(_selectedController.Room, _room);
				_selectedController.Room.IsMoveable = false;
				_selectedController.Deselect();
			}
		}
	}
	
	public void Deselect()
	{
		_selectedController = null;
		_spriteRenderer.color = _startColor;
	}
}
