using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {
	Material material;
	Shader outlineShader;
	Shader defaultShader;
	NavMeshAgent agent;
    Animator animator;

	void Awake() {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }

		agent = GetComponent<NavMeshAgent>();
		material = renderer.material;
        animator = GetComponent<Animator>();
    }

    void Start() {
		defaultShader = material.shader;
		outlineShader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
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
		agent.isStopped = false;
        animator.SetBool("IsRun", true);
	}

	public void StopMoving() {
		agent.isStopped = true;
        animator.SetBool("IsRun", false);
    }

    public Vector3 Position {
		get { return transform.position; }
	}

}
