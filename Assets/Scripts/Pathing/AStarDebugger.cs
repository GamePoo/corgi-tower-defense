using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour {

	// Provides start and end tiles in Unity
	[SerializeField]
	private TileScript start, goal;

	[SerializeField]
	private int pathLength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ClickTile ();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			AStar.GetPath (start.GridPosition, goal.GridPosition);
		}
	}

	// Sets start to clicked tile on first click, and goal to clicked tile on second click
	// Clicks on non-TileScript objects will be ignored
	private void ClickTile() {
		if (Input.GetMouseButtonDown(1)) 
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			if (hit.collider != null) 
			{
				TileScript tmp = hit.collider.GetComponent<TileScript> ();

				if (tmp != null) 
				{
					if (start == null)
					{
						start = tmp;
					} 
					else if (goal == null)
					{
						goal = tmp;
					}
				}
			}
		}
	}

	public void DebugPath(Stack<Node> path) {
		this.pathLength = path.Count;

	}
}
