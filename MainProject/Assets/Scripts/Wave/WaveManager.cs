using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text wavecount;
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text enemycounts;
    [SerializeField] public GameObject allclear;


    [SerializeField] private WaveData wavedata;
    [SerializeField] public GameObject canvas;

    [SerializeField] public int Wavecount = 1;
    [SerializeField] public int enemycount;

    [SerializeField] public EnemyManagement enemyspawn;
    private void Awake()
    {
        enemycount = wavedata.wave[Wavecount-1].spawnlist[0].SpawnPoints.Length+wavedata.wave[Wavecount-1].spawnlist[1].SpawnPoints.Length+ wavedata.wave[Wavecount-1].spawnlist[2].SpawnPoints.Length;
        wavecount.text = Wavecount.ToString();
        wave.text = wavedata.waves.ToString();
        enemycounts.text = enemycount.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            Debug.Log("hit");
            //spawnlist[0]‚ÍwanderingEnemy‚Ìspawnpoints,spawnlist[1]‚ÍchasingEnemy,spawnlist[2]‚ÍfloatingEnemy
            enemyspawn.StartSpawnEney(wavedata.wave[Wavecount - 1].spawnlist[0].SpawnPoints.Length, wavedata.wanderingEnemyPrefab, wavedata.wanderingEnemyName, enemyspawn.wanderingList);//wanderingenemy‚ðspawn
            enemyspawn.StartSpawnEney(wavedata.wave[Wavecount - 1].spawnlist[1].SpawnPoints.Length, wavedata.chasingEnemyPrefab, wavedata.chasingEnemyName, enemyspawn.wanderingList);//chasingEnemy‚ðspawn
            enemyspawn.StartSpawnEney(wavedata.wave[Wavecount - 1].spawnlist[2].SpawnPoints.Length, wavedata.floatingEnemyPrefab, wavedata.floatingEnemyName, enemyspawn.floatList);//floatingEnemy‚ðspawn



            canvas.SetActive(true);
            
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemycount--;
            enemycounts.text = enemycount.ToString();

            Debug.Log("hit");
        }
        if (collision.gameObject.CompareTag("WanderEnemy"))
        {
            enemycount--;
            enemycounts.text = enemycount.ToString();
        }
        

    }

    private void Update()
    {
        if(enemycount == 0)
        {
            Wavecount++;
            if(Wavecount-1 == wavedata.wave.Count)
            {
                allclear.SetActive(true);
                

            }
            enemycount = wavedata.wave[Wavecount-1].spawnlist[0].SpawnPoints.Length+wavedata.wave[Wavecount-1].spawnlist[1].SpawnPoints.Length+ wavedata.wave[Wavecount-1].spawnlist[2].SpawnPoints.Length;
            
            wavecount.text = Wavecount.ToString();
            enemycounts.text = enemycount.ToString();

            enemyspawn.StartSpawnEney(wavedata.wave[Wavecount - 1].spawnlist[0].SpawnPoints.Length, wavedata.wanderingEnemyPrefab, wavedata.wanderingEnemyName, enemyspawn.wanderingList);
            enemyspawn.StartSpawnEney(wavedata.wave[Wavecount - 1].spawnlist[1].SpawnPoints.Length, wavedata.chasingEnemyPrefab, wavedata.chasingEnemyName, enemyspawn.wanderingList);//chasingEnemy‚ðspawn
            enemyspawn.StartSpawnEney(wavedata.wave[Wavecount - 1].spawnlist[2].SpawnPoints.Length, wavedata.floatingEnemyPrefab, wavedata.floatingEnemyName, enemyspawn.floatList);//floatingEnemy‚ðspawn

        }
    }
    

}
