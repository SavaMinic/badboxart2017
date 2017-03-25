using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorExtensions
{

	[MenuItem("BAD BOX ART/New game")]
	private static void NewGame()
	{
		if (!Application.isPlaying)
			return;
		GameManager.I.StartNewGame();
	}

	[MenuItem("BAD BOX ART/Level up")]
	private static void LevelUp()
	{
		if (!Application.isPlaying)
			return;
		GameManager.I.LevelUp();
	}
}
