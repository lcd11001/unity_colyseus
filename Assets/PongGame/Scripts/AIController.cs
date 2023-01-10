using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PaddleController))]
public class AIController : MonoBehaviour
{
	[SerializeField] private BallController ball;
	[SerializeField] private float timeInterval = 1.0f;

	private float time = 0;
	private PaddleController paddle;
	private float targetPositionX;

	private void Start()
	{
		paddle = GetComponent<PaddleController>();
	}

	void Update()
    {
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
