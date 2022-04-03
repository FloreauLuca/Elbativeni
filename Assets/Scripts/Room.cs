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
		set { 
			_isEnemyOccupied = value;
			UpdateColors();
		}
	}

	/// <summary>
	/// Contain the player.
	/// </summary>
	private bool _isPlayerOccupied = false;

	public bool IsPlayerOccupied
	{
		get { return _isPlayerOccupied; }
		set {
			_isPlayerOccupied = value;
			UpdateColors();
		}
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
	[SerializeField] private SpriteRenderer _fog;
	[SerializeField] private Sprite _fogSprite;
	[SerializeField] private Sprite _fogPlayer;
	[SerializeField] private float _fogAlphaPlayer = 0.75f;
	[SerializeField] private float _fogAlphaMap = 0.5f;
	[SerializeField] private float _doorFadeDuration = 0.5f;
	[SerializeField] private Animator _landslideAnimator;
	[SerializeField] private GameObject _lockSprite = null;

	private CameraManager _cameraManager;
	private AudioSource _audioSource;

	public void GenerateRoom(Vector2Int pos, RoomType roomType, bool isInteractible)
	{
		_isMoveable = true;
		_isInteractible = isInteractible;
		_roomIndex = pos;
		_myRoomType = roomType;
		
		_cameraManager = FindObjectOfType<CameraManager>();
		_cameraManager.HasModeSwitched += OnModeSwitch;
		_audioSource = GetComponent<AudioSource>();

		UpdateDoors();
		UpdateColors();
	}
	
	public void CloseRoom()
	{
		if (_myRoomType != RoomType.NONE)
		{
			ChangeType(RoomType.NONE);
			_landslideAnimator.SetTrigger("Fall");
			_audioSource.Play();
		}
	}

	public void ChangeType(RoomType newRoomType)
	{
		_myRoomType = newRoomType;
		UpdateDoors();
		UpdateColors();
	}

	private void UpdateDoors()
	{
		if (!MyRoomType.HasFlag(RoomType.TOP))
		{
			_doors[0].SetActive(true);
			StartCoroutine(Fade(1.0f, _doors[0].GetComponentInChildren<SpriteRenderer>()));
		}
		else
		{
			_doors[0].SetActive(false);
			StartCoroutine(Fade(0.0f, _doors[0].GetComponentInChildren<SpriteRenderer>()));
		}

		if (!MyRoomType.HasFlag(RoomType.BOTTOM))
		{
			_doors[1].SetActive(true);
			StartCoroutine(Fade(1.0f, _doors[1].GetComponentInChildren<SpriteRenderer>()));
		}
		else
		{
			_doors[1].SetActive(false);
			StartCoroutine(Fade(0.0f, _doors[1].GetComponentInChildren<SpriteRenderer>()));
		}

		if (!MyRoomType.HasFlag(RoomType.LEFT))
		{
			_doors[2].SetActive(true);
			StartCoroutine(Fade(1.0f, _doors[2].GetComponentInChildren<SpriteRenderer>()));
		}
		else
		{
			_doors[2].SetActive(false);
			StartCoroutine(Fade(0.0f, _doors[2].GetComponentInChildren<SpriteRenderer>()));
		}

		if (!MyRoomType.HasFlag(RoomType.RIGHT))
		{
			_doors[3].SetActive(true);
			StartCoroutine(Fade(1.0f, _doors[3].GetComponentInChildren<SpriteRenderer>()));
		}
		else
		{
			_doors[3].SetActive(false);
			StartCoroutine(Fade(0.0f, _doors[3].GetComponentInChildren<SpriteRenderer>()));
		}
	}

	public IEnumerator Fade(float newAlpha, SpriteRenderer spriteRenderer)
	{
		float timeElapsed = 0;
		float oldAlpha = spriteRenderer.color.a;
		while (timeElapsed < _doorFadeDuration)
		{
			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(oldAlpha, newAlpha, timeElapsed / _doorFadeDuration);
			spriteRenderer.color = color;
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		Color colorFinal = spriteRenderer.color;
		colorFinal.a = newAlpha;
		spriteRenderer.color = colorFinal;
	}

	private void OnModeSwitch()
	{
		Color fogColor = _fog.color;
		if (_cameraManager.IsPlayerView)
		{
			fogColor.a = _fogAlphaPlayer;
		}
		else
		{
			fogColor.a = _fogAlphaMap;
		}
		_fog.color = fogColor;
		UpdateColors();
	}

	private void UpdateColors()
	{
		if ((!_isInteractible || !_isMoveable) && !_cameraManager.IsPlayerView)
		{
			_lockSprite.SetActive(true);
		}
		else
		{
			_lockSprite.SetActive(false);
		}

		if (MyRoomType == RoomType.NONE)
		{
			_spriteRenderer.color = Color.black;
		}
		
		if (IsPlayerOccupied)
		{
			_fog.sprite = _fogPlayer;
		}
		else
		{
			_fog.sprite = _fogSprite;
		}
	}

	public void OnFinishMove()
	{
		_hasMoved.Invoke();
	}
}
