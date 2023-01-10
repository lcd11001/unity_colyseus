using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
	[SerializeField] private int thrust;

	private void Start()
	{
		GetComponent<Rigidbody>().AddForce(new Vector3(3, 0, 15) * thrust, ForceMode.Force);
	}
}
