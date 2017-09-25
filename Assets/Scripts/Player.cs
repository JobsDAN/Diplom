using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	static Color[] defaultColors = { Color.red, Color.blue, Color.green };
	static List<Color> colors;
	static List<Player> players;
	public Color Color
	{
		get { return colors[playerId]; }
	}

	public int playerId
	{
		get; private set;
	}


	static Player()
	{
		colors = new List<Color>(defaultColors);
		players = new List<Player>();
	}

	static void AddRandomColor()
	{
	}

	private Player(int id)
	{
		playerId = id;
	}

	public static Player NewPlayer()
	{
		Player p = new Player(players.Count);
		players.Add(p);
		if (players.Count > colors.Count)
			colors.Add(new Color(Random.value, Random.value, Random.value, 1.0f));

		return p;
	}

	public static Player GetPlayer(int id)
	{
		return players[id];
	}
}
