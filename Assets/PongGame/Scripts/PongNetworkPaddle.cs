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
	private float targetSyncPotision = 0;

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
			targetSyncPotision = -player.pos;
		}
	}

	public void InitPosition(Vector3 myPosition, Vector3 opponentPosition)
	{
		transform.position = IsLocalPlayer ? myPosition : opponentPosition;
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
			if (isSyncPosition
				&& this.transform.position.x != targetSyncPotision
				//&& rb.position.x != targetSyncPotision
			)
			{
				this.SyncPosition(targetSyncPotision);
			}
			else
			{
				isSyncPosition = false;
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

	private void SyncPosition(float targetSyncPotision)
	{
		//MoveTo(targetSyncPotision);

		var step = (moveSpeed + timeIntervalSyncPosition) * Time.deltaTime;
		Vector3 target = new Vector3(targetSyncPotision, transform.position.y, transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, target, step);
	}
}
