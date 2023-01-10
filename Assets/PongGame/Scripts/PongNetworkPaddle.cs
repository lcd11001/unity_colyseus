using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PongNetworkPaddle : MonoBehaviour
{
	[SerializeField] private string playerID;

	private PongNetworkManager _networkManager;

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

	public bool IsLocalPlayer => (_networkManager == null || _networkManager.GameRoom == null) ? false : _networkManager.GameRoom.SessionId == PlayerID;
	public bool IsOwner(string sectionID) => sectionID == PlayerID;

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

}
