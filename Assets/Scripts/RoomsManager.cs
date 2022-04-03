using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
	[SerializeField] private Room _roomPrefab;

	[SerializeField] private int _gridSize = 5;
	[SerializeField] private int _borderSize = 3;
	[SerializeField] private float _roomSize = 10;

	[SerializeField] private float _moveAnimDuration = 5.0f;

	private List<Room> _rooms = null;

    void Start()
    {
	    _rooms = new List<Room>(_gridSize * _gridSize);

        Vector2 bottomLeftPos = new Vector2(_roomSize * -Mathf.Floor(_gridSize / 2.0f), _roomSize * -Mathf.Floor(_gridSize / 2.0f));
	    for (int x = -_borderSize; x < _gridSize + _borderSize; x++)
	    {
		    for (int y = -_borderSize; y < _gridSize + _borderSize; y++)
		    {
			    Vector2 roomPos = bottomLeftPos + new Vector2(_roomSize * x, _roomSize * y);
			    Room room = Instantiate(_roomPrefab, roomPos, Quaternion.identity, transform);
				if (x < 0 || x >= _gridSize || y < 0 || y >= _gridSize)
				{
					//Debug.Log(x +";" + y + " : " + Room.RoomType.NONE);
					room.GenerateRoom(new Vector2Int(x, y), Room.RoomType.NONE, false);
				}
				else
				{
					//Debug.Log(x + ";" + y + " : " + Room.RoomType.ALL);
					room.GenerateRoom(new Vector2Int(x, y), (Room.RoomType)Random.Range(2, 1 << 5), true);
					room.IsMoveable = true;
				}

				_rooms.Add(room);
			}
	    }
    }

	public void SwitchRoomsPos(Room room1, Room room2)
	{
		StartCoroutine(MovingAnimation(room1, room2));
	}

	private IEnumerator MovingAnimation(Room room1, Room room2)
	{
		room1.IsMoving = true;
		float timeElapsed = 0;
		Vector3 room1Pos = room1.transform.position;
		while (timeElapsed < _moveAnimDuration)
		{
			room1.transform.position = Vector3.Lerp(room1Pos, room2.transform.position, timeElapsed / _moveAnimDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		room1.IsMoving = false;
		room1.transform.position = room2.transform.position;
		room2.transform.position = room1Pos;
		room2.CloseRoom();
		Vector2Int room1Index = room1.RoomIndex;
		room1.RoomIndex = room2.RoomIndex;
		room2.RoomIndex = room1Index;

		room1.OnFinishMove();
		room2.OnFinishMove();
	}
}
