using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class AStar {

	// Dictionary containing all nodes
	private static Dictionary<Point, Node> nodes;

	// Possible movement posisitons from current position (relative positions of tiles 1 unit away from local origin 0, 0, in each cardinal direction)
	private static KeyValuePair<int, int>[] childPositions = new KeyValuePair<int, int>[] {new KeyValuePair<int, int>(0, 1), new KeyValuePair<int, int>(1, 0), new KeyValuePair<int, int>(0, -1), new KeyValuePair<int, int>(-1, 0)};

	private static void CreateNodes() {
		nodes = new Dictionary<Point, Node>();

		foreach (TileScript tile in LevelManager.Instance.Tiles.Values) 
		{
			nodes.Add(tile.GridPosition, new Node(tile));
		}
	}

	// Generates shortest path from start to goal
	public static void GetPath(Point start, Point goal) {
		
		// Only re-generate the node map if it does not already exist
		if (nodes == null) 
		{
			CreateNodes ();
		}

		// A hash table of nodes to be processed
		HashSet<Node> openList = new HashSet<Node> ();

		// A hash table of nodes that have already been processed
		HashSet<Node> closedList = new HashSet<Node> ();

		Stack<Node> finalPath = new Stack<Node>();

		// Reference to starting node
		Node currentNode = nodes [start];

		openList.Add (currentNode);

		while (openList.Count > 0) 
		{
			foreach (KeyValuePair<int, int> pos in childPositions) {
				// pos.Key is one of: -1, 0, 1; represents delta x from local origin 0, 0
				// pos.Value is one of: -1, 1, 1; represents delta y from local origin 0, 0

				// eg. if currentNode is at x = 10, y = 10, we will examine all of:
				//		11, 10; 10, 11; 9, 10; 10, 9
				Point neighbourPos = new Point(currentNode.GridPosition.X - pos.Key, currentNode.GridPosition.Y - pos.Value);

				if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble) 
				{
					int gCost = 10;

					Node neighbour = nodes [neighbourPos];

					if (openList.Contains(neighbour))
					{
						if (currentNode.G + gCost < neighbour.G) 
						{
							neighbour.CalcValues (currentNode, nodes [goal], gCost);
						}
						openList.Add (neighbour);
					}
					else if (!closedList.Contains(neighbour))
					{
						openList.Add (neighbour);
						neighbour.CalcValues (currentNode, nodes[goal], gCost);
					}
				}
			}

			// Moves currentNode from openList to closedList
			openList.Remove (currentNode);
			closedList.Add (currentNode);

			if (openList.Count > 0) 
			{
				// Sorts the list by F value, then sets currentNode to the first of the sorted list	
				currentNode = openList.OrderBy (n => n.F).First ();
			}

			if (currentNode == nodes[goal]) 
			{
				while (currentNode.GridPosition != start) 
				{
					finalPath.Push (currentNode);
					currentNode = currentNode.Parent;
				}

				break;
			}
		}

		// ************************* THIS IS FOR DEBUGGING ONLY ************************* \\
		GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(finalPath);
	}
}
