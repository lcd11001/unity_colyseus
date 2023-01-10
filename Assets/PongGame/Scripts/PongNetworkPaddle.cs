using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PongNetworkPaddle : PaddleController
{
	[SerializeField] private string playerID;
	[SerializeField] private float timeIntervalSyncPosition = 1;

	private PongNetworkManager _networkManager;
	private float timeSyncPosition = 0;

	private bool isSyncPosition = false;
	private float targetSyncPosition = 0;
	private float targetSyncSpeed = 0;
	private Queue<PongSyncPosition> queueSyncPosition = new Queue<PongSyncPosition>();
	private float timeEnqueue = 0;

	public string PlayerID
	{
		get
		{
			return playerID;
		}
		set
		{
			playerID = value;
		}
	}

	public bool IsMultiplayerMode => (_networkManager == null || _networkManager.GameRoom == null) ? false : true;
	public bool IsLocalPlayer => IsMultiplayerMode ?_networkManager.GameRoom.SessionId == PlayerID : false;
	public bool IsOwner(string sectionID) => IsMultiplayerMode ? sectionID == PlayerID : false;

	private void Awake()
	{
		_networkManager = FindObjectOfType<PongNetworkManager>();
	}

	private void OnEnable()
	{
		RegisterEvents();
	}

	private void OnDisable()
	{
		UnregisterEvents();
	}

	private void RegisterEvents()
	{
		PongNetworkManager.OnPositionChanged += PongNetworkManager_OnPositionChanged;
	}

	private void UnregisterEvents()
	{
		PongNetworkManager.OnPositionChanged -= PongNetworkManager_OnPositionChanged;
	}

	private void PongNetworkManager_OnPositionChanged(string id, PongPlayer player)
	{
		//Debug.Log($"OnPositionChanged {id} pos {player.pos}");
		if (IsOwner(id))
		{
			isSyncPosition = true;
			queueSyncPosition.Enqueue(new PongSyncPosition { pos = -player.pos, time = Time.time - timeEnqueue });

			timeEnqueue = Time.time;
		}
	}

	public void InitPosition(Vector3 myPosition, Vector3 opponentPosition)
	{
		transform.position = IsLocalPlayer ? myPosition : opponentPosition;

		targetSyncPosition = transform.position.x;

		MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
		if (mr != null)
		{
			mr.material.color = IsLocalPlayer ? Color.blue : Color.red;
		}
	}

	protected override void Movement()
	{
		if (!IsLocalPlayer)
		{
			/*
			if (isSyncPosition
				&& this.transform.position.x != targetSyncPosition
				//&& rb.position.x != targetSyncPotision
			)
			{
				this.SyncPosition(targetSyncPosition);
			}
			else
			{
				isSyncPosition = false;
			}
			*/

			if (isSyncPosition)
			{
				if (transform.position.x != targetSyncPosition)
				{
					SyncPosition(targetSyncPosition, targetSyncSpeed);
				}
				else
				{
					if (queueSyncPosition.Count > 0)
					{
						var item = queueSyncPosition.Dequeue();
						var remainTime = 2 * timeIntervalSyncPosition - item.time;
						if (remainTime > 0)
						{
							targetSyncPosition = item.pos;
							targetSyncSpeed = Mathf.Abs(targetSyncPosition - transform.position.x) / remainTime;
						}
						//Debug.Log($"item pos {item.pos} time {item.time} remain {remainTime} speed {targetSyncSpeed}");
						isSyncPosition = true;
					}
					else
					{
						isSyncPosition = false;
					}
				}
			}
			return;
		}
		base.Movement();

		
		timeSyncPosition += Time.deltaTime;
		if (timeSyncPosition > timeIntervalSyncPosition)
		{
			timeSyncPosition = 0;
			_networkManager.PlayerPosition(transform.position.x);
			//_networkManager.PlayerPosition(rb.position.x);
		}
	}

	private void SyncPosition(float targetPosition, float targetSpeed)
	{
		//MoveTo(targetSyncPotision);

		var step = targetSpeed * Time.deltaTime;
		Vector3 target = new Vector3(targetPosition, transform.position.y, transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, target, step);
	}
}


public class PongSyncPosition
{
	public float pos;
	public float time;
}