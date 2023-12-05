using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    [Serializable]
    public struct waveData
    {
        public Transform respawnPoint;
        public EnemyBase enemyBase;
    }
    public List<waveData> waveDatas = new List<waveData>();
    public ItemScript waveStartItem;
}
