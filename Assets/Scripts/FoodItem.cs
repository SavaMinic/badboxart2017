﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class FoodItem : MonoBehaviour
{

	public enum FoodType
	{
		// Fast food
		Hotdog = 0,
		Pizza,
		Burger,
		Milkshake,
		FrenchFries,

		// Healthy food
		Cucumber = 100,
		Apple,
		Banana,
		Broccoli,
		Lemon,

		// default one
		Nothing = 999,
	}

	[System.Serializable]
	public class FoodSprite
	{
		public FoodType Type;
		public SpriteRenderer Sprite;
	}

	public readonly static List<FoodType> FastFood = new List<FoodType>();
	public readonly static List<FoodType> HealthyFood = new List<FoodType>();
	static FoodItem()
	{
		var list = new List<FoodType>((FoodType[])Enum.GetValues(typeof(FoodType)));
		var cnt = list.IndexOf(FoodType.Cucumber);
		FastFood.AddRange(list.GetRange(0, cnt));
		HealthyFood.AddRange(list.GetRange(cnt, cnt));
	}

	#region Editor fields

	[SerializeField]
	private float moveDownDuration;

	[SerializeField]
	private GoEaseType moveDownEase;

	[SerializeField]
	private float moveToEndDuration;

	[SerializeField]
	private GoEaseType moveToEndEase;

	[SerializeField]
	private List<FoodSprite> foodSprites;

	#endregion

	#region Fields

	private SpriteRenderer sprite;
	private int order;
	private Vector3 lookAtPosition;

	private Vector3 endPosition;
	private GoTween movingAnimation;
	private GoTween colorAnimation;
	private GoTween zoomAnimation;

	#endregion

	#region Properties

	public FoodType Type { get; private set; }

	#endregion

	void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	void OnDestroy()
	{
		if (movingAnimation != null)
		{
			movingAnimation.destroy();
		}
		if (colorAnimation != null)
		{
			colorAnimation.destroy();
		}
		if (zoomAnimation != null)
		{
			zoomAnimation.destroy();
		}
	}

	#region Public API

	public void Init(int initialOrder, Vector3 lookAt, FoodType foodType)
	{
		order = initialOrder;
		lookAtPosition = lookAt;
		Type = foodType;

		for(int i = 0; i < foodSprites.Count; i++)
		{
			foodSprites[i].Sprite.enabled = foodSprites[i].Type == Type;
		}
	}

	public void MoveDown()
	{
		if (movingAnimation != null)
		{
			movingAnimation.destroy();
		}
		order--;
		var nextPosition = FoodManager.I.GetPosition(order);
		movingAnimation = Go.to(transform, moveDownDuration, new GoTweenConfig()
			.vector3Prop("position", nextPosition)
			.setEaseType(moveDownEase)
			.onUpdate(t =>
			{
				transform.LookAt(lookAtPosition);
			})
			.onComplete(t =>
			{
				movingAnimation = null;
				// last ending animation was delayed, call it now
				if (endPosition != Vector3.zero)
				{
					MoveToEnd(endPosition);
				}
			})
		);
	}

	public void MoveToEnd(Vector3 endingPosition)
	{
		// wait for this to finish
		if (movingAnimation != null)
		{
			endPosition = endingPosition;
			return;
		}
		movingAnimation = Go.to(transform, moveToEndDuration, new GoTweenConfig()
			.vector3Prop("position", endingPosition)
			.setEaseType(moveToEndEase)
			.onComplete(t =>
			{
				movingAnimation = null;
				GameObject.Destroy(gameObject);
				// TODO: call callback, open mouth and staff
			})
		);
		var endColor = Color.white;
		endColor.a = 0f;
		colorAnimation = Go.to(sprite, moveToEndDuration, new GoTweenConfig()
			.colorProp("color", endColor)
			.setEaseType(GoEaseType.Linear)
		);

		zoomAnimation = Go.to(transform, moveToEndDuration, new GoTweenConfig()
			.vector3Prop("localScale", Vector3.zero)
			.setEaseType(GoEaseType.Linear)
		);
	}

	#endregion

}
