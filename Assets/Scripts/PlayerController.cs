using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterController
{
	[SerializeField] private ParticleSystem _pointerParticleSystem = null;

    private Camera _camera = null;

    public override void Start()
	{
        base.Start();

		_camera = Camera.main;
	}
    
    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
}
