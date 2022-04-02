using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	private Rigidbody2D _rigidbody = null;
	private SpriteRenderer _playerSprite = null;

	[SerializeField] private float _speed = 1.0f;
	[SerializeField] private float _radius = 2.0f;

	private Vector2 _targetPos = Vector2.zero;
	private bool _moving = false;

	public virtual void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_playerSprite = GetComponentInChildren<SpriteRenderer>();
	}

	public virtual void Update()
	{
		if (_moving && (_targetPos - (Vector2)transform.position).sqrMagnitude < _radius)
		{
			_moving = false;
			_rigidbody.velocity = Vector2.zero;
		}
	}
	
	protected void MoveTo(Vector2 targetPos)
	{
		_targetPos = targetPos;
		Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
		_rigidbody.velocity = dir * _speed;
		_moving = true;
		LookAt(dir);
	}

	private void LookAt(Vector2 dir)
	{
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}