using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.JoystickButton7))
		{
			FoodManager.I.MoveLeft();
		}
		else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.JoystickButton8))
		{
			FoodManager.I.MoveRight();
		}
	}
}
