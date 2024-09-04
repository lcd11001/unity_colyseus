using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PongMenuManager : MonoBehaviour
{
	public enum FIELD
	{
		GAME_NAME,
		HOST_NAME,
		PORT,
		IS_AI,
		IS_SECURE_PROTOCOL,
	}

	private static PongMenuManager _instance = null;

	[field: SerializeField]
	public string GameName { get; set; } = "pong_room";

	[field: SerializeField]
	public string HostName { get; set; } = "localhost";

	[field: SerializeField]
	public string Port { get; set; } = "2567";

	public string Protocol
	{
		get => IsSecureProtocol ? "wss" : "ws";
	}

	[field: SerializeField]
	public bool IsAI { get; set; } = false;


	[field: SerializeField]
	public bool IsSecureProtocol { get; set; } = false;


	public string HostAddress => $"{Protocol}://{HostName}:{Port}";

	private void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void Play()
	{
		SceneManager.LoadScene("PongMultiPlayer", LoadSceneMode.Single);
	}

	public string GetField(FIELD field)
	{
		switch (field)
		{
			case FIELD.GAME_NAME:
				return GameName;
			case FIELD.HOST_NAME:
				return HostName;
			case FIELD.PORT:
				return Port;
			case FIELD.IS_AI:
				return IsAI.ToString();
			case FIELD.IS_SECURE_PROTOCOL:
				return IsSecureProtocol.ToString();
			default:
				throw new ArgumentOutOfRangeException(nameof(field), field, null);
		}
	}
}
