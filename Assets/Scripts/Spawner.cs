using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField]
	private GameObject unitPrefab;
	private Vector3 unitSize;

	[SerializeField]
	private GameObject flockPrefab;

	[SerializeField]
	private float period = 2f;

	private Vector3 size;
	private Vector3 position;
	// Use this for initialization
	void Start () {
		size = GetComponent<Renderer>().bounds.size;
		unitSize = unitPrefab.GetComponent<Renderer>().bounds.size;
		position = transform.position;
		float firstTime = period;
		InvokeRepeating("SpawnUnit", firstTime, period);
	}
	
	void SpawnUnit()
	{
		float x = 0, z = 0;
		while (Mathf.Abs(x) < size.x / 2 && Mathf.Abs(z) < size.z)
		{
			x = Random.Range(-size.x * 3, size.x * 3);
			z = Random.Range(-size.z * 2, size.z * 2);
		}
		Vector3 pos = new Vector3(position.x + x, 0.5f, position.z + z);
		Quaternion q = new Quaternion();
		GameObject flock = Instantiate<GameObject>(flockPrefab, pos, q);
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				float ux = i * unitSize.x * 2;
				float uz = j * unitSize.z * 2;
				Vector3 offset = new Vector3(ux, 0, uz);
				Vector3 unitPos = pos + offset;
				GameObject unit = Instantiate<GameObject>(unitPrefab, unitPos, q);
				unit.transform.parent = flock.transform;
			}
		}
	}
}