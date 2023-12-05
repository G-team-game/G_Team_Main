using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageDatabase : ScriptableObject
{
    public List<StageData> stageDatas = new List<StageData>();
}

[Serializable]
public class StageData
{
    public int id;
    public string stageName;
    public string stageData;
    public GameObject stageObject;
    public WaveData waveData;
}
