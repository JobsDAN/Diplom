using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour {

	[SerializeField]
	GameObject boxPrefab;

	GameObject currentObject;

	Collider groundCollider;

	void Start() {
		GameObject groundGameObject = GameObject.Find("Ground");
		groundCollider = groundGameObject.GetComponent<Collider>();
	}
	
	void Update () {
		// Follow mouse
		if (currentObject) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity)) {
				return;
			}

			currentObject.transform.position = hit.point;
		}

		// Place on left mouse click
		if (Input.GetMouseButtonDown(0)) {
			currentObject = null;
		}
	}

	public void BuildBox() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity)) {
			return;
		}

		if (currentObject != null) {
			Destroy(currentObject);
			currentObject = null;
			return;
		}

		Quaternion q = new Quaternion();
		Vector3 size = boxPrefab.GetComponent<Renderer>().bounds.size;
		Vector3 pos = hit.point;
		pos += new Vector3(0, size.y / 2, 0);
		currentObject = Instantiate<GameObject>(boxPrefab, pos, q);
	}
}
