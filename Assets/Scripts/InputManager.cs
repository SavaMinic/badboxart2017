using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

	void Update()
	{
		if (GameManager.I.IsPlaying)
		{
			if (Input.GetKeyDown(KeyCode.A)
				|| Input.GetKeyDown(KeyCode.LeftArrow)
				|| Input.GetKeyDown(KeyCode.JoystickButton7)
			)
			{
				FoodManager.I.MoveToTrash();
			}
			else if (Input.GetKeyDown(KeyCode.D)
				|| Input.GetKeyDown(KeyCode.RightArrow)
				|| Input.GetKeyDown(KeyCode.JoystickButton8)
			)
			{
				FoodManager.I.MoveToMouth();
			}
		}
		else if (GameManager.I.State == GameManager.GameState.Intro && Input.anyKeyDown)
		{
			GameManager.I.FinishIntro();
		}
	}
}
