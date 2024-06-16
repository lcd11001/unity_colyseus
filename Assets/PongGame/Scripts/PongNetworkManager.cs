using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using Colyseus.Schema;
using UnityEngine;

public enum JOIN_ROOM_STATUS
{
	CONNECTING = -1,
	FAILED = 0,
	SUCCESS = 1,
}

public class PongNetworkManager : MonoBehaviour
{
	private static ColyseusClient _client = null;
	private static PongMenuManager _menuManager = null;
	private static ColyseusRoom<MyPongState> _room = null;

	public static event Action<string, PongPlayer> OnPositionChanged;
	public static event Action<PongBall> OnBallPosisionChanged;
	public static event Action<PongNetworkBall> OnBallCreated;
	public static event Action<JOIN_ROOM_STATUS> OnJoinRoomStatus;

	public GameObject playerPrefab;
	public GameObject aiPrefab;
	public GameObject ballPrefab;
	public Transform spawnPlayerPosition;
	public Transform spawnOpponentPosition;
	public Transform spawnBallPosition;

	private async void Start()
	{
		await this.JoinOrCreateGame();
		RegisterEvents();
	}

	private async void OnDestroy()
	{
		if (_room != null)
		{
			await _room.Leave(true);
		}
		UnregisterEvents();
	}

	public void Initialize()
	{
		InitClient();
	}

	private void InitClient()
	{
		if (_client == null)
		{
			_client = new ColyseusClient(Menu.HostAddress);
		}
	}

	private void InitMenu()
	{
		if (_menuManager == null)
		{
			_menuManager = FindObjectOfType<PongMenuManager>();

			if (_menuManager == null)
			{
				_menuManager = gameObject.AddComponent<PongMenuManager>();
			}
		}
	}


	public async Task JoinOrCreateGame()
	{
		try
		{
			OnJoinRoomStatus?.Invoke(JOIN_ROOM_STATUS.CONNECTING);

			// Will create a new game room if there is no available game rooms in the server.
			Dictionary<string, object> options = new Dictionary<string, object>
			{
				{ "isAI", Menu.IsAI }
			};
			_room = await Client.JoinOrCreate<MyPongState>(Menu.GameName, options);

			OnJoinRoomStatus?.Invoke(JOIN_ROOM_STATUS.SUCCESS);
		}
		catch (Exception e)
		{
			Debug.Log($"JoinOrCreateGame error {e.Message}");

			OnJoinRoomStatus?.Invoke(JOIN_ROOM_STATUS.FAILED);
		}
	}

	private void UnregisterEvents()
	{
		if (_room == null)
		{
			return;
		}
		_room.OnLeave -= Room_OnLeave;
		_room.State.players.OnAdd -= Players_OnAdd;
		_room.State.players.OnRemove -= Players_OnRemove;
		//_room.State.OnChange -= State_OnChange;
	}

	private void RegisterEvents()
	{
		if (_room == null)
		{
			return;
		}
		_room.OnLeave += Room_OnLeave;
		_room.State.players.OnAdd += Players_OnAdd;
		_room.State.players.OnRemove += Players_OnRemove;
		//_room.State.OnChange += State_OnChange;
		_room.OnMessage<PongPlayer>(player =>
		{
			OnPositionChanged?.Invoke(player.id, player);
		});
		_room.OnMessage("pong_start_game", (Vector2 force) =>
		{
			CreateBall(_room.Id, force);
		});
		_room.OnMessage("pong_stop_game", (string _) =>
		{
			DeleteBall(_room.Id);
		});
		_room.OnMessage<PongBall>(ball =>
		{
			OnBallPosisionChanged?.Invoke(ball);
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
		var player = CreatePlayer(key, value.ai);
	}

	public ColyseusClient Client
	{
		get
		{
			// Initialize Colyseus client, if the client has not been initiated yet or input values from the Menu have been changed.
			if (_client == null || !_client.Endpoint.Uri.ToString().Contains(Menu.HostAddress))
			{
				Initialize();
			}
			return _client;
		}
	}

	public PongMenuManager Menu
	{
		get
		{
			if (_menuManager == null)
			{
				InitMenu();
			}
			return _menuManager;
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

	public void PlayerPosition(float pos)
	{
		GameRoom.Send("pong_player_position", new { pos });
	}

	public void BallPosition(float x, float y, int tick)
	{
		GameRoom.Send("pong_ball_position", new { x, y, tick });
	}

	public GameObject CreatePlayer(string sectionId, bool isAI)
	{
		var player = isAI ? Instantiate(aiPrefab) : Instantiate(playerPrefab);
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

	private GameObject CreateBall(string roomId, Vector2 force)
	{
		var ball = Instantiate(ballPrefab);
		ball.name = roomId;

		PongNetworkBall networkBall = ball.GetComponent<PongNetworkBall>();
		networkBall.SetForce(force);

		OnBallCreated?.Invoke(networkBall);

		return ball;
	}

	private bool DeleteBall(string roomId)
	{
		// Fix: sometime, ball is inactive
		var ball = GameObject.FindObjectOfType<PongNetworkBall>(true);
		if (ball != null && ball.gameObject.name == roomId)
		{
			Destroy(ball.gameObject);

			OnBallCreated?.Invoke(null);

			return true;
		}
		Debug.LogError($"can not destroy ball {roomId}");
		return false;
	}


}
