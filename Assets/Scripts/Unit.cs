using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {
	Material material;
	Shader outlineShader;
	Shader defaultShader;

	NavMeshAgent agent;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		material = GetComponent<Renderer>().material;
		defaultShader = material.shader;
		outlineShader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
	}

	Vector3 computeAlignment() {
		return new Vector3();
	}

	void Update() {
	}

	public void Select() {
		material.shader = outlineShader;
		material.SetColor("_OutlineColor", Color.red);
	}

	public void Unselect() {
		material.shader = defaultShader;
	}

	public void MoveTo(Vector3 dest) {
		agent.destination = dest;
	}

	public Vector3 Position {
		get { return transform.position; }
	}
}
