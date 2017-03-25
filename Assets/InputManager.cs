using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.JoystickButton7))
		{
			FoodManager.I.MoveLeft();
		}
		else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)|| Input.GetKeyUp(KeyCode.JoystickButton8))
		{
			FoodManager.I.MoveRight();
		}
	}
}
