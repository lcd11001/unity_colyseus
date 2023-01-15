using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using Cinemachine;
using UnityEngine.Events;

public class PongMultiplayerManager : AttributesSync
{
	[SerializeField] private CinemachineVirtualCamera camPlayer1;
	[SerializeField] private CinemachineVirtualCamera camPlayer2;
	[SerializeField] private Spawner spawner;
	[SerializeField] private Transform spawnBallPosition;

	private GameObject ball;

	public delegate void OnBallCreated(BallController ball);
	public delegate void OnBallDestroyed(BallController ball);

	public static OnBallCreated onBallCreated;
	public static OnBallDestroyed onBallDestroyed;

	private void Start()
	{
		camPlayer1.gameObject.SetActive(false);
		camPlayer2.gameObject.SetActive(false);
	}

	public void OnOtherUserJoined(Multiplayer multiplayer, User user)
	{
		Debug.Log($"OnOtherUserJoined {user.Name} count {multiplayer.CurrentRoom.Users.Count}");
		if (multiplayer.CurrentRoom.Users.Count == 2)
		{
			Debug.Log("spawn by other");
			if (user.Index == 0)
			{
				//BroadcastRemoteMethod("SpawnBall");
				SpawnBall();
			}
		}
	}

	public void OnOtherUserLeft(Multiplayer multiplayer, User user)
	{
		Debug.Log($"OnOtherUserLeft {user.Name}");
		BroadcastRemoteMethod("DespawnBall");
	}

	public void OnRoomJoined(Multiplayer multiplayer, Room room, User me)
	{
		Debug.Log($"OnRoomJoined {room.Name} me {me.Name} count {multiplayer.CurrentRoom.Users.Count}");
		camPlayer1.gameObject.SetActive(me.Index == 0);
		camPlayer2.gameObject.SetActive(me.Index == 1);

		if (room.Users.Count == 2)
		{
			Debug.Log("spawn by me");
			if (me.Index == 0)
			{
				//BroadcastRemoteMethod("SpawnBall");
				SpawnBall();
			}
		}
	}

	public void OnRoomLeft(Multiplayer multiplayer)
	{
		Debug.Log($"OnRoomLeft");

		BroadcastRemoteMethod("DespawnBall");
	}

	public void OnSpawnedObject(User user, GameObject obj)
	{
		Debug.Log($"OnSpawnedObject {user.Name} obj {obj.name}");
		//onBallCreated?.Invoke(obj.GetComponent<BallController>());
	}

	public void OnDespawnedObject(User user)
	{
		Debug.Log($"OnDespawnedObject {user.Name}");
	}

	[SynchronizableMethod]
	public void SpawnBall()
	{
		if (ball == null)
		{
			ball = spawner.Spawn(0, spawnBallPosition.position);
			Debug.Log($"SpawnBall ball {ball.name}");
			onBallCreated?.Invoke(ball.GetComponent<BallController>());
		}
	}

	[SynchronizableMethod]
	public void DespawnBall()
	{
		if (ball != null)
		{
			Debug.Log($"DespawnBall ball {ball.name}");
			onBallDestroyed?.Invoke(ball.GetComponent<BallController>());
			spawner.Despawn(ball);
			ball = null;
		}
	}
}
