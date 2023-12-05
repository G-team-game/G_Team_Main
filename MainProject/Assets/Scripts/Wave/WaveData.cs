using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
	public int waves;
	[SerializeField] public GameObject wanderingEnemyPrefab;
	[SerializeField] public GameObject chasingEnemyPrefab;
	[SerializeField] public GameObject floatingEnemyPrefab;
	[SerializeField] public string wanderingEnemyName = "WanderingEnemy";
	[SerializeField] public string chasingEnemyName = "ChaseEnemy";
	[SerializeField] public string floatingEnemyName = "FloatEnemy";
	public GameObject Item;
	public List<wave> wave = new List<wave>();

}

[Serializable]
public class wave
{
	public List<enemy> enemylist = new List<enemy>();
	public List<spawn> spawnlist = new List<spawn>();
	//[SerializeField] public Transform[] SpawnPoints;

	[Serializable]
	public class enemy
	{
		public int id;
		public GameObject enemytype;

	}
	[Serializable]
	public class spawn
	{
		public Transform[] SpawnPoints;
	}

}

