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

	[SerializeField]
	private Color mistakeColor = Color.red;

	[SerializeField]
	private Image imgProgress;

	[SerializeField]
	private float decaySpeed = 0.1f;

	[SerializeField]
	private float increasePerHit = 0.02f;

	[SerializeField]
	private float decreasePerMistake = 0.15f;

	[SerializeField]
	private GameObject menuPanel;

	[SerializeField]
	private GameObject progressPanel;

	[SerializeField]
	private Text txtMenuTitle;

	[SerializeField]
	private Renderer driveInImage;

	#endregion

	public int Score { get; private set; }
	public int Level { get; private set; }
	public float Progress
	{
		get { return progress; }
		set
		{
			progress = Mathf.Clamp01(value);
			imgProgress.fillAmount = Progress;
		}
	}

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
	public bool IsEndGame { get { return State == GameState.EndGame; } }
	public bool IsGameActive { get { return State == GameState.Playing || State == GameState.Delayed; } }

	private GoTween backgroundAnimation;
	private float delayTimeRemaining;
	private float progress;

	void Start()
	{
		txtMenuTitle.text = "←A    D→";
		menuPanel.SetActive(true);
		// just a placeholder
		FoodMarkers.I.Reset();
		progressPanel.SetActive(false);
		driveInImage.enabled = false;
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
		if (IsGameActive)
		{
			if (Progress <= 0f)
			{
				EndGame();
			}
			else if (Progress >= 1f)
			{
				LevelUp();
			}
		}

		if (State == GameState.Delayed)
		{
			Progress -= decaySpeed / 2f * Time.deltaTime;
			delayTimeRemaining -= Time.deltaTime;
			if (delayTimeRemaining <= 0f)
			{
				delayTimeRemaining = 0f;
				State = GameState.Playing;
			}
		}
		else if (State == GameState.Playing)
		{
			Progress -= decaySpeed * Time.deltaTime;
		}
	}

	#region Public API

	public void StartNewGame()
	{
		progressPanel.SetActive(true);
		menuPanel.SetActive(false);
		driveInImage.enabled = true;
		Progress = 0.75f;
		Score = 0; txtScore.text = "SCORE: 0";
		Level = 1; txtLevel.text = "LEVEL 1";
		State = GameState.Playing;
		FoodManager.I.StartNewGame();
	}

	public void EndGame()
	{
		State = GameState.EndGame;
		txtMenuTitle.text = Level == maxLevel ? "BRAVO FOR LEVEL 5!" : "GAME OVER";
		menuPanel.SetActive(true);
		progressPanel.SetActive(false);
		driveInImage.enabled = false;
	}

	public void IncreaseScore()
	{
		Score += Level;
		txtScore.text = "SCORE: " + Score;
		Progress += increasePerHit;
	}

	public void LevelUp()
	{
		if (Level == maxLevel)
			return;
		Level++;
		Progress = 0.5f;
		txtLevel.text = "LEVEL " + (Level == maxLevel ? "MAX!" : Level.ToString());
		FoodManager.I.AddFoodDependingOnLevel();
	}

	public void SignalMistake()
	{
		Progress -= decreasePerMistake;
		delayTimeRemaining += mistakeDelayDuration;
		State = GameState.Delayed;
		if (backgroundAnimation != null)
		{
			backgroundAnimation.destroy();
		}
		backgroundAnimation = Go.to(Camera.main, mistakeDelayDuration / 2f, new GoTweenConfig()
			.colorProp("backgroundColor", mistakeColor)
			.setIterations(2, GoLoopType.PingPong)
		);
	}

	#endregion
}
