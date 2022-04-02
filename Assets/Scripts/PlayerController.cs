using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterController
{
	[SerializeField] private ParticleSystem _pointerParticleSystem = null;

    private Camera _camera = null;

    private CameraManager _cameraManager = null;

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
		_cameraManager.HasModeSwitched += StopMoving;
	}

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && _cameraManager.IsPlayerView)
        {
	        Vector2 pointerPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            MoveTo(pointerPos);
            DrawFxPointer(pointerPos);
		}

        base.Update();
    }

    private void DrawFxPointer(Vector2 pointerPos)
    {
        _pointerParticleSystem.Stop(true);
	    _pointerParticleSystem.transform.position = pointerPos;
        _pointerParticleSystem.Play(true);
    }

    public void SetRoom(Room room)
    {
	    if (_room != null)
	    {
		    _room.IsPlayerOccupied = false;

		    if (_room.IsEnemyOccupied)
		    {
                _room.ChangeType(Room.RoomType.NONE);
		    }
	    }

	    _room = room;
	    _room.IsPlayerOccupied = true;

        transform.SetParent(_room.transform);
    }
}
