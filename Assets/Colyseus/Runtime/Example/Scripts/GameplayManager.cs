using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
	NetworkManager networkManager;

	private void Start()
	{
		networkManager = FindObjectOfType<NetworkManager>();
	}
	public void BackToMenu()
    {
		if (networkManager.GameRoom != null)
		{
			networkManager.GameRoom.Leave(true);
		}
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Exit()
    {
		if (networkManager.GameRoom != null)
		{
			networkManager.GameRoom.Leave(true);
		}
		Application.Quit();
    }
}
