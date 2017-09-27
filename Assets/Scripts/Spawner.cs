using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField]
    private bool EquipClub;
    [SerializeField]
    private GameObject club;

    [SerializeField]
	private GameObject unitPrefab;
	private Vector3 unitSize;


	[SerializeField]
	private GameObject flockPrefab;

	[SerializeField]
	private float period = 2f;

	[SerializeField]
	private float firstTime = 2f;

	private Vector3 size;
	private Vector3 position;
	private Player player;

	// Use this for initialization
	void Start () {
		player = Player.NewPlayer();
		Renderer unitRenderer = unitPrefab.GetComponent<Renderer>();
		if (unitRenderer == null)
		{
			unitRenderer = unitPrefab.GetComponentInChildren<Renderer>();
		}

		GameObject flag = null;
		GameObject castle = null;
		foreach (Transform child in transform)
		{
			if (child.name == "Flag")
				flag = child.gameObject;
			else if (child.name == "Castle")
				castle = child.gameObject;
		}

		flag.GetComponent<Renderer>().material.color = player.Color;
        size = castle.GetComponent<Renderer>().bounds.size;
		unitSize = unitRenderer.bounds.size;
		position = transform.position;
		InvokeRepeating("SpawnUnit", firstTime, period);
	}
	
	void SpawnUnit()
	{
		float x = 0, z = 0;
		while (Mathf.Abs(x) < size.x / 2 && Mathf.Abs(z) < size.z) {
			x = Random.Range(-size.x * 3, size.x * 3);
			z = Random.Range(-size.z * 3, size.z * 3);
		}

		Vector3 pos = new Vector3(position.x + x, 0, position.z + z);
		Quaternion q = new Quaternion();
		GameObject flockGameObject = Instantiate(flockPrefab, pos, q);
		Flock flock = flockGameObject.GetComponent<Flock>();
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				float ux = i * unitSize.x * 2;
				float uz = j * unitSize.z * 2;
				Vector3 offset = new Vector3(ux, 0, uz);
				Vector3 unitPos = pos + offset;
				GameObject unitGameObject = Instantiate(unitPrefab, unitPos, q);
                Unit unit = unitGameObject.GetComponent<Unit>();

                if (EquipClub)
                {
                    SkinnedMeshRenderer unitMesh = unitGameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                    SkinnedMeshRenderer newMesh = Instantiate(club.GetComponent<SkinnedMeshRenderer>());
                    newMesh.transform.parent = unitMesh.transform;
                    newMesh.bones = unitMesh.bones;
                    newMesh.rootBone = unitMesh.rootBone;
                }

				unit.SetPlayer(player);
				flock.AddUnit(unit, offset);
			}
		}
	}
}