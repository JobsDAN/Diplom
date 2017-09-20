using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

	static private Flock selectedFlock;

	private Vector3 destination;

	private List<Unit> units;

	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	Collider groundCollider;

	float stopDistance = 1f;

	void Awake() {
		units = new List<Unit>();
	}

	void Start() {
		GameObject groundGameObject = GameObject.Find("Ground");
		groundCollider = groundGameObject.GetComponent<Collider>();
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

		FollowUnits();
	}

	private void FollowUnits() {
		if (units.Count == 0) {
			return;
		}

		Vector3 centerOfMass = new Vector3();
		foreach (Unit unit in units) {
			centerOfMass += unit.Position;
		}

		centerOfMass /= units.Count;
		transform.position = centerOfMass;
		float dist = Vector3.Distance(centerOfMass, destination);
		if (dist < stopDistance) {
			foreach (Unit unit in units) {
				unit.StopMoving();
			}
		}
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

		destination = hit.point;
		foreach (Unit unit in units) {
			unit.MoveTo(destination);
		}
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
		foreach (Unit unit in units) {
			unit.Select();
		}

		selectedFlock = this;
	}

	private void Unselect() {
		foreach (Unit unit in units) {
			unit.Unselect();
		}

		selectedFlock = null;
	}

	public void AddUnit(Unit unit) {
		units.Add(unit);
	}
}
