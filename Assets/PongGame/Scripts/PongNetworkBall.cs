using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongNetworkBall : BallController
{
	[SerializeField] private float timeIntervalSyncPosition = 1;
	private float timeSyncPosition = 0;
	private PongNetworkManager _networkManager;

	protected override void Awake()
	{
		base.Awake();
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
		PongNetworkManager.OnBallPosisionChanged += PongNetworkManager_OnBallPosisionChanged;
	}

	private void UnregisterEvents()
	{
		PongNetworkManager.OnBallPosisionChanged -= PongNetworkManager_OnBallPosisionChanged;
	}

	private void PongNetworkManager_OnBallPosisionChanged(PongBall ball)
	{
		Vector3 netPos = new Vector3(ball.x, 0, ball.y);
		Debug.Log($"PongNetworkManager_OnBallPosisionChanged {netPos} vs {transform.position}");
	}

	public override void SetForce(Vector2 force)
	{
		this.rbForce = new Vector3(force.x, 0, force.y) * thrust;
	}

	private void Update()
	{
		SyncPositionToServer();
	}

	private void SyncPositionToServer()
	{
		timeSyncPosition += Time.deltaTime;
		if (timeSyncPosition > timeIntervalSyncPosition)
		{
			timeSyncPosition = 0;
			_networkManager.BallPosition(new Vector2(transform.position.x, transform.position.z));
		}
	}
}
