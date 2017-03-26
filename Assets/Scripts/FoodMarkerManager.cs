using UnityEngine;
using System.Collections.Generic;

public class FoodMarkerManager : MonoBehaviour
{
	#region Instance

	private static FoodMarkerManager instance;

	public static FoodMarkerManager I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<FoodMarkerManager>();
			}
			return instance;
		}
	}

	#endregion

	[SerializeField]
	private List<FoodMarker> healthyFood;

	[SerializeField]
	private List<FoodMarker> fastFood;

	#region API

	public void ResetMarkers()
	{
		for (int i = 0; i < healthyFood.Count; i++)
		{
			healthyFood[i].Disable();
		}
		for (int i = 0; i < fastFood.Count; i++)
		{
			fastFood[i].Disable();
		}
	}

	public void RefreshFoodMarkers(List<FoodItem.FoodType> possibleFoodTypes)
	{
		ResetMarkers();
		for (int i = 0; i < possibleFoodTypes.Count; i++)
		{
			var foodType = possibleFoodTypes[i];
			var foodMarker = (foodType.IsHealthyFood() ? healthyFood : fastFood).Find(f => !f.IsActivated);
			foodMarker.Activate(foodType);
		}
	}

	#endregion

}
