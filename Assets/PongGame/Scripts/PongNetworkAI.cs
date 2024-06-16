using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongNetworkAI : AIController
{
	private PongNetworkManager _networkManager;
	public override void Start()
	{
		base.Start();
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
		PongNetworkManager.OnBallCreated += NetworkManager_OnBallCreated;
	}

	private void UnregisterEvents()
	{
		PongNetworkManager.OnBallCreated -= NetworkManager_OnBallCreated;
	}

	private void NetworkManager_OnBallCreated(PongNetworkBall ball)
	{
		this.ball = ball;
	}
}
