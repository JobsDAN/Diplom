using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BuildSystem : MonoBehaviour {

	[SerializeField]
	GameObject boxPrefab;

	[SerializeField]
	Material avaliableMaterial;

	[SerializeField]
	Material unavaliableMaterial;

	GameObject currentObject;
	Material currentMaterial;

	Collider groundCollider;

	void Start() {
		GameObject groundGameObject = GameObject.Find("Ground");
		groundCollider = groundGameObject.GetComponent<Collider>();
	}
	
	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	bool BuildAvaliable()
	{
		Vector3 size = boxPrefab.GetComponent<Renderer>().bounds.size;
		Vector3 pos = currentObject.transform.position;
		Quaternion q = currentObject.transform.rotation;
		foreach (Collider coll in Physics.OverlapBox(pos, size / 1.5f, q))
		{
			if (coll == currentObject.GetComponent<Collider>())
				continue;

			if (coll.gameObject.tag == "Building")
				return false;
		}

		return true;
	}

	void CancelBuilding()
	{
		Destroy(currentObject);
		currentObject = null;
	}

	void FollowMouse()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity))
			return;

		Renderer renderer = currentObject.GetComponent<Renderer>();
		if (!renderer)
			return;

		Vector3 size = renderer.bounds.size;
		Vector3 pos = hit.point;
		pos.y = size.y / 2;
		currentObject.transform.position = pos;
	}

	void ColorizeBuilding(bool avaliable)
	{
		Renderer renderer = currentObject.GetComponent<Renderer>();
		renderer.material = avaliable ? avaliableMaterial : unavaliableMaterial;
	}

	void PlaceBuilding()
	{
		currentObject.GetComponent<NavMeshObstacle>().enabled = true;
		currentObject.GetComponent<Renderer>().material = currentMaterial;
		currentObject = null;
	}

	void Update () {
		// Follow mouse
		if (!currentObject)
			return;

		// Right mouse click to cancel building
		if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
		{
			CancelBuilding();
			return;
		}

		FollowMouse();
		bool avaliable = BuildAvaliable();
		ColorizeBuilding(avaliable);
	
		// Place on left mouse click
		if (avaliable &&
			Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON) &&
			// Avoid build under UI
			!EventSystem.current.IsPointerOverGameObject())
		{
			PlaceBuilding();
		}
	}

	public void BuildBox() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!groundCollider.Raycast(ray, out hit, Mathf.Infinity))
			return;

		if (currentObject != null)
		{
			CancelBuilding();
			return;
		}

		Quaternion q = new Quaternion();
		currentObject = Instantiate<GameObject>(boxPrefab, hit.point, q);
		currentMaterial = currentObject.GetComponent<Renderer>().material;
	}
}
