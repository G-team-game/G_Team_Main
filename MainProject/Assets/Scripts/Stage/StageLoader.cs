using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stage
{
    public static class StageLoader
    {
        private static StageDatabase stageDatabase;
        public static StageData CurrentStage { get; private set; }

        public static void InitSettings()
        {
            stageDatabase = Resources.Load<StageDatabase>("StageDatabase");
        }

        public static void Load(int stageId)
        {
            IReadOnlyList<StageData> stageSetting = stageDatabase.stageData;

            //配列範囲内のときのみロードする
            if (0 <= stageId && stageId < stageSetting.Count)
            {
                //インデックス番号をidとして扱う
                CurrentStage = stageSetting[stageId];
                SceneManager.LoadScene("Loading");
            }
            else
            {
                Debug.LogError("指定されたステージIDは存在しないよ");
            }
        }
    }
}