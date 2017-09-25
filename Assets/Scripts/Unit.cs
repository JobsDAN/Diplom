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

	public Player Player
	{
		get; private set;
	}

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

	void Update() {
		float dist = agent.remainingDistance;
		float stopDist = agent.stoppingDistance;

		bool close = dist <= stopDist;
		bool stop = !agent.hasPath || agent.velocity.sqrMagnitude == 0f;

		if (!agent.pathPending && close && stop)
			StopMoving();
	}

	public void SetPlayer(Player p)
	{
		Player = p;
	}

	public void Select() {
		material.shader = outlineShader;
		material.SetColor("_OutlineColor", Player.Color);
	}

	public void Unselect() {
		material.shader = defaultShader;
	}

	public void MoveTo(Vector3 dest) {
		agent.destination = dest;
        animator.SetBool("IsRun", true);
	}

	public void StopMoving() {
        animator.SetBool("IsRun", false);
    }

    public Vector3 Position {
		get { return transform.position; }
	}
}
