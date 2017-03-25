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

	[SerializeField]
	private float mistakeDelayDuration = 0.2f;

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
	public bool IsPlaying { get { return State == GameState.Playing; } }

	private GoTween backgroundAnimation;
	private float delayTimeRemaining;

	void Start()
	{
		StartNewGame();
	}

	void OnDestroy()
	{
		if (backgroundAnimation != null)
		{
			backgroundAnimation.destroy();
		}
	}

	void Update()
	{
		if (State == GameState.Delayed)
		{
			delayTimeRemaining -= Time.deltaTime;
			if (delayTimeRemaining <= 0f)
			{
				delayTimeRemaining = 0f;
				State = GameState.Playing;
			}
		}
	}

	#region Public API

	public void StartNewGame()
	{
		Score = 0; txtScore.text = "SCORE: 0";
		Level = 1; txtLevel.text = "LEVEL 1";
		State = GameState.Playing;
		FoodManager.I.StartNewGame();
	}

	public void IncreaseScore()
	{
		Score += Level;
		txtScore.text = "SCORE: " + Score;
	}

	public void LevelUp()
	{
		if (Level == maxLevel)
			return;
		Level++;
		txtLevel.text = "LEVEL " + (Level == maxLevel ? "MAX!" : Level.ToString());
		FoodManager.I.AddFoodDependingOnLevel();
	}

	public void SignalMistake()
	{
		delayTimeRemaining += mistakeDelayDuration;
		State = GameState.Delayed;
		if (backgroundAnimation != null)
		{
			backgroundAnimation.destroy();
		}
		backgroundAnimation = Go.to(Camera.main, mistakeDelayDuration / 2f, new GoTweenConfig()
			.colorProp("backgroundColor", Color.red)
			.setIterations(2, GoLoopType.PingPong)
		);
	}

	#endregion
}
