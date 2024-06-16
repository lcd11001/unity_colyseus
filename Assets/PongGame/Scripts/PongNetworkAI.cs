using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PongNetworkPaddle))]
public class PongNetworkAI : AIController
{
	private PongNetworkManager _networkManager;
	private PongNetworkPaddle _networkPaddle;
	public override void Start()
	{
		base.Start();
		_networkManager = FindObjectOfType<PongNetworkManager>();
		_networkPaddle = GetComponent<PongNetworkPaddle>();
	}

	protected override void Update()
	{
		if (_networkPaddle.IsLocalPlayer)
		{
			base.Update();
		}
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
