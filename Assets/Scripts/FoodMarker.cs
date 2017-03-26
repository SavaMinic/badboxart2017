using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMarker : MonoBehaviour
{
	public bool IsActivated { get; private set; }

	[SerializeField]
	private List<FoodItem.FoodSprite> foodSprites;

	private SpriteRenderer back;

	void Awake()
	{
		back = GetComponent<SpriteRenderer>();
	}

	public void Disable()
	{
		IsActivated = false;
		back.enabled = false;
		for(int i = 0; i < foodSprites.Count; i++)
		{
			foodSprites[i].Sprite.enabled = false;
		}
	}

	public void Activate(FoodItem.FoodType type)
	{
		IsActivated = true;
		back.enabled = true;
		for(int i = 0; i < foodSprites.Count; i++)
		{
			foodSprites[i].Sprite.enabled = foodSprites[i].Type == type;
		}
	}

}
