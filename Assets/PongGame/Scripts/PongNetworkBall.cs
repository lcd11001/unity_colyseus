using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongNetworkBall : BallController
{
	[SerializeField] private float maxDistance = 0.2f;
	[SerializeField] private float timeIntervalSyncPosition = 1;
	private float timeSyncPosition = 0;
	private float timeCheckPosition = 0;
	private PongNetworkManager _networkManager;

	private SortedList<int, Vector2> otherQueue;
	private SortedList<int, Vector2> myQueue;

	private Vector2 otherBall;
	private Vector2 myBall;
	private int checkIndex = 0;

	private int tick = 0;

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
		BufferState(ball);
	}

	public void BufferState(PongBall state)
	{
		otherQueue.Add(state.tick, new Vector2(state.x, state.y));
	}

	public override void ResetBall()
	{
		base.ResetBall();
		timeCheckPosition = 0;
		timeSyncPosition = 0;
		tick = 0;
		checkIndex = 0;
		otherQueue = new SortedList<int, Vector2>();
		myQueue = new SortedList<int, Vector2>();
	}

	public override void SetForce(Vector2 force)
	{
		this.force = force;
		this.rbForce = new Vector3(force.x, 0, force.y) * thrust;
	}

	private void Update()
	{
		SyncPositionToServer();

		CheckPosition();
	}

	private void CheckPosition()
	{
		timeCheckPosition += Time.deltaTime;
		if (timeCheckPosition > timeIntervalSyncPosition / 2)
		{
			timeCheckPosition = 0;

			if (otherQueue.TryGetValue(checkIndex, out otherBall))
			{
				if (myQueue.TryGetValue(checkIndex, out myBall))
				{
					if (Vector2.Distance(otherBall, myBall) > maxDistance)
					{
						CorrectPosition(otherBall, myBall);
					}

					otherQueue.Remove(checkIndex);
					myQueue.Remove(checkIndex);

					checkIndex++;
				}
			}
		}
	}

	private void CorrectPosition(Vector2 otherBall, Vector2 myBall)
	{
		Debug.Log($"CorrectPosition {otherBall} vs {myBall}");
	}

	private void SyncPositionToServer()
	{
		timeSyncPosition += Time.deltaTime;
		if (timeSyncPosition > timeIntervalSyncPosition)
		{
			timeSyncPosition = 0;
			Vector2 pos = new Vector2(transform.position.x, transform.position.z);
			_networkManager.BallPosition(pos.x, pos.y, tick);
			myQueue.Add(tick, pos);
			tick++;
		}
	}
}
