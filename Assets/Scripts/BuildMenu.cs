using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour {

	const int widthN = 5;
	const int heightN = 4;

	const float menuWidthPersent = 1f / 3f;
	const float menuHeightPersent = 1f / 3f;

	public GameObject emptyCell;

	GameObject[,] buttons;
	int screenWidth, screenHeight;

	Object[] avaliableBuildings;
	BuildSystem buildSystem;

	EventTrigger.Entry CreateBuildEvent(GameObject gameObject)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((eventData) => { 
			buildSystem.Build(gameObject);
		});

		return entry;
	}

	void CreateMenu()
	{
		Quaternion q = new Quaternion();
		Vector3 pos = new Vector3(0, 0, 0);
		for (int i = 0; i < heightN; i++)
		{
			for (int j = 0; j < widthN; j++)
			{
				buttons[i, j] = Instantiate<GameObject>(emptyCell, pos, q, transform);
			}
		}

		int count = 0;
		foreach (Object o in avaliableBuildings)
		{
			GameObject gameObject = o as GameObject;
			Building b = gameObject.GetComponent<Building>();
			int i = count / widthN;
			int j = count % widthN;
			EventTrigger.Entry entry = CreateBuildEvent(gameObject);
			buttons[i, j].GetComponent<Image>().sprite = b.Sprite;
			buttons[i, j].GetComponent<EventTrigger>().triggers.Add(entry);
			count++;
		}
	}

	void UpdateMenu()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		float cellWidth = menuWidthPersent * screenWidth / widthN;
		float cellHeight = menuHeightPersent * screenHeight / heightN;

		float cellSide = Mathf.Min(cellWidth, cellHeight);
		float menuWidth = cellSide * widthN;
		float menuHeight = cellSide * heightN;

		float x = screenWidth - menuWidth + cellSide / 2;
		float y = menuHeight - cellSide / 2;
		Vector2 size = new Vector2(cellSide, cellSide);
		for (int i = 0; i < heightN; i++)
		{
			for (int j = 0; j < widthN; j++)
			{
				Vector3 pos = new Vector3(x + j * cellSide, y - i * cellSide, 0);
				buttons[i, j].transform.position = pos;
				buttons[i, j].GetComponent<RectTransform>().sizeDelta = size;
			}
		}

	}
	 
	void Start()
	{
		buildSystem = GameObject.Find("BuildSystem").GetComponent<BuildSystem>();
		buttons = new GameObject[heightN, widthN];
		avaliableBuildings = Resources.LoadAll("Buildings", typeof(GameObject));
		CreateMenu();
		UpdateMenu();
	}

	void Update()
	{
		if (Screen.width != screenWidth || 
		    Screen.height != screenHeight)
		{
			UpdateMenu();
		}
	}
}
