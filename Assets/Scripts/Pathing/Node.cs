using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

	// Position of node on grid
	public Point GridPosition { get; private set; }

	// Reference to tile at GridPosition
	public TileScript TileRef { get; private set; }

	// Reference to parent node
	public Node Parent { get; private set; }

	public int G { get; set; }

	public int H { get; set; }

	public int F { get; set; }

	public Node(TileScript tileRef){
		this.TileRef = tileRef;
		this.GridPosition = tileRef.GridPosition;
	}

	public void CalcValues (Node parent, Node goal, int gCost)
	{
		this.Parent = parent;
		this.G = parent.G + gCost;
		this.H = (System.Math.Abs(GridPosition.X - goal.GridPosition.X) + System.Math.Abs(GridPosition.Y - goal.GridPosition.Y)) * 10;
		this.F = G + H;
	}
}
