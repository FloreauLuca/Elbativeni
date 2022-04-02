using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : CharacterController
{
	private PlayerController _playerController = null;

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
}