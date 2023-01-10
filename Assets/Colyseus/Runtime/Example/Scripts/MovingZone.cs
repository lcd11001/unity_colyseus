using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MovingZone : MonoBehaviour
{
	RectTransform rt;
	Vector3[] worldCorners = new Vector3[4];
	Vector3[] screenCorners = new Vector3[4];

	public float Left => screenCorners[(int)CORNER.BOTTOM_LEFT].x;
	public float Right => screenCorners[(int)CORNER.BOTTOM_RIGHT].x;

	public float Bottom => screenCorners[(int)CORNER.BOTTOM_LEFT].y;
	public float Top => screenCorners[(int)CORNER.TOP_LEFT].y;

	// https://docs.unity3d.com/ScriptReference/RectTransform.GetWorldCorners.html
	public enum CORNER: int
	{
		BOTTOM_LEFT,
		TOP_LEFT,
		TOP_RIGHT,
		BOTTOM_RIGHT
	}

	private void Start()
	{
		GetCorners();
	}

	private void GetCorners()
	{
		if (rt == null)
		{
			rt = GetComponent<RectTransform>();
		}
		rt.GetWorldCorners(worldCorners);

		//Debug.Log("Corners");
		for (int i=0; i<4; i++)
		{
			if (Camera.main == null)
			{
				//Debug.Log("Camera is null");
				break;
			}
			screenCorners[i] = Camera.main.WorldToScreenPoint(worldCorners[i]);
			//Debug.Log($" {(CORNER)i} : world {worldCorners[i]} => screen {screenCorners[i]}");
		}
	}

	private void OnRectTransformDimensionsChange()
	{
		GetCorners();
	}
}
