using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
	[SerializeField] protected float thrust = 100f;
	[SerializeField] protected Vector2 force = new Vector2(3, 15);
	protected Rigidbody rb;
	protected Vector3 rbForce;

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
		SetForce(force);
	}

	protected virtual void Start()
	{
		ResetBall();
	}

	public virtual void ResetBall()
	{
		transform.position = Vector3.zero;
		rb.velocity = Vector3.zero;
		rb.AddForce(rbForce, ForceMode.Force);
	}

	public virtual void SetForce(Vector2 force)
	{
		this.rbForce = new Vector3(force.x * RandomDirection, 0, force.y * RandomDirection) * thrust;
	}

	private int RandomDirection => Random.Range(0, 2) * 2 - 1;
}
