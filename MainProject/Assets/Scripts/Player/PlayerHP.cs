using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int hp = 100;
    private int maxHp = 0;
    private  int lastHP;
    public float addTimer = 5;
    bool isAdd = false;

    public GameObject panel;

    float addhpTimer = 1;
    public int addHPValue=3;
    public float addHpTimerValue = 2;

    private void Awake()
    {
        maxHp = hp;        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        addTimer -= Time.deltaTime;

        if(addTimer<=0)
        {
            isAdd = true;
        }

        if (isAdd)
        {
            addhpTimer += Time.deltaTime;
            if (addhpTimer >= addHpTimerValue)
            {
                if (hp + addHPValue >= maxHp)
                {
                    hp = maxHp;
                    isAdd = false;
                    playerHealth.UpdateAddHP(hp, maxHp, hp);
                    return;
                }
                addhpTimer = 0;
                playerHealth.UpdateAddHP(hp + addHPValue, maxHp, hp);
                hp += addHPValue;
            }
        }
    }
    public void Hit(int atk)
    {
        isAdd = false;
        addTimer = 5;
          if (hp-atk<=0)
        {
            //Game Over
            panel.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
          else
        {          
            playerHealth.UpdateReduceHP(hp - atk, maxHp, hp);
            hp = hp - atk;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadStart()
    {
       SceneManager.LoadScene(0);
    }
}
