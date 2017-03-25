using UnityEngine;
using System.Collections.Generic;

public class FoodMarkers : MonoBehaviour
{
	#region Instance

	private static FoodMarkers instance;

	public static FoodMarkers I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<FoodMarkers>();
			}
			return instance;
		}
	}

	#endregion

	[SerializeField]
	private List<SpriteRenderer> healthyFood;

	[SerializeField]
	private List<SpriteRenderer> fastFood;

	#region API

	public void Reset()
	{
		for (int i = 0; i < healthyFood.Count; i++)
		{
			healthyFood[i].enabled = false;
		}
		for (int i = 0; i < fastFood.Count; i++)
		{
			fastFood[i].enabled = false;
		}
	}

	public void RefreshFoodMarkers(List<FoodItem.FoodType> possibleFoodTypes)
	{
		Reset();
		for (int i = 0; i < possibleFoodTypes.Count; i++)
		{
			var food = possibleFoodTypes[i];
			ActivateFoodMarker(FoodManager.I.GetFoodColor(food), food.IsHealthyFood());
		}
	}

	public void ActivateFoodMarker(Color color, bool isHealthy)
	{
		var food = (isHealthy ? healthyFood : fastFood).Find(f => !f.enabled);
		food.color = color;
		food.enabled = true;
	}

	#endregion

}
