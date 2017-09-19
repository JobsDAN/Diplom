using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

	static private Flock selectedFlock;

	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	void Update () {
		if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
			SelectClick();
		} else if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON)) {
			MoveClick();
		}

	}

	private void MoveClick() {
		if (selectedFlock != transform.gameObject) {
			return;
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Collider gridCollider = GameObject.Find("Grid").GetComponent<Collider>();
		if (!gridCollider.Raycast(ray, out hit, Mathf.Infinity)) {
			return;
		}

		if (hit.transform.gameObject == null) {
			return;
		}

		Grid grid = GameObject.Find("Grid").GetComponent<Grid> ();
		Cell c = grid.CellFromWorldPosition(hit.point);
		Vector3 newPosition = c.Position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;
	}

	private void SelectClick() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Collider coll = GetComponent<BoxCollider>();
		if (!coll.Raycast(ray, out hit, Mathf.Infinity)) {
			return;
		}

		Flock flock = hit.transform.gameObject.GetComponent<Flock>();
		if (flock == null) {
			return;
		}

		GameObject obj = hit.transform.gameObject;
		if (obj != transform.gameObject) {
			return;
		}

		if (selectedFlock != null) {
			selectedFlock.Unselect();
		}

		flock.Select();
	}

	private void Select() {
		foreach (Transform child in transform) {
			Unit unit = child.gameObject.GetComponent<Unit>();
			unit.Select();
		}

		selectedFlock = this;
	}

	private void Unselect() {
		foreach (Transform child in transform) {
			Unit unit = child.gameObject.GetComponent<Unit>();
			unit.Unselect();
		}

		selectedFlock = null;
	}
}
