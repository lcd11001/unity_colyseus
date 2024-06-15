using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;

public class PongRoomStatus : MonoBehaviour
{
	[SerializeField] private GameObject pannel;
	[SerializeField] private TMP_Text status;

	private void Start()
	{
		pannel.SetActive(false);
		status.text = string.Empty;

		PongNetworkManager.OnJoinRoomStatus += OnJoinRoomStatus;
	}

	private void OnDestroy()
	{
		PongNetworkManager.OnJoinRoomStatus -= OnJoinRoomStatus;
	}

	private void OnJoinRoomStatus(JOIN_ROOM_STATUS status)
	{
		if (status == JOIN_ROOM_STATUS.CONNECTING || status == JOIN_ROOM_STATUS.FAILED)
		{
			pannel.SetActive(true);
			this.status.text = status.ToString();
		}
		else
		{
			pannel.SetActive(false);
		}
	}
}
