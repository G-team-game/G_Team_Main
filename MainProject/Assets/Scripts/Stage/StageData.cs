using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(menuName = "StageData")]
    public class StageData : ScriptableObject
    {
        [Header("ID")][SerializeField] private int stageId = 0;
        [Header("ステージ名")][SerializeField] private string stageName = "StageName";
        [Header("ステージ説明")][SerializeField] private string stageDescription = "description";
        [Header("ステージのプレハブ")][SerializeField] private GameObject stageObject;
        [Header("何体敵を倒したらゴールできるか?")][SerializeField] private int clearEnemyCount;

        public int StageId => stageId;
        public string StageName => stageName;
        public string StageDescription => stageDescription;
        public GameObject StageObject => stageObject;
        public int ClearEnemyCount => clearEnemyCount;
    }
}