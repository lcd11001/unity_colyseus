using Alteruna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
	[SerializeField] private int indexToSpawn = 0;
	[SerializeField] private LayerMask despawnLayer;
	[SerializeField] private GameObject reddotPrefab;

	private Alteruna.Avatar avatar;
	private Spawner spawner;
	private GameObject reddot;

	private void Awake()
	{
		avatar = GetComponent<Alteruna.Avatar>();
		spawner = GameObject.FindObjectOfType<Spawner>();

		reddot = Instantiate(reddotPrefab);
		reddot.SetActive(false);
	}

	private void Update()
	{

		if (!avatar.IsMe)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			SpawnCube();
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			DespawnCube();
		}
	}

	private void DespawnCube()
	{
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, despawnLayer))
		{
			Debug.Log($"hit {hit.transform.gameObject.name} pos {hit.transform.position} point {hit.point}");
			spawner.Despawn(hit.transform.gameObject);
		}
		else
		{
			Debug.Log("no hit");
		}
	}

	private void SpawnCube()
	{
		spawner.Spawn(indexToSpawn, Camera.main.transform.position + Camera.main.transform.forward * 2.5f, new Vector3(0.5f, 0.5f, .5f));
	}
}
