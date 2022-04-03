using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterController
{
	[SerializeField] private Animator _pointer = null;

    private Camera _camera = null;

    private CameraManager _cameraManager = null;
    private Animator _playerAnimator = null;

    private Room _room = null;

    public Room Room
    {
	    get { return _room; }
    }

    public override void Start()
	{
        base.Start();
        
        _camera = Camera.main;
		_cameraManager = FindObjectOfType<CameraManager>();
		_playerAnimator = GetComponentInChildren<Animator>();

        _cameraManager.HasModeSwitched += OnModeSwitch;
	}

    public void OnModeSwitch()
    {
	    StopMoving();
	    _pointer.gameObject.SetActive(_cameraManager.IsPlayerView);
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && _cameraManager.IsPlayerView)
        {
	        Vector2 pointerPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            MoveTo(pointerPos);
            DrawFxPointer(pointerPos);
		}

        _playerAnimator.SetBool("Walk", _rigidbody.velocity.magnitude > 0.1f);

        base.Update();
    }

    private void DrawFxPointer(Vector2 pointerPos)
    {
        _pointer.transform.position = pointerPos;
        _pointer.SetTrigger("Point");
    }

    public void SetRoom(Room room)
    {
	    if (_room != null)
	    {
		    _room.IsPlayerOccupied = false;

		    if (_room.IsEnemyOccupied)
		    {
                _room.CloseRoom();
		    }
	    }

	    _room = room;
	    _room.IsPlayerOccupied = true;

        transform.SetParent(_room.transform);
    }
}
