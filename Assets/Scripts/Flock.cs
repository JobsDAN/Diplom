using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

	static private Flock selectedFlock;

	private float speed = 0.05f;
	private Queue<Vector3> path;

	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	Collider groundCollider;
	Grid grid;

	void Start() {
		GameObject groundGameObject = GameObject.Find("Grid");
		groundCollider = groundGameObject.GetComponent<Collider>();
		grid = groundGameObject.GetComponent<Grid>();
		path = new Queue<Vector3>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
			SelectClick();
			return;
		}

		if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON)) {
			MoveClick();
			return;
		}

		if (path.Count == 0) {
			return;
		}

		Vector3 dest = path.Peek();
		Vector3 direction = dest - transform.position;
		Vector3 step = direction.normalized * speed;
		float time = Time.deltaTime;
		float distThisFrame = speed * time;
		if (direction.magnitude < step.magnitude) {
			path.Dequeue();
			transform.position = dest;
		}

		transform.Translate(step);
	}

	private void MoveClick() {
		if (selectedFlock != this) {
			return;
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity)) {
			return;
		}

		if (hit.transform.gameObject == null) {
			return;
		}

		Cell c = grid.CellFromWorldPosition(hit.point);
		Vector3 newPosition = c.Position;
		newPosition.y = transform.position.y;
		path.Enqueue(newPosition);
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
