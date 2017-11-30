using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager> 
{
	//array of tile prefabs, used to create tiles ingame
	// private because public is error prone
	//Creates prefab and makes it accessible in Unity Inspector
	[SerializeField]
	private GameObject[] tilePrefabs; 

	[SerializeField]
	private CameraMovement cameraMovement;

	[SerializeField]
	private Transform map;

	private Point blueSpawn, pinkSpawn;

	//Prefabs for spawning portals
	[SerializeField]
	private GameObject bluePortalPrefab;

	[SerializeField]
	private GameObject pinkPortalPrefab;

	//Dictionary containing all tiles in our game
	public Dictionary<Point, TileScript> Tiles {get; set;}

	private Point mapSize;

	// Property for returning size of tile
	public float TileSize 
	{
		//Finds size of tiles
		get { 
			return tilePrefabs[0].GetComponent<SpriteRenderer> ().sprite.bounds.size.y; }
	}

	void Start () 
	{
		//Creates level
		createLevel ();
		
	}

	void Update () {
		
	}
    

	private void createLevel() 
	{
		Tiles = new Dictionary<Point, TileScript> ();

		string[] mapData = ReadLevelText();

		//Calculates map X size
		int mapX = mapData [0].ToCharArray ().Length;

		//Calculates map Y size
		int mapY = mapData.Length;

		mapSize = new Point (mapX, mapY);

		Vector3 maxTile = Vector3.zero;

		//Calculate world start point = top left corner of screen
		Vector3 worldStart = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height));
		for (int y = 0; y < mapY; y++) //The y positions
		{
			char[] newTiles = mapData [y].ToCharArray (); //Gets all tiles that we need to place
			for (int x = 0; x < mapX; x++) //The x positions
			{
				//Places tile in world
				PlaceTile (newTiles[x].ToString(),x,y,worldStart);
			}
		}

		maxTile = Tiles [new Point (mapX - 1, mapY - 1)].transform.position;

		//Sets camera limit to max tile position
		cameraMovement.SetLimits (maxTile);

		SpawnPortals ();

	}

	private void PlaceTile(string TileType, int x, int y, Vector3 worldStart)
	{
		//Parses the tiletype to an int, so we can use it as an indexer when we create a new tile
		int tileIndex = int.Parse (TileType);

		//Creates a new tile and makes a reference to that tile in the newTile variable
		TileScript newTile = Instantiate (tilePrefabs[tileIndex]).GetComponent<TileScript>();

		if (tileIndex == 0) 
		{
			newTile.WalkAble = true;
		}
		else
		{
			newTile.WalkAble = false;
		}

		//Uses the new tile variable to change position of the tile
		newTile.Setup (new Point (x, y),new Vector3 (worldStart.x + TileSize * x,worldStart.y - TileSize * y, 0), map);
	}

	private string[] ReadLevelText()
	{
		TextAsset bindData = Resources.Load ("Level") as TextAsset;

		string data = bindData.text.Replace (Environment.NewLine, string.Empty);

		return data.Split('-');
	}

	private void SpawnPortals() 
	{
		blueSpawn = new Point (0, 0);

		Instantiate (bluePortalPrefab, Tiles [blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

		pinkSpawn = new Point (17,8);

		Instantiate (pinkPortalPrefab, Tiles [pinkSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
	}

	public bool InBounds(Point position) {
		return position.X >= 0 && position.Y >= 0 && position.X <= mapSize.X && position.Y <= mapSize.Y;
	}
}
