using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	#region Instance

	private static GameManager instance;

	public static GameManager I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}


	#endregion

	public enum GameState
	{
		Intro = 0,
		Menu,
		Playing,
		Paused,
		Delayed,
		EndGame
	}

	public GameState State { get; private set; }

	void Start()
	{
		StartNewGame();
	}

	public void StartNewGame()
	{
		State = GameState.Playing;
		FoodManager.I.StartNewGame();
	}
}
