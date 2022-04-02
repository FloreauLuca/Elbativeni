using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomDetector : MonoBehaviour
{
	[SerializeField] private Room _room;

	private CameraManager _cameraManager = null;
	private BoxCollider2D _boxCollider2D = null;

	private void Start()
	{
		_cameraManager = FindObjectOfType<CameraManager>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_room.HasMoved += ForceTrigger;
		ForceTrigger();
	}

	public void ForceTrigger()
	{
		Collider2D[] collider2Ds = Physics2D.OverlapBoxAll((Vector2)transform.position + _boxCollider2D.offset, _boxCollider2D.size, transform.eulerAngles.z);

		foreach (Collider2D collider2D in collider2Ds)
		{
			OnTriggerEnter2D(collider2D);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_room.IsMoving)
			return;
		if (other.CompareTag("Player"))
		{
			if (_cameraManager.IsPlayerView)
			{
				_cameraManager.MoveCameraTo(_room.transform.position);
				other.GetComponent<PlayerController>().SetRoom(_room);
			}
		}

		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyController>().SetRoom(_room);
		}
	}
}
