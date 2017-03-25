using UnityEngine;
using System.Collections.Generic;
using System;

public class FoodManager : MonoBehaviour
{
	#region Instance

	private static FoodManager instance;

	public static FoodManager I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<FoodManager>();
			}
			return instance;
		}
	}

	#endregion

	#region Editor fields

	[SerializeField]
	private GameObject foodItemPrefab;

	[SerializeField]
	private Vector3 startPosition;

	[SerializeField]
	private Vector3 endPosition;

	[SerializeField]
	private int count;

	[SerializeField]
	private Vector3 lookAtPosition;

	[SerializeField]
	private Vector3 trashCanPosition;

	[SerializeField]
	private Vector3 mouthPosition;

	#endregion

	#region Just debug

	[Serializable]
	public class FoodColor
	{
		public FoodItem.FoodType type;
		public Color color;
	}

	public List<FoodColor> foodColors;

	public Color GetFoodColor(FoodItem.FoodType type)
	{
		return foodColors.Find(fc => fc.type == type).color;
	}

	#endregion

	#region Fields

	private List<FoodItem> activeFoodItems = new List<FoodItem>();
	private List<FoodItem.FoodType> possibleFoodTypes = new List<FoodItem.FoodType>();

	#endregion

	public Vector3 GetPosition(int order)
	{
		return Util.Between(startPosition, endPosition, count, order);
	}


	private void CreateFood()
	{
		var foodItem = (Instantiate(foodItemPrefab) as GameObject).GetComponent<FoodItem>();

		var order = activeFoodItems.Count;
		foodItem.Init(order, lookAtPosition, possibleFoodTypes.GetRandom());
		foodItem.transform.parent = transform;
		foodItem.transform.position = GetPosition(order);
		foodItem.transform.LookAt(lookAtPosition);

		activeFoodItems.Add(foodItem);
	}

	private void ClearOldFoodItems()
	{
		for (int i = 0; i < activeFoodItems.Count; i++)
		{
			GameObject.Destroy(activeFoodItems[i].gameObject);
		}
		activeFoodItems.Clear();
	}

	private void AddRandomFood(bool isHealthyFood)
	{
		FoodItem.FoodType type;
		do
		{
			type = isHealthyFood ? FoodItem.HealthyFood.GetRandom() : FoodItem.FastFood.GetRandom();
		}
		while (possibleFoodTypes.Contains(type));
		possibleFoodTypes.Add(type);
	}

	private void ClearPossibleFoodTypes()
	{
		possibleFoodTypes.Clear();
		AddRandomFood(true); AddRandomFood(true);
		AddRandomFood(false); AddRandomFood(false);

		FoodMarkers.I.Reset();
		for (int i = 0; i < possibleFoodTypes.Count; i++)
		{
			var food = possibleFoodTypes[i];
			FoodMarkers.I.ActivateFoodMarker(GetFoodColor(food), food.IsHealthyFood());
		}
	}

	public void StartNewGame()
	{
		ClearOldFoodItems();
		ClearPossibleFoodTypes();
		for (int i = 0; i < count; i++)
		{
			CreateFood();
		}
	}

	public void MoveLeft()
	{
		Move(trashCanPosition);
	}

	public void MoveRight()
	{
		Move(mouthPosition);
	}

	private void Move(Vector3 endPosition)
	{
		activeFoodItems[0].MoveToEnd(endPosition);
		activeFoodItems.RemoveAt(0);
		for (int i = 0; i < activeFoodItems.Count; i++)
		{
			activeFoodItems[i].MoveDown();
		}
		CreateFood();
	}

}
