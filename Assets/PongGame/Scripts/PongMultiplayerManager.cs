using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using Cinemachine;

public class PongMultiplayerManager : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera camPlayer1;
	[SerializeField] private CinemachineVirtualCamera camPlayer2;

	private void Start()
	{
		camPlayer1.gameObject.SetActive(false);
		camPlayer2.gameObject.SetActive(false);
	}

	public void OnOtherUserJoined(Multiplayer multiplayer, User user)
	{
		Debug.Log($"OnOtherUserJoined {user.Name}");
	}

	public void OnOtherUserLeft(Multiplayer multiplayer, User user)
	{
		Debug.Log($"OnOtherUserLeft {user.Name}");
	}

	public void OnRoomJoined(Multiplayer multiplayer, Room room, User me)
	{
		Debug.Log($"OnRoomJoined {room.Name} me {me.Name}");
		camPlayer1.gameObject.SetActive(me.Index == 0);
		camPlayer2.gameObject.SetActive(me.Index == 1);
	}

	public void OnRoomLeft(Multiplayer multiplayer)
	{
		Debug.Log($"OnRoomLeft");
	}
}
