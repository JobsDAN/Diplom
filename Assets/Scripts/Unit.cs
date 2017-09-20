using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	static GameObject selectedUnit;
	Cell currentCell;

	const int LEFT_MOUSE_BUTTON = 0;
	const int RIGHT_MOUSE_BUTTON = 1;

	const float SELECTION_UP = 0.1f;
	Material material;
	Shader outlineShader;
	Shader defaultShader;

	Queue<Vector3> path;
	float speed = 0.05f;
	void Awake()
	{
		path = new Queue<Vector3>();
	}

	void Start()
	{
		material = GetComponent<Renderer>().material;
		defaultShader = material.shader;
		outlineShader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
	}

	void Update()
	{
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

	public void Select()
	{
		material.shader = outlineShader;
		material.SetColor("_OutlineColor", Color.red);
	}

	public void Unselect()
	{
		material.shader = defaultShader;
	}

	public void MoveTo(Vector3 dest)
	{
		path.Clear();
		path.Enqueue(dest);
	}

	public Vector3 Position { get {
		return transform.position;
	} }
}
