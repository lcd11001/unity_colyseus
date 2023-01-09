using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private TextMeshPro textName;
	[SerializeField] private TextMeshProUGUI textGuiName;
	[SerializeField] private string playerID;
	[SerializeField] private AiMovment ai;

	private bool _moving;
	private NetworkManager _networkManager;
	private Vector2 _targetPosition;

	public bool IsLocalPlayer => (_networkManager == null ||_networkManager.GameRoom == null) ? false : _networkManager.GameRoom.SessionId == PlayerID;
	public bool IsOwner(string sectionID) => sectionID == PlayerID;

	public string PlayerID
	{
		get
		{
			return playerID;
		}
		set
		{
			playerID = value;
			SetName(value);
		}
	}

	private void SetName(string value)
	{
		if (textName != null)
		{
			textName.text = value;
			if (IsLocalPlayer)
			{
				textName.color = Color.red;
			}
		}

		if (textGuiName != null)
		{
			textGuiName.text = value;
			if (IsLocalPlayer)
			{
				textGuiName.color = Color.red;
			}
		}
	}

	private void Awake()
	{
		_networkManager = FindObjectOfType<NetworkManager>();
		if (ai != null)
		{
			ai.enabled = false;
		}
	}

	private void OnEnable()
	{
		RegisterEvents();
	}

	private void OnDestroy()
	{
		UnregisterEvents();
	}

	private void RegisterEvents()
	{
		NetworkManager.OnPositionChanged += NetworkManager_OnPositionChanged;
		if (ai != null)
		{
			ai.OnAiPosition.AddListener(OnAiPosition);
		}
	}

	private void UnregisterEvents()
	{
		NetworkManager.OnPositionChanged -= NetworkManager_OnPositionChanged;
		if (ai != null)
		{
			ai.OnAiPosition.RemoveListener(OnAiPosition);
		}
	}

	private void OnAiPosition(Vector2 screenPosition)
	{
		if (IsLocalPlayer)
		{
			_networkManager.PlayerPosition(screenPosition);
		}
	}

	private void NetworkManager_OnPositionChanged(string sectionId, Player player)
	{
		//Debug.Log($"network {sectionId} vs my {this.sectionID}");
		if (IsOwner(sectionId))
		{
			TargetPosition = Camera.main.ScreenToWorldPoint( new Vector2(player.x, player.y));
		}
	}

	public Vector2 TargetPosition
	{
		get => _targetPosition;
		set
		{
			_moving = true;
			_targetPosition = value;
		}
	}

	//public async void Start()
	//{
	// Initialize game room.
	//_networkManager = gameObject.AddComponent<NetworkManager>();
	//await _networkManager.JoinOrCreateGame();

	// Assigning listener for incoming messages
	//_networkManager.GameRoom.OnMessage<string>("welcomeMessage", message =>
	//{
	//	Debug.Log(message);
	//});

	//_networkManager.GameRoom.OnMessage<string>("sessionId", sessionId =>
	//{
	//	Debug.Log($"sessionId {sessionId} vs room {_networkManager.GameRoom.SessionId}");
	//	_sectionID = sessionId;
	//	textName.text = sessionId;
	//});


	//// Set player's new position after synchronized the mouse click's position with the Colyseus server. 
	//_networkManager.GameRoom.State.OnChange += (changes) =>
	//{
	//	changes.ForEach(change =>
	//	{
	//		Debug.Log($"change {change.ToString()}");

	//	});
	//	var player = _networkManager.GameRoom.State.players[_networkManager.GameRoom.SessionId];
	//	_targetPosition = new Vector2(player.x, player.y);
	//	_moving = true;
	//};

	//_networkManager.GameRoom.State.players.OnAdd += (key, player) =>
	//{
	//	Debug.Log($"Player {key} has joined the Game!");
	//	Debug.Log($"key {key} vs room {_networkManager.GameRoom.SessionId}");

	//};

	//_networkManager.GameRoom.State.players.OnRemove += (key, player) =>
	//{
	//	Debug.Log($"Player {key} has left the Game!");
	//};

	//}

	private void Update()
	{
		if (IsLocalPlayer)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				ai.enabled = !ai.enabled;
				Debug.Log(ai.enabled ? "Enable AI" : "Disable AI");
			}
			if (Input.GetMouseButtonDown(0))
			{
				// Synchronize mouse click position with the Colyseus server.
				_networkManager.PlayerPosition(Input.mousePosition);
			}
		}

		if (_moving && (Vector2)transform.position != _targetPosition)
		{
			var step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, _targetPosition, step);
		}
		else
		{
			_moving = false;
		}
	}
}
