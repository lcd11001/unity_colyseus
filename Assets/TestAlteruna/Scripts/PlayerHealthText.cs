using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthText : MonoBehaviour
{
	[SerializeField] private TextMeshPro healthText;
	
	public void UpdateHealth(int health)
	{
		healthText.text = health.ToString();
	}
}
