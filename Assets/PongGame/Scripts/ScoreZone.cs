using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreZone : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtName;
	[SerializeField] private TextMeshProUGUI txtScore;

	[SerializeField] private GameObject gameOverScreen;

	private int score;

	private void Start()
	{
		score = 0;
		txtScore.text = score.ToString();
	}

	private void OnCollisionEnter(Collision collision)
	{
		var ball = collision.gameObject.GetComponent<BallController>();
		if (ball != null)
		{
			score++;
			txtScore.text = score.ToString();

			ball.ResetBall();

			//if (score >= 7)
			//{
			//	ball.gameObject.SetActive(false);
			//	gameOverScreen.SetActive(true);
			//}
		}
	}
}
