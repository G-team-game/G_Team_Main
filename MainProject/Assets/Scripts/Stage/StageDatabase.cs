using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(menuName = "StageDatabase")]
    public class StageDatabase : ScriptableObject
    {
        [SerializeField] private List<StageData> stageDatas = new List<StageData>();

        public IReadOnlyList<StageData> stageData => stageDatas;
        public int stageCount => stageDatas.Count;
    }
}