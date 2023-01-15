using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
	[SerializeField] private float thrust = 100f;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		ResetBall();
	}

	public void ResetBall()
	{
		transform.position = new Vector3(0, transform.position.y, 0);
		rb.velocity = Vector3.zero;
		rb.AddForce(new Vector3(3 * RandomDirection, 0, 15 * RandomDirection) * thrust, ForceMode.Force);
	}

	private int RandomDirection => Random.Range(0, 2) * 2 - 1;
}
