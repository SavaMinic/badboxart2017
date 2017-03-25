using UnityEngine;
using System.Collections;

public class FoodItem : MonoBehaviour
{

	private Renderer myRenderer;

	void Awake()
	{
		myRenderer = GetComponent<Renderer>();
		myRenderer.material.color = Random.ColorHSV();
	}

}
