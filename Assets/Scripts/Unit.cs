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
}
