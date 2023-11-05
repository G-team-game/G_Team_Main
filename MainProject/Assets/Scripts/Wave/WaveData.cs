using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
	public int waves;
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

