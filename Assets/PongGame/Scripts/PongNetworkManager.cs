using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using Colyseus.Schema;
using UnityEngine;

public class PongNetworkManager : MonoBehaviour
{
	private static ColyseusClient _client = null;
	private static PongMenuManager _menuManager = null;
	private static ColyseusRoom<MyPongState> _room = null;

	public static event Action<string, PongPlayer> OnPositionChanged;

	public GameObject playerPrefab;
	public GameObject ballPrefab;
	public Transform spawnPlayerPosition;
	public Transform spawnOpponentPosition;
	public Transform spawnBallPosition;

	private async void Start()
	{
		await this.JoinOrCreateGame();
		RegisterEvents();
	}

	private void OnDestroy()
	{
		_room?.Leave(true);
		UnregisterEvents();
	}

	public void Initialize()
	{
		if (_menuManager == null)
		{
			_menuManager = gameObject.AddComponent<PongMenuManager>();
		}

		_client = new ColyseusClient(_menuManager.HostAddress);
	}

	public async Task JoinOrCreateGame()
	{
		// Will create a new game room if there is no available game rooms in the server.
		_room = await Client.JoinOrCreate<MyPongState>(_menuManager.GameName);
	}

	private void UnregisterEvents()
	{
		_room.OnLeave -= Room_OnLeave;
		_room.State.players.OnAdd -= Players_OnAdd;
		_room.State.players.OnRemove -= Players_OnRemove;
		//_room.State.OnChange -= State_OnChange;
	}

	private void RegisterEvents()
	{
		_room.OnLeave += Room_OnLeave;
		_room.State.players.OnAdd += Players_OnAdd;
		_room.State.players.OnRemove += Players_OnRemove;
		//_room.State.OnChange += State_OnChange;
		_room.OnMessage<PongPlayer>(player =>
		{
			OnPositionChanged?.Invoke(player.id, player);
		});
	}

	private void Room_OnLeave(int code)
	{
		Debug.Log($"Room_OnLeave {code} sectionID {_room.SessionId}");
		DestroyPlayer(_room.SessionId);
	}

	private void State_OnChange(List<DataChange> changes)
	{
		var player = GameRoom.State.players[GameRoom.SessionId];
		OnPositionChanged?.Invoke(GameRoom.SessionId, player);
	}

	private void Players_OnRemove(string key, PongPlayer value)
	{
		Debug.Log($"Players_OnRemove {key} player pos {value.pos}");
		DestroyPlayer(key);
	}

	private void Players_OnAdd(string key, PongPlayer value)
	{
		Debug.Log($"Players_OnAdd {key} player pos {value.pos}");
		var player = CreatePlayer(key);
	}

	public ColyseusClient Client
	{
		get
		{
			// Initialize Colyseus client, if the client has not been initiated yet or input values from the Menu have been changed.
			if (_client == null || !_client.Endpoint.Uri.ToString().Contains(_menuManager.HostAddress))
			{
				Initialize();
			}
			return _client;
		}
	}

	public ColyseusRoom<MyPongState> GameRoom
	{
		get
		{
			if (_room == null)
			{
				Debug.LogError("Room hasn't been initialized yet!");
			}
			return _room;
		}
	}

	public void PlayerPosition(float x)
	{
		GameRoom.Send("pong_player_position", new { pos = x });
	}

	public GameObject CreatePlayer(string sectionId)
	{
		var player = Instantiate(playerPrefab);
		player.name = sectionId;

		PongNetworkPaddle paddle = player.GetComponent<PongNetworkPaddle>();
		paddle.PlayerID = sectionId;
		paddle.InitPosition(spawnPlayerPosition.position, spawnOpponentPosition.position);

		return player;
	}

	public bool DestroyPlayer(string sectionId)
	{
		var player = GameObject.Find(sectionId);
		if (player != null)
		{
			Destroy(player);
			return true;
		}
		Debug.LogError($"can not destroy player {sectionId}");
		return false;
	}
}
