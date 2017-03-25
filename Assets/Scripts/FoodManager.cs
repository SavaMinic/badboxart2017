using UnityEngine;
using System.Collections.Generic;

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

	#endregion

	#region Fields

	private List<FoodItem> activeFoodItems = new List<FoodItem>();

	#endregion

	public void CreateFood()
	{
		var foodItem = (Instantiate(foodItemPrefab) as GameObject).GetComponent<FoodItem>();

		foodItem.transform.parent = transform;
		foodItem.transform.position = Util.Between(startPosition, endPosition, count, activeFoodItems.Count);
		foodItem.transform.LookAt(lookAtPosition);

		activeFoodItems.Add(foodItem);
	}

	public void ClearOldFoodItems()
	{
		for (int i = 0; i < activeFoodItems.Count; i++)
		{
			GameObject.Destroy(activeFoodItems[i].gameObject);
		}
		activeFoodItems.Clear();
	}

	public void StartNewGame()
	{
		ClearOldFoodItems();
		for (int i = 0; i < count; i++)
		{
			CreateFood();
		}
	}

}
