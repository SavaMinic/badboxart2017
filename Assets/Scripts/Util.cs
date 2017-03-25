using UnityEngine;
using System.Collections.Generic;

public static class Util
{

	public static float Between(float start, float end, int count, int order)
	{
		var distance = end - start;
		var dx = distance / count;
		return start + order * dx;
	}

	public static Vector3 Between(Vector3 start, Vector3 end, int count, int order)
	{
		return new Vector3(
			Between(start.x, end.x, count, order),
			Between(start.y, end.y, count, order),
			Between(start.z, end.z, count, order)
		);
	} 

	public static T GetRandom<T>(this List<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}

	public static bool IsHealthyFood(this FoodItem.FoodType type)
	{
		return type >= FoodItem.FoodType.Cucumber;
	}

	public static bool IsFastFood(this FoodItem.FoodType type)
	{
		return !type.IsHealthyFood();
	}
}
