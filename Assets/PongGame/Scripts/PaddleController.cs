using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PaddleController : MonoBehaviour
{
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float limitX = 6.9f;
	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Movement();
	}

	private void Movement()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
			Vector3 myPosition = rb.position;

			myPosition.x = Mathf.Lerp(myPosition.x, mousePosition.x, moveSpeed);
			myPosition.x = Mathf.Clamp(myPosition.x, -limitX, limitX);

			rb.position = myPosition;
		}
	}
}
