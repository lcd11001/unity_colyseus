using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Avatar = Alteruna.Avatar;

[RequireComponent(typeof(PaddleController))]
public class AIController : MonoBehaviour
{
	[SerializeField] private BallController ball;
	[SerializeField] private float timeInterval = 1.0f;

	private float time = 0;
	private PaddleController paddle;
	private float targetPositionX;

	private PongMultiplayerManager manager;
	private Avatar avatar;

	private void Awake()
	{
		paddle = GetComponent<PaddleController>();
		avatar = GetComponent<Avatar>();

		/*
		manager = GameObject.FindObjectOfType<PongMultiplayerManager>();

		manager.OnBallCreated.AddListener(spawnBall =>
		{
			Debug.Log($"Ai::OnBallCreated {spawnBall.name}");
			ball = spawnBall;
		});
		manager.OnBallDestroyed.AddListener(destroyedBall =>
		{
			Debug.Log($"Ai::OnBallDestroyed {destroyedBall.name}");
			ball = null;
		});
		*/
		Debug.Log("PongMultiplayerManager register event");
		PongMultiplayerManager.onBallCreated += onBallCreated;
		PongMultiplayerManager.onBallDestroyed += onBallDestroyed;
	}

	private void onBallDestroyed(BallController ball)
	{
		if (!avatar.IsMe)
		{
			return;
		}

		Debug.Log($"Ai onBallDestroyed {ball.name}");
		this.ball = null;
	}

	private void onBallCreated(BallController ball)
	{
		if (!avatar.IsMe)
		{
			return;
		}

		Debug.Log($"Ai onBallCreated {ball.name}");
		this.ball = ball;
	}

	void Update()
	{
		if (!avatar.IsMe)
		{
			return;
		}
		time += Time.deltaTime;
		if (time > timeInterval)
		{
			if (ball != null)
			{
				targetPositionX = ball.transform.position.x;
			}
			time = 0;
		}

		if (paddle.transform.position.x != targetPositionX)
		{
			paddle.MoveTo(targetPositionX);
		}
	}
}
