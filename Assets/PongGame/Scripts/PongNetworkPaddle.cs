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

	protected override void Start()
	{
		base.Start();
		timeEnqueue = Time.unscaledTime;
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
			float nextTarget = -player.pos;

			if (nextTarget != targetSyncPosition)
			{
				// https://docs.unity3d.com/Manual/TimeFrameManagement.html
				float timeSendNetworkPackage = Time.unscaledTime - timeEnqueue;
				//float remainTime = timeIntervalSyncPosition - (timeSendNetworkPackage - timeIntervalSyncPosition);
				float remainTime = 2 * timeIntervalSyncPosition - timeSendNetworkPackage;
				float speed = Mathf.Abs(nextTarget - targetSyncPosition) / remainTime;

				Debug.Log($"** queue ** current {targetSyncPosition} next {nextTarget}");
				Debug.Log($"   network {timeSendNetworkPackage} remain {remainTime}");
				Debug.Log($"   pos {nextTarget} speed {speed}");

				queueSyncPosition.Enqueue(new PongSyncPosition { pos = nextTarget, speed = speed });
				isSyncPosition = true;
			}

			timeEnqueue = Time.unscaledTime;
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

	protected override void Update()
	{
		if (!IsLocalPlayer)
		{
			if (isSyncPosition)
			{
				if (transform.position.x != targetSyncPosition)
				{
					InterpolatePosition(targetSyncPosition, targetSyncSpeed);
				}
				else
				{
					SyncPositionToClient();
				}
			}
			return;
		}

		base.Update();

		SyncPositionToServer();
	}

	private void SyncPositionToClient()
	{
		if (queueSyncPosition.Count > 0)
		{
			var item = queueSyncPosition.Dequeue();
			targetSyncPosition = item.pos;
			targetSyncSpeed = item.speed;

			Debug.Log($"## Interpolate ## pos {targetSyncPosition} speed {targetSyncSpeed}");
			isSyncPosition = true;
		}
		else
		{
			isSyncPosition = false;
		}
	}

	private void SyncPositionToServer()
	{
		timeSyncPosition += Time.unscaledDeltaTime;
		if (timeSyncPosition > timeIntervalSyncPosition)
		{
			timeSyncPosition = 0;
			_networkManager.PlayerPosition(transform.position.x);
		}
	}

	private void InterpolatePosition(float targetPosition, float targetSpeed)
	{
		var step = targetSpeed * Time.deltaTime;
		targetPosition = Mathf.Clamp(targetPosition, -limitX, limitX);
		Vector3 target = new Vector3(targetPosition, transform.position.y, transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, target, step);
	}
}


public class PongSyncPosition
{
	public float pos;
	public float speed;
}