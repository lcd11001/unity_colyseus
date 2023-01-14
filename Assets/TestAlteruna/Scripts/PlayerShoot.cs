using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using System;

public class PlayerShoot : AttributesSync
{
	[SynchronizableField] private int health = 100;
	[SerializeField] private int damage = 10;
	[SerializeField] private Alteruna.Avatar avatar;
	[SerializeField] private LayerMask playerMask;
	[SerializeField] private int selfPlayerMask;

	[SerializeField] PlayerHealthText playerHealthText;

	private void Start()
	{
		playerHealthText.UpdateHealth(health);

		if (avatar.IsMe)
		{
			avatar.gameObject.layer = selfPlayerMask;
		}
	}

	private void Update()
	{
		if (!avatar.IsMe)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, playerMask))
		{
			PlayerShoot playerShoot = hit.transform.parent.GetComponentInChildren<PlayerShoot>();
			if (playerShoot != null)
			{
				playerShoot.Hit(damage);
			}
			else
			{
				Debug.Log($"not hit player shoot {hit.transform.gameObject.name}");
			}
		}
	}

	private void Hit(int damgeTaken)
	{
		health -= damgeTaken;
		playerHealthText.UpdateHealth(health);

		if (health <= 0)
		{
			//Die();

			// Send all client
			BroadcastRemoteMethod("Die");

			// Send other client, except me
			InvokeRemoteMethod("Die");
		}
	}

	[SynchronizableMethod]
	private void Die()
	{
		Debug.Log($"player die");
	}
}
