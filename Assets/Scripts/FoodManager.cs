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

	[SerializeField]
	private Vector3 trashCanPosition;

	[SerializeField]
	private Vector3 mouthPosition;

	#endregion

	#region Fields

	private List<FoodItem> activeFoodItems = new List<FoodItem>();

	#endregion

	public Vector3 GetPosition(int order)
	{
		return Util.Between(startPosition, endPosition, count, order);
	}

	public void CreateFood()
	{
		var foodItem = (Instantiate(foodItemPrefab) as GameObject).GetComponent<FoodItem>();

		var order = activeFoodItems.Count;
		foodItem.Init(order, lookAtPosition);
		foodItem.transform.parent = transform;
		foodItem.transform.position = GetPosition(order);
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
