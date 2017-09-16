using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		
	}
	
	const int LEFT_MOUSE_BUTTON = 0;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.gameObject != null)
				{
					Debug.Log ("Hit something");
				}

				Grid grid = GameObject.Find("Grid").GetComponent<Grid> ();
				Cell c = grid.CellFromWorldPosition(hit.point);
				Vector3 newPosition = c.Position;
				newPosition.y = transform.position.y;
				transform.position = newPosition;
			}
		}

	}
}
