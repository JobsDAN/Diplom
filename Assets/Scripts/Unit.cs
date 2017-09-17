using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	static GameObject selectedUnit;
	private Cell currentCell;

	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	const float SELECTION_UP = 0.1f;
	Material material;
	Shader outlineShader;
	Shader defaultShader;

	void Start()
	{
		material = GetComponent<Renderer>().material;
		defaultShader = material.shader;
		outlineShader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
		{
			SelectClick();
		}
		else if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
		{
			MoveClick();
		}

	}

	private void MoveClick()
	{
		if (selectedUnit != transform.gameObject)
			return;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Collider gridCollider = GameObject.Find("Grid").GetComponent<Collider>();
		if (!gridCollider.Raycast(ray, out hit, Mathf.Infinity))
			return;

		if (hit.transform.gameObject == null)
			return;

		Grid grid = GameObject.Find("Grid").GetComponent<Grid> ();
		Cell c = grid.CellFromWorldPosition(hit.point);
		Vector3 newPosition = c.Position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;
	}

	private void SelectClick()
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

	private void Select()
	{
		material.shader = outlineShader;
		material.SetColor("_OutlineColor", Color.red);
		selectedUnit = transform.gameObject;
	}

	private void Unselect()
	{
		material.shader = defaultShader;
		selectedUnit = null;
	}
}
