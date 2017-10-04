using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

	static private Flock selectedFlock;

	private Vector3 destination;

	private List<Unit> units;
	private List<Vector3> positions;

	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	Collider groundCollider;

	void Awake() {
		units = new List<Unit>();
		positions = new List<Vector3>();
	}

	void Start() {
		GameObject groundGameObject = GameObject.Find("Ground");
		groundCollider = groundGameObject.GetComponent<Collider>();
	}

	void Update () {
		if (units.Count == 0)
		{
			Destroy(this);
			return;
		}

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

	private bool IsDestinationReached
	{
		get { return destination.x == 0 && destination.y == 0 && destination.z == 0; }
	}

	private void FollowUnits()
	{
		if (IsDestinationReached)
		{
			return;
		}

		Vector3 centerOfMass = new Vector3();
		foreach (Unit unit in units)
		{
			centerOfMass += unit.Position;
		}

		centerOfMass /= units.Count;
		transform.position = centerOfMass;
	}

	private void MoveClick() {
		if (selectedFlock != this) {
			return;
		}

		if (selectedFlock.units[0].Player != Player.SelfPlayer)
		{
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
		for (int i = 0; i < units.Count; i++) {
			units[i].MoveTo(destination + positions[i]);
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

	public void AddUnit(Unit unit, Vector3 pos) {
		units.Add(unit);
		positions.Add(pos);
	}
}
