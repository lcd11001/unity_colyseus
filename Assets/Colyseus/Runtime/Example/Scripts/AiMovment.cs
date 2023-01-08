
using UnityEngine;
using UnityEngine.Events;

public class AiMovment : MonoBehaviour
{
	public UnityEvent<Vector2> OnAiPosition;
	public float scheduleTime = 0f;
	public MovingZone movingZone;

	private float remainingTime;

	private void Start()
	{
		remainingTime = scheduleTime;
		movingZone = FindObjectOfType<MovingZone>();
	}

	private void Update()
	{
		remainingTime -= Time.deltaTime;
		if (remainingTime < 0)
		{
			if (movingZone != null)
			{
				float x = Random.Range(movingZone.Left, movingZone.Right);
				float y = Random.Range(movingZone.Bottom, movingZone.Top);
				Vector2 nextPosition = new Vector2(x, y);
				OnAiPosition?.Invoke(nextPosition);
			}

			remainingTime = scheduleTime;
		}
	}
}
