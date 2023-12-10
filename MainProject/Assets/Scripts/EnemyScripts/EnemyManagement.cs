using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyManagement : MonoBehaviour
{
    public static EnemyManagement Instance;
    private List<EnemyBase> enemies = new List<EnemyBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of EnemySpawn already exists.");
        }
    }

    public void SpawnEney(int stageCount, int waveIndex)
    {
        // var wave = stageDatabase.stageDatas[stageCount].waveData.wave[waveIndex];
        // int childCount = wave.spawnPoints.transform.childCount;
        // List<Transform> points = new List<Transform>();
        // for (int i = 0; i < childCount; i++)
        // {
        //     Transform childTransform = wave.spawnPoints.transform.GetChild(i);
        //     points.Add(childTransform);
        // }

        // for (int i = 0; i < wave.enemylist.Count; i++)
        // {
        //     var enemy = wave.enemylist[i];
        //     var newEnemy = Instantiate(enemy.enemyObject, points[i].position, Quaternion.identity);
        //     newEnemy.name = enemy.enemyName + (enemies.Count + 1);
        //     enemies.Add(newEnemy);
        // }
    }

    public void SelectDashEnemy(GameObject enemyobject)
    {
        if (enemies.All(e => e.getEnemyType == EnemyType.dash && e.gameObject == enemyobject))
        {
            enemies.Where(e => e.gameObject == enemyobject).FirstOrDefault()._enemyState = EnemyState.tracking;
        }
    }
}
