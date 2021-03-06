﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
		
	[SerializeField]
	private GameObject towerPrefab;
	
	public GameObject TowerPrefab
	{
		get
		{
			return towerPrefab;
		}

	}
	
	public TowerBtn ClickedBtn { get; private set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PickTower(TowerBtn towerBtn)
	{
		this.ClickedBtn = towerBtn;
		Hover.Instance.Activate(towerBtn.Sprite);
	}

	public void BuyTower()
	{
		ClickedBtn = null;
	}
}
