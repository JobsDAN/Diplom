using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{

	const float baseSpeed = 0.025f;
	const int boundary = 40;

	const int minAltitude = 10;
	const int maxAltitude = 40;

	const int scrollSpeedFactor = 50;
	const int edgeSpeedFactor = 50;

	const int inversion = -1;

	int screenWidth;
	int screenHeight;
	
	void Start ()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	bool IsZoomPossible()
	{
		float y = transform.position.y;
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		return (scroll < 0 && y < maxAltitude) 
		    || (scroll > 0 && y > minAltitude);
	}

	void Update ()
	{
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		float speed = baseSpeed * transform.position.y;
		if (Input.GetMouseButton(2))
		{
			float h = inversion * speed * Input.GetAxis("Mouse Y");
			float v = inversion * speed * Input.GetAxis("Mouse X");
			transform.Translate(v, h, h);
			return;
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (IsZoomPossible())
		{
			float dist = scroll * speed * scrollSpeedFactor;
			transform.Translate(0, 0, dist);
			return;
		}

		float delta = speed * Time.deltaTime * edgeSpeedFactor;
		if (screenWidth - boundary < x && x < screenWidth)
			transform.position += Vector3.right * delta;

		if (0 < x && x < boundary)
			transform.position += Vector3.left * delta;

		if (screenHeight - boundary < y && y < screenHeight)
			transform.position += Vector3.forward * delta;

		if (0 < y && y < boundary)
			transform.position += Vector3.back * delta;
	}
}
