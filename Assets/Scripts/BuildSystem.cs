using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BuildSystem : MonoBehaviour {

	[SerializeField]
	GameObject boxPrefab;

	GameObject currentObject;

	Collider groundCollider;

	void Start() {
		GameObject groundGameObject = GameObject.Find("Ground");
		groundCollider = groundGameObject.GetComponent<Collider>();
	}
	
	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	void Update () {
		// Follow mouse
		if (currentObject)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity))
			{
				return;
			}

			Vector3 size = boxPrefab.GetComponent<Renderer>().bounds.size;
			Vector3 pos = hit.point;
			pos.y = size.y / 2;

			currentObject.transform.position = pos;
		}

		// Right mouse click to cancel building
		if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
		{
			Destroy(currentObject);
			currentObject = null;
			return;
		}

		// Place on left mouse click
		if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON) &&
			// Avoid build under UI
			!EventSystem.current.IsPointerOverGameObject())
		{
				currentObject.GetComponent<NavMeshObstacle>().enabled = true;
				currentObject = null;
		}
	}

	public void BuildBox() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity))
		{
			return;
		}

		if (currentObject != null)
		{
			Destroy(currentObject);
			currentObject = null;
			return;
		}

		Quaternion q = new Quaternion();
		currentObject = Instantiate<GameObject>(boxPrefab, hit.point, q);
	}
}
