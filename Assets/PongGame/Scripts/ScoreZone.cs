using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreZone : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtName;
	[SerializeField] private TextMeshProUGUI txtScore;

	private int score;

	private void Start()
	{
		score = 0;
		txtScore.text = score.ToString();
	}

	private void OnCollisionEnter(Collision collision)
	{
		score++;
		txtScore.text = score.ToString();
	}
}
