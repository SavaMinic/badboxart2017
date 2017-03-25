using UnityEngine;
using System.Collections;

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
}
