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

	void Start()
	{
		StartNewGame();
	}

	public void StartNewGame()
	{
		FoodManager.I.StartNewGame();
	}
}
