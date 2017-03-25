using UnityEngine;
using System.Collections;

public class FoodItem : MonoBehaviour
{

	#region Editor fields

	[SerializeField]
	private float moveDownDuration;

	[SerializeField]
	private GoEaseType moveDownEase;

	[SerializeField]
	private float moveToEndDuration;

	[SerializeField]
	private GoEaseType moveToEndEase;

	#endregion

	#region Fields

	private Renderer myRenderer;
	private int order;
	private GoTween movingAnimation;
	private Vector3 lookAtPosition;

	private Vector3 endPosition;

	#endregion

	void Awake()
	{
		myRenderer = GetComponent<Renderer>();
	}

	public void Init(int initialOrder, Vector3 lookAt)
	{
		order = initialOrder;
		myRenderer.material.color = Random.ColorHSV();
		lookAtPosition = lookAt;
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
			.onUpdate(t =>
			{
				
			})
			.onComplete(t =>
			{
				movingAnimation = null;
				GameObject.Destroy(gameObject);
				// TODO: call callback, open mouth and staff
			})
		);
	}

}
