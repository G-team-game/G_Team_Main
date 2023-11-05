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

    [SerializeField] private WaveData wavedata;
    [SerializeField] public GameObject canvas;

    [SerializeField] public int Wavecount;
    [SerializeField] public int enemycount;

    [SerializeField] public EnemySpawn enemyspawn;
    private void Awake()
    {
        enemycount = wavedata.wave[0].spawnlist[0].SpawnPoints.Length;
        wavecount.text = Wavecount.ToString();
        wave.text = wavedata.waves.ToString();
        enemycounts.text = enemycount.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            Debug.Log("hit");
            enemyspawn.StartSpawnEney();
            enemyspawn.SelectChaseEnemy();
            canvas.SetActive(true);
            
        }
        if (collision.gameObject.CompareTag("enemy"))
        {
            enemycount--;
            enemycounts.text = enemycount.ToString();

            Debug.Log("hit");
        }
        

    }

    private void Update()
    {
        if(enemycount == 0)
        {
            Wavecount++;
            if(Wavecount-1 == wavedata.wave.Count)
            {
                
            }
            enemycount = wavedata.wave[Wavecount-1].spawnlist[0].SpawnPoints.Length;
            wavecount.text = Wavecount.ToString();
            enemycounts.text = enemycount.ToString();

            enemyspawn.StartSpawnEney();
            enemyspawn.SelectChaseEnemy();
        }
    }
    

}
