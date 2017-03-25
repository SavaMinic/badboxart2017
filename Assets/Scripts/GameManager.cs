using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	#region Editor fields

	[SerializeField]
	private Text txtScore;

	[SerializeField]
	private Text txtLevel;

	[SerializeField]
	private int maxLevel = 4;

	#endregion

	public int Score { get; private set; }
	public int Level { get; private set; }

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
		Score = 0; txtLevel.text = "SCORE: 0";
		Level = 0; txtLevel.text = "LEVEL 1";
		State = GameState.Playing;
		FoodManager.I.StartNewGame();
	}

	public void IncreaseScore(int points)
	{
		Score += points;
		txtScore.text = "SCORE: " + Score;
	}

	public void LevelUp()
	{
		if (Level == maxLevel)
			return;
		Level++;
		txtLevel.text = "LEVEL " + (Level == maxLevel ? "MAX!" : (Level + 1).ToString());
		FoodManager.I.AddFoodDependingOnLevel();
	}
}
