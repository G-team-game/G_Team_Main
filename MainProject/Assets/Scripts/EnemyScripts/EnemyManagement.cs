using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    public static EnemyManagement Instance;

   


    //[SerializeField] GameObject wanderingEnemyPrefab; scrptabeleobject��wavedata.cs�Ɉړ�
    //[SerializeField] GameObject chasingEnemyPrefab; ,,
    //[SerializeField] GameObject floatingEnemyPrefab; ,,

    //[SerializeField] int wanderingEnemyNumbers=5;
    //[SerializeField] int chasingEnemyNumbers=3;
    //[SerializeField] int dashEnemyNumbers=1;
    //[SerializeField] int floatingEnemyNumbers = 1;

    //[SerializeField] float dashSelectTime = 5.0f;

    //�G���X�|�[��������Ƃ��̖��O�ł��Bdash�͜p�j���Ă�̂��ω������Ȃ̂ŏ����ĂȂ��ł��B
    //[SerializeField] string wanderingEnemyName = "WanderingEnemy"; scrptabeleobject��wavedata.cs�Ɉړ�
    //[SerializeField] string chasingEnemyName = "ChaseEnemy"; ,,
    //[SerializeField] string floatingEnemyName = "FloatEnemy"; ,,
    [SerializeField] Transform[] spawnPoints;
    
    [SerializeField] public List<GameObject> wanderingList=new List<GameObject>();
    [SerializeField] public List<GameObject> chaseList=new List<GameObject>();
    [SerializeField] public List<GameObject> dashList=new List<GameObject>();
    [SerializeField] public List<GameObject> floatList = new List<GameObject>();
    private int prefabCount;

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
    // Start is called before the first frame update
    void Start()
    {
        //StartSpawnEney(wanderingEnemyNumbers, wavedata.wanderingEnemyPrefab,wanderingEnemyName,wanderingList);//wavedata��������Ă���悤�ɕύX
        //StartSpawnEney(chasingEnemyNumbers, wavedata.chasingEnemyPrefab, chasingEnemyName, chaseList);//,,
        //StartSpawnEney(floatingEnemyNumbers, wavedata.floatingEnemyPrefab, floatingEnemyName, floatList);//,,
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawnEney(int enemyNumbers,GameObject enemyPrefab,string objectName,List<GameObject> enemyList)
    {
        prefabCount = 0;
        for (int i=0;i<enemyNumbers;i++)
        {
            int randomIndex=Random.Range(0,spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            newEnemy.name = objectName+(prefabCount+1);
            Debug.Log(newEnemy.name);
            enemyList.Add(newEnemy);
            prefabCount++;
        }
    }

    public void SelectDashEnemy(GameObject enemyobject)
    {
        if(wanderingList.Contains(enemyobject)&&!dashList.Contains(enemyobject))
        {
            Debug.Log("�_�b�V��");
            dashList.Add(enemyobject);
        }
    }
}
