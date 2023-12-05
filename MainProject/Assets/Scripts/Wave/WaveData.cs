using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
	public int stageId;
	public List<wave> wave = new List<wave>();
}

[Serializable]
public class wave
{
	public Transform spawnItemPosition;
	public ItemScript spwanItem;
	public Transform spawnPoints;
	public List<enemy> enemylist = new List<enemy>();

	[Serializable]
	public class enemy
	{
		public int id;
		public EnemyBase enemyObject;
		public string enemyName;
	}

}

