﻿using UnityEngine;
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

	[Header("Game config")]
	[SerializeField]
	private int maxLevel = 4;

	[SerializeField]
	private float mistakeDelayDuration = 0.2f;

	[SerializeField]
	private Color mistakeColor = Color.red;

	[SerializeField]
	private float decaySpeed = 0.1f;

	[SerializeField]
	private float increasePerHit = 0.02f;

	[SerializeField]
	private float decreasePerMistake = 0.15f;

	[Header("GUI")]
	[SerializeField]
	private Text txtScore;

	[SerializeField]
	private Text txtLevel;

	[SerializeField]
	private GameObject menuPanel;

	[SerializeField]
	private GameObject progressPanel;

	[SerializeField]
	private Image imgProgress;

	[SerializeField]
	private Text txtMenuTitle;

	[SerializeField]
	private GameObject driveInObject;

	[SerializeField]
	private GameObject tapsPanel;

	[Header("Level up animation")]
	[SerializeField]
	private Text txtLevelUp;

	[SerializeField]
	private float levelUpMoveInDuration = 0.9f;

	[Header("Intro animation")]
	[SerializeField]
	private Image introPanel;

	[SerializeField]
	private Image imgPoster;

	[SerializeField]
	private Image imgScreenshot;

	[SerializeField]
	private Text txtIntroTitle;

	[SerializeField]
	private Text txtIntroText;

	[SerializeField]
	private Text txtIntroPressToPlay;

	[SerializeField]
	private Color pressToPlayColor;

	[SerializeField]
	private float introDelay;

	[SerializeField]
	private float introElementdsDelay;

	[SerializeField]
	private float introFadeInDuration;

	[SerializeField]
	private float introFadeOutDuration;

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
	private IEnumerator introCouroutine;

	void Start()
	{
		Application.targetFrameRate = 60;

		// just a placeholder
		txtMenuTitle.text = "←A    D→";
		FoodMarkers.I.Reset();
		SetMenuActive(true);

		PlayIntroAgain();
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

	private void SetMenuActive(bool isActive)
	{
		menuPanel.SetActive(isActive);
		progressPanel.SetActive(!isActive);
		driveInObject.SetActive(!isActive);
		tapsPanel.SetActive(!isActive);
		if (isActive)
		{
			var c = txtLevelUp.color;
			c.a = 0f;
			txtLevelUp.color = c;
		}
	}

	private IEnumerator IntroAnimation()
	{
		introPanel.color = Color.black;
		introPanel.gameObject.SetActive(true);
		State = GameState.Intro;

		yield return new WaitForSeconds(introDelay);

		Go.to(imgPoster, introFadeInDuration, new GoTweenConfig().colorProp("color", Color.white));

		yield return new WaitForSeconds(introElementdsDelay);

		Go.to(txtIntroTitle, introFadeInDuration, new GoTweenConfig().colorProp("color", Color.white));
		Go.to(txtIntroText, introFadeInDuration, new GoTweenConfig().colorProp("color", Color.white));

		yield return new WaitForSeconds(introElementdsDelay);

		Go.to(imgScreenshot, introFadeInDuration, new GoTweenConfig().colorProp("color", Color.white));

		yield return new WaitForSeconds(introElementdsDelay * 3f);

		txtIntroPressToPlay.enabled = true;
		Go.to(txtIntroPressToPlay, introFadeInDuration / 4f, new GoTweenConfig().colorProp("color", Color.white));

		yield return new WaitForSeconds(introFadeInDuration / 3f);

		Go.to(txtIntroPressToPlay, introFadeInDuration / 4f, new GoTweenConfig()
			.colorProp("color", pressToPlayColor)
			.setIterations(-1, GoLoopType.PingPong)
		);
	}

	private IEnumerator FinishIntroAnimation()
	{
		txtIntroPressToPlay.enabled = false;

		var invisibleColor = Color.white;
		invisibleColor.a = 0f;
		Go.to(imgPoster, introFadeOutDuration, new GoTweenConfig().colorProp("color", invisibleColor));
		Go.to(imgScreenshot, introFadeOutDuration, new GoTweenConfig().colorProp("color", invisibleColor));
		Go.to(txtIntroTitle, introFadeOutDuration, new GoTweenConfig().colorProp("color", invisibleColor));
		Go.to(txtIntroText, introFadeOutDuration, new GoTweenConfig().colorProp("color", invisibleColor));
		var panelColor = Color.black;
		panelColor.a = 0f;
		Go.to(introPanel, introFadeOutDuration, new GoTweenConfig().colorProp("color", panelColor));

		yield return new WaitForSeconds(introFadeOutDuration);

		introPanel.gameObject.SetActive(false);
		State = GameState.Menu;
	}

	private IEnumerator LevelUpAnimation()
	{
		txtLevelUp.rectTransform.localScale = Vector3.one * 0.3f;

		yield return new WaitForEndOfFrame();

		Go.to(txtLevelUp.rectTransform, levelUpMoveInDuration, new GoTweenConfig()
			.vector3Prop("localScale", Vector3.one)
			.setEaseType(GoEaseType.BackOut)
			.setIterations(2, GoLoopType.PingPong)
		);

		// fade in and out
		var fullColor = txtLevelUp.color;
		fullColor.a = 1f;
		Go.to(txtLevelUp, levelUpMoveInDuration, new GoTweenConfig()
			.colorProp("color", fullColor)
			.setEaseType(GoEaseType.Linear)
			.setIterations(2, GoLoopType.PingPong)
		);
	}

	#region Public API

	public void FinishIntro()
	{
		StopCoroutine(introCouroutine);
		StartCoroutine(FinishIntroAnimation());
	}

	public void StartNewGame()
	{
		SetMenuActive(false);
		Progress = 0.75f;
		Score = 0; txtScore.text = "SCORE: 0";
		Level = 1; txtLevel.text = "LEVEL 1";
		State = GameState.Playing;
		FoodManager.I.StartNewGame();
	}

	public void EndGame()
	{
		State = GameState.EndGame;
		txtMenuTitle.text = Level == maxLevel ? "BRAVO FOR LEVEL 5!" : "←A        GAME OVER      D→";
		SetMenuActive(true);
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

		// activate level up
		StartCoroutine(LevelUpAnimation());
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

	public void PlayIntroAgain()
	{
		introCouroutine = IntroAnimation();
		StartCoroutine(introCouroutine);
	}

	#endregion
}
