using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avatar = Alteruna.Avatar;

[RequireComponent(typeof(Rigidbody), typeof(Avatar), typeof(MeshRenderer))]
public class PaddleController : MonoBehaviour
{
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float limitX = 6.9f;
	[SerializeField] float touchControlDistance = 5f;
	[SerializeField] Material player1;
	[SerializeField] Material player2;
	private Rigidbody rb;
	private float distanceToCamera;
	private Avatar avatar;
	private MeshRenderer mr;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		avatar = GetComponent<Avatar>();
		mr = GetComponent<MeshRenderer>();
		distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
		mr.material = avatar.IsMe ? player1 : player2;
	}

	private void Update()
	{
		if (!avatar.IsMe)
		{
			return;
		}

		Movement();
	}

	private void Movement()
	{
		Vector3 myPosition = rb.position;

		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera));
			if (Mathf.Abs(mousePosition.z - myPosition.z) <= touchControlDistance)
			{
				MoveTo(mousePosition.x);
			}
			//else
			//{
			//	Debug.Log($"mouse {mousePosition.z} vs {rb.gameObject.name} {myPosition.z}");
			//}
		}

		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
			if (Mathf.Abs(touchPosition.z - myPosition.z) <= touchControlDistance)
			{
				MoveTo(touchPosition.x);
			}
			//else
			//{
			//	Debug.Log($"touch {touchPosition.z} vs {rb.gameObject.name} {myPosition.z}");
			//}
		}
	}

	public void MoveTo(float x)
	{
		Vector3 myPosition = rb.position;

		myPosition.x = Mathf.Lerp(myPosition.x, x, moveSpeed * Time.deltaTime);
		myPosition.x = Mathf.Clamp(myPosition.x, -limitX, limitX);

		rb.position = myPosition;
	}
}
