using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PongMenuManager : MonoBehaviour
{
	[SerializeField]
	private string gameName = null;
	[SerializeField]
	private string hostname = null;
	[SerializeField]
	private string port = null;
	[SerializeField]
	private bool secureProtocol = false;
	[SerializeField]
	private bool isAI = false;

	private static PongMenuManager _instance = null;

	public string GameName
	{
		get => string.IsNullOrEmpty(gameName) ? "pong_room" : gameName;
		set => gameName = value;
	}

	public string HostName
	{
		get => string.IsNullOrEmpty(hostname) ? "localhost" : hostname;
		set => hostname = value;
	}

	public string Port
	{
		get => string.IsNullOrEmpty(port) ? "2567" : port;
		set => port = value;
	}

	public string Protocol
	{
		get => secureProtocol ? "wss" : "ws";
		set => secureProtocol = !secureProtocol;
	}

	public bool IsAI
	{
		get => isAI;
		set => isAI = value;
	}

	public bool IsSecureProtocol
	{
		get => secureProtocol;
		set => secureProtocol = value;
	}

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
}
