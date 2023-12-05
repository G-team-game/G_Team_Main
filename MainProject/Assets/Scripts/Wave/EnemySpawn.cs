using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instance;
    //[SerializeField] GameObject EnemyPrefab;//���O
    [SerializeField] private WaveData wavedata;//�ǉ�

    //[SerializeField] int EnemySpawnNumbers=5;
    [SerializeField] int ChaseEnemyNumbers = 3;
    [SerializeField] int DashEnemyNumbers = 1;
    [SerializeField] float DashSelectTime = 5f;
    private float timeCount;

    //[SerializeField] private WaveData waveData;

    //[SerializeField] Transform[] SpawnPoints;


    [SerializeField] List<GameObject> SpawnList = new List<GameObject>();
    [SerializeField] List<GameObject> ChaseList = new List<GameObject>();
    [SerializeField] public List<GameObject> DashList = new List<GameObject>();
    private int prefabcount;
    // [SerializeField] public WaveManager wavemanager;

    private void Awake()//�V���O���g���ł���Ă݂����Ċ����ł��B
    {
        //int i = waveData.wave;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of EnemySpawn already exists.");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //if (EnemySpawnNumbers < ChaseEnemyNumbers + DashEnemyNumbers)
        //{
        //    DashEnemyNumbers = EnemySpawnNumbers - ChaseEnemyNumbers;
        //}

        //StartSpawnEney();
        //SelectChaseEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;//�����̃��[�v����InvokeRepeating�ɕς��悤���ȁ[���Ċ����ł��B
        if (timeCount >= DashSelectTime)
        {
            SelectDashEnemy();
            timeCount = 0;
        }

    }

    // public void StartSpawnEney()//�Q�[���X�^�[�g���ɏo���Ƃ��Ɏg��
    // {
    //     for (int i=0;i<wavedata.wave[wavemanager.Wavecount-1].spawnlist[0].SpawnPoints.Length;i++)
    //     {
    //         //int randomIndex = Random.Range(0, SpawnPoints.Length);
    //         int randomIndex = Random.Range(0, wavedata.wave[wavemanager.Wavecount-1].spawnlist[0].SpawnPoints.Length);//�ǉ�
    //         //Transform spawnPoint = SpawnPoints[randomIndex];
    //         Transform spawnPoint = wavedata.wave[wavemanager.Wavecount-1].spawnlist[0].SpawnPoints[randomIndex];//�ǉ�
    //         //GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, Quaternion.identity);
    //         GameObject newEnemy = Instantiate(wavedata.wave[wavemanager.Wavecount-1].enemylist[0].enemytype, spawnPoint.position, Quaternion.identity);//�ǉ�
    //         newEnemy.name = "Enemy"+(prefabcount+1);
    //         SpawnList.Add(newEnemy);
    //         prefabcount++;
    //     }
    // }
    // public void SpawnEnemy()//���Ԃ��ƂɃ����_���ʒu�ɃX�|�[��������Ȃ�g��
    // {
    //     //int randomIndex = Random.Range(0, SpawnPoints.Length);
    //     int randomIndex = Random.Range(0, wavedata.wave[wavemanager.Wavecount-1].spawnlist[0].SpawnPoints.Length);//�ǉ�
    //     //Vector3 spawnPosition = SpawnPoints[randomIndex].position;
    //     Vector3 spawnPosition = wavedata.wave[wavemanager.Wavecount-1].spawnlist[0].SpawnPoints[randomIndex].position;//�ǉ�
    //     //Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
    //     Instantiate(wavedata.wave[wavemanager.Wavecount-1].enemylist[0].enemytype, spawnPosition, Quaternion.identity);
    // }

    public void SelectChaseEnemy()//�X�|�[��������enemy����ǂ���������I��
    {
        List<GameObject> selectedEnemies = new List<GameObject>(SpawnList);
        int count = Mathf.Min(ChaseEnemyNumbers, selectedEnemies.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, selectedEnemies.Count);
            GameObject enemyToChase = selectedEnemies[randomIndex];

            EnemyScript enemyscript = enemyToChase.GetComponent<EnemyScript>();
            if (enemyscript != null)
            {
                ChaseList.Add(enemyToChase);
                enemyscript.EnemyChaseSetActive();
                Debug.Log(enemyToChase.name);
            }
            else
            {
                Debug.LogError("EnemyScript component not found on enemy object: " + enemyToChase.name);
            }

            selectedEnemies.RemoveAt(randomIndex);
        }
    }

    private void SelectDashEnemy()
    {
        DashList.Clear();
        List<int> selectedDash = new List<int>();
        for (int i = 0; i < SpawnList.Count; i++)
            selectedDash.Add(i);
        for (int i = 0; i < DashEnemyNumbers; i++)
        {
            int randomIndex = Random.Range(0, selectedDash.Count);
            int selectedIndex = selectedDash[randomIndex];
            DashList.Add(SpawnList[selectedIndex]);
            selectedDash.RemoveAt(randomIndex);
            Debug.Log(SpawnList[selectedIndex]);
        }

    }
}
