using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	static GameObject selectedUnit;

	// Use this for initialization
	void Start()
	{
		
	}
	
	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	const float SELECTION_UP = 0.1f;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out hit))
				return;

			Unit unit = hit.transform.gameObject.GetComponent<Unit>();
			if (unit == null)
				return;

			GameObject obj = hit.transform.gameObject;
			if (obj != transform.gameObject)
				return;

			if (selectedUnit != null)
				selectedUnit.GetComponent<Unit>().Unselect();

			unit.Select();
		}
		else if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
		{
			if (selectedUnit != transform.gameObject)
				return;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out hit))
				return;

			if (hit.transform.gameObject != null)
				Debug.Log ("Hit something");

			Grid grid = GameObject.Find("Grid").GetComponent<Grid> ();
			Cell c = grid.CellFromWorldPosition(hit.point);
			Vector3 newPosition = c.Position;
			newPosition.y = transform.position.y;
			transform.position = newPosition;
		}

	}

	private void Select()
	{
		Vector3 p = transform.position;
		p.y += SELECTION_UP;
		transform.position = p;

		selectedUnit = transform.gameObject;
	}

	private void Unselect()
	{
		Vector3 p = transform.position;
		p.y -= SELECTION_UP;
		transform.position = p;

		selectedUnit = null;
	}
}
