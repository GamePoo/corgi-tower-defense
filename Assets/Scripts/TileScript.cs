
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

	public Point GridPosition { get; private set; }

	public bool WalkAble { get; set; }

	public Vector2 WorldPosition {
		get {
			return new Vector2 (transform.position.x + GetComponent<SpriteRenderer> ().bounds.size.x / 2, transform.position.y - GetComponent<SpriteRenderer> ().bounds.size.y / 2);
		}
	}


	// Use this for initialization
	void Start () {
		WalkAble = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
	{
		this.GridPosition = gridPos;
		transform.position = worldPos;

		//Sets tiles as children of Map
		transform.SetParent (parent);

		//Puts tile into dictionary in LevelManager
		LevelManager.Instance.Tiles.Add (gridPos, this);
	}

	private void OnMouseOver() 
	{
		if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
		{
			
			if (Input.GetMouseButtonDown (0)) {
				PlaceTower ();
			}
		}

	}

	private void PlaceTower()
	{
		GameObject tower = (GameObject)Instantiate (GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
		tower.GetComponent<SpriteRenderer> ().sortingOrder = GridPosition.Y;

		tower.transform.SetParent (transform);

		Hover.Instance.Deactivate ();

		GameManager.Instance.BuyTower();

		// After placing a tower on a tile, it is no longer walkable
		WalkAble = false;
	}
}
